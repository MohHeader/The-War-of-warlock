﻿//$ Copyright 2016, Code Respawn Technologies Pvt Ltd - All Rights Reserved $//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonArchitect.Utils;
using DungeonArchitect.Graphs;

namespace DungeonArchitect
{
	using PropBySocketType_t = Dictionary<string, List<PropTypeData>>;
	using PropBySocketTypeByTheme_t = Dictionary<DungeonPropDataAsset, Dictionary<string, List<PropTypeData>>>;

    /// <summary>
    /// Builds the layout of the dungeon and emits markers around the layout
    /// Implement this class to create your own builder
    /// </summary>
	[ExecuteInEditMode]
    public abstract class DungeonBuilder : MonoBehaviour
    {
        protected DungeonConfig config;
        protected PMRandom nrandom;
        protected PMRandom random;
        protected DungeonModel model;
        protected Vector3 GridToMeshScale;
        protected List<PropSocket> PropSockets = new List<PropSocket>();
        protected int _SocketIdCounter = 0;
        protected Blackboard blackboard = new Blackboard();

        private bool isLayoutBuilt = false;
        public bool IsLayoutBuilt
        {
            get
            {
                {
                    // Hot code reload sometimes invalidates the model.  
                    if (random == null) return false;
                }
                return isLayoutBuilt;
            }
        }

        public DungeonArchitect.DungeonModel Model
        {
            get { return model; }
        }

        public Blackboard Blackboard
        {
            get { return blackboard; }
        }

        /// <summary>
        /// Builds the dungeon layout
        /// </summary>
        /// <param name="config">The builder configuration</param>
        /// <param name="model">The dungeon model that the builder will populate</param>
        public virtual void BuildDungeon(DungeonConfig config, DungeonModel model)
        {
            this.config = config;
            this.model = model;
            nrandom = new PMRandom(config.Seed);
            random = new PMRandom(config.Seed);
            GridToMeshScale = config.GridCellSize;

            isLayoutBuilt = true;
        }

		public virtual void OnDestroyed() {
			ClearSockets();
			isLayoutBuilt = false;
		}

        /// <summary>
        /// Emit markers defined by this builder
        /// </summary>
        public virtual void EmitMarkers() { 
			ClearSockets();
		}

        /// <summary>
        /// Emit markers defined by the user (implementation of DungeonMarkerEmitter)
        /// </summary>
        public void EmitCustomMarkers()
        {
            var emitters = GetComponents<DungeonMarkerEmitter>();
            foreach (var emitter in emitters)
            {
                emitter.EmitMarkers(this);
            }
        }
		
		protected void ClearSockets()
		{
			_SocketIdCounter = 0;
			PropSockets.Clear();
		}

        public void EmitMarker(string SocketType, Matrix4x4 transform, IntVector gridPosition, int cellId)
        {
            PropSocket socket = new PropSocket();
            socket.Id = ++_SocketIdCounter;
            socket.IsConsumed = false;
            socket.SocketType = SocketType;
            socket.Transform = transform;
            socket.gridPosition = gridPosition;
            socket.cellId = cellId;
            PropSockets.Add(socket);
        }

        protected void EmitMarker(string SocketType, Matrix4x4 _transform, int count, Vector3 InterOffset, IntVector gridPosition, int cellId)
        {
            var iposition = new IntVector(gridPosition.x, gridPosition.y, gridPosition.z);
            var ioffset = new IntVector(
                Mathf.RoundToInt(InterOffset.x / GridToMeshScale.x),
                Mathf.RoundToInt(InterOffset.y / GridToMeshScale.y),
                Mathf.RoundToInt(InterOffset.z / GridToMeshScale.z)
            );
            Matrix4x4 transform = Matrix.Copy(_transform);
            var position = Matrix.GetTranslation(ref transform);

            for (int i = 0; i < count; i++)
            {
                EmitMarker(SocketType, transform, iposition, cellId);
                position += InterOffset;
                iposition += ioffset;
                transform = Matrix.Copy(transform);
                Matrix.SetTranslation(ref transform, position);
            }
        }

        protected void EmitMarker(List<PropSocket> pPropSockets, string SocketType, Matrix4x4 transform, IntVector gridPosition, int cellId)
        {
            PropSocket socket = new PropSocket();
            socket.Id = ++_SocketIdCounter;
            socket.IsConsumed = false;
            socket.SocketType = SocketType;
            socket.Transform = transform;
            socket.gridPosition = gridPosition;
            socket.cellId = cellId;
            pPropSockets.Add(socket);
        }


        protected void CreatePropLookup(DungeonPropDataAsset PropAsset, PropBySocketTypeByTheme_t PropBySocketTypeByTheme)
        {
            if (PropAsset == null || PropBySocketTypeByTheme.ContainsKey(PropAsset))
            {
                // Lookup for this theme has already been built
                return;
            }

            PropBySocketType_t PropBySocketType = new PropBySocketType_t();
            PropBySocketTypeByTheme.Add(PropAsset, PropBySocketType);

            foreach (PropTypeData Prop in PropAsset.Props)
            {
                if (!PropBySocketType.ContainsKey(Prop.AttachToSocket))
                {
                    PropBySocketType.Add(Prop.AttachToSocket, new List<PropTypeData>());
                }
                PropBySocketType[Prop.AttachToSocket].Add(Prop);
            }
        }

        // Picks a theme from the list that has a definition for the defined socket
        protected DungeonPropDataAsset GetBestMatchedTheme(List<DungeonPropDataAsset> Themes, PropSocket socket, PropBySocketTypeByTheme_t PropBySocketTypeByTheme)
        {
            var ValidThemes = new List<DungeonPropDataAsset>();
            foreach (DungeonPropDataAsset Theme in Themes)
            {
                if (PropBySocketTypeByTheme.ContainsKey(Theme))
                {
                    PropBySocketType_t PropBySocketType = PropBySocketTypeByTheme[Theme];
                    if (PropBySocketType.Count > 0)
                    {
                        if (PropBySocketType.ContainsKey(socket.SocketType) && PropBySocketType[socket.SocketType].Count > 0)
                        {
                            ValidThemes.Add(Theme);
                        }
                    }
                }
            }
            if (ValidThemes.Count == 0)
            {
                return null;
            }

            int index = Mathf.FloorToInt(random.GetNextUniformFloat() * ValidThemes.Count) % ValidThemes.Count;
            return ValidThemes[index];
        }

        public virtual void ApplyTheme(List<DungeonPropDataAsset> Themes, DungeonSceneProvider SceneProvider)
        {
			var instanceCache = new InstanceCache();

            PropBySocketTypeByTheme_t PropBySocketTypeByTheme = new PropBySocketTypeByTheme_t();
            foreach (DungeonPropDataAsset Theme in Themes)
            {
                CreatePropLookup(Theme, PropBySocketTypeByTheme);
            }

            // Collect all the theme override volumes and prepare their theme lookup
            var overrideVolumes = new List<ThemeOverrideVolume>();
            Dictionary<Graph, DungeonPropDataAsset> GraphToThemeMapping = new Dictionary<Graph, DungeonPropDataAsset>();

            // Process the theme override volumes
            var themeOverrides = GameObject.FindObjectsOfType<ThemeOverrideVolume>();
            foreach (var themeOverride in themeOverrides)
            {
                overrideVolumes.Add(themeOverride);
                var graph = themeOverride.overrideTheme;
                if (graph != null && !GraphToThemeMapping.ContainsKey(graph))
                {
                    DungeonPropDataAsset theme = new DungeonPropDataAsset();
                    theme.BuildFromGraph(themeOverride.overrideTheme);
                    GraphToThemeMapping.Add(themeOverride.overrideTheme, theme);

                    CreatePropLookup(theme, PropBySocketTypeByTheme);
                }
            }

            var srandom = new PMRandom(config.Seed);
            // Fill up the prop sockets with the defined mesh data 
            for (int i = 0; i < PropSockets.Count; i++)
            {
                PropSocket socket = PropSockets[i];

                DungeonPropDataAsset ThemeToUse = GetBestMatchedTheme(Themes, socket, PropBySocketTypeByTheme); // PropAsset;

                // Check if this socket resides within a override volume
                {
                    var socketPosition = Matrix.GetTranslation(ref socket.Transform);
                    foreach (var volume in overrideVolumes)
                    {
                        if (volume.GetBounds().Contains(socketPosition))
                        {
                            var graph = volume.overrideTheme;
                            if (graph != null && GraphToThemeMapping.ContainsKey(graph))
                            {
                                ThemeToUse = GraphToThemeMapping[volume.overrideTheme];
                                break;
                            }
                        }
                    }
                }

                if (ThemeToUse == null) continue;

                PropBySocketType_t PropBySocketType = PropBySocketTypeByTheme[ThemeToUse];

                if (PropBySocketType.ContainsKey(socket.SocketType))
                {
                    List<PropTypeData> props = PropBySocketType[socket.SocketType];
                    foreach (PropTypeData prop in props)
                    {
						bool insertMesh = false;
						Matrix4x4 transform = socket.Transform * prop.Offset;

                        if (prop.UseSelectionRule && prop.SelectorRuleClassName != null)
                        {
							var selectorRule = instanceCache.GetInstance(prop.SelectorRuleClassName) as SelectorRule;
							if (selectorRule != null) {
	                            // Run the selection rule logic to determine if we need to insert this mesh in the scene
								insertMesh = selectorRule.CanSelect(socket, transform, model, random.UniformRandom);
							}
                        }
                        else
                        {
                            // Perform probability based selection logic
                            float probability = srandom.GetNextUniformFloat();
                            insertMesh = (probability < prop.Affinity);
                        }


                        if (insertMesh)
                        {
                            // Attach this prop to the socket
							// Apply transformation logic, if specified
							if (prop.UseTransformRule && prop.TransformRuleClassName != null && prop.TransformRuleClassName.Length > 0) {
								var transformer = instanceCache.GetInstance(prop.TransformRuleClassName) as TransformationRule;
								if (transformer != null) {
									Vector3 _position, _scale;
									Quaternion _rotation;
									transformer.GetTransform(socket, model, transform, random.UniformRandom, out _position, out _rotation, out _scale);
									var offset = Matrix4x4.TRS(_position, _rotation, _scale);
									transform = transform * offset;
								}
							}

							if (prop is GameObjectPropTypeData) {
								var gameObjectProp = prop as GameObjectPropTypeData;
								SceneProvider.AddGameObject(gameObjectProp, transform);
							}
							else if (prop is SpritePropTypeData) {
								var spriteProp = prop as SpritePropTypeData;
								SceneProvider.AddSprite(spriteProp, transform);
							}
                            // TODO: Handle light creation


                            // Add child sockets if any
                            foreach (PropChildSocketData ChildSocket in prop.ChildSockets)
                            {
                                Matrix4x4 childTransform = transform * ChildSocket.Offset;
                                EmitMarker(ChildSocket.SocketType, childTransform, socket.gridPosition, socket.cellId);
                            }

                            if (prop.ConsumeOnAttach)
                            {
                                // Attach no more on this socket
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}