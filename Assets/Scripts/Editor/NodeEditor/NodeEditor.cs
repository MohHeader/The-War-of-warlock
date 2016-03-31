// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.1433
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using skill;


namespace  node
{

	public abstract class NodeEditor<TNode> : XBaseWindow where TNode : Node
	{
		internal List<TNode> Nodes = null;
		public XWindowCamera graphCamera = null;
		protected SkillWindowMenu contextMenu = null;
		GraphSelectionBox selectionBox = null;
		KeyboardState keyboardState;

		Vector2 lastMousePosition = new Vector2();
		static bool isInit = false;

		public void OnEnable()
		{
			Nodes = new List<TNode>();
			graphCamera = new XWindowCamera();
			selectionBox = new GraphSelectionBox();
			selectionBox.SelectionPerformed += HandleBoxSelection;

			keyboardState = new KeyboardState();

			contextMenu = new SkillWindowMenu();
			contextMenu.RequestContextMenuCreation += OnRequestContextMenuCreation;
			contextMenu.MenuItemClicked += OnMenuItemClicked;
		}

		public void OnDisable()
		{
			Nodes = null;
			if( contextMenu != null ) {
				contextMenu.RequestContextMenuCreation -= OnRequestContextMenuCreation;
				contextMenu.MenuItemClicked -= OnMenuItemClicked;
				contextMenu = null;
			}
			keyboardState = null;
			if( selectionBox != null ) {
				selectionBox.SelectionPerformed -= HandleBoxSelection;
				selectionBox = null;
			}
			graphCamera = null;
		}

		void OnMenuItemClicked(SkillWindowMenu.SkillMenuAction action, SkillWindowMenu.SkillContextMenuEvent e)
		{
			var mouseScreen = lastMousePosition;
			TNode node = null;
			/*
			if (action == SkillWindowMenu.SkillMenuAction.AddGameObjectNode)
            {
                node = CreateNode<GameObjectNode>(mouseScreen);
                SelectNode(node);
            }
			else if (action == SkillWindowMenu.SkillMenuAction.AddSpriteNode)
            {
                node = CreateNode<SpriteNode>(mouseScreen);
                SelectNode(node);
            }
			else if (action == SkillWindowMenu.SkillMenuAction.AddMarkerNode)
            {
                node = CreateNode<MarkerNode>(mouseScreen);
                SelectNode(node);
            }
			else if (action == SkillWindowMenu.SkillMenuAction.AddMarkerEmitterNode)
            {
                if (e.userdata != null)
                {
                    var markerName = e.userdata as String;
                    node = CreateMarkerEmitterNode(mouseScreen, markerName);
                    if (node != null)
                    {
                        SelectNode(node);
                    }
                }
            }


            if (node != null)
            {
                // Check if the menu was created by dragging out a link
                if (e.sourcePin != null)
                {
                    GraphPin targetPin =
                            e.sourcePin.PinType == GraphPinType.Input ?
                            node.OutputPins[0] :
                            node.InputPins[0];

                    // Align the target pin with the mouse position where the link was dragged and released
                    node.Position = e.mouseWorldPosition - targetPin.Position;

                    GraphPin inputPin, outputPin;
                    if (e.sourcePin.PinType == GraphPinType.Input)
                    {
                        inputPin = e.sourcePin;
                        outputPin = targetPin;
                    }
                    else
                    {
                        inputPin = targetPin;
                        outputPin = e.sourcePin;
                    }
                    CreateLinkBetweenPins(outputPin, inputPin);
                }
            }
            */
		}

		protected virtual void OnRequestContextMenuCreation(Event e)
		{
			
		}

		public virtual void OnNoneSelectedNode()
		{
			
		}

		#region DrawBesize

		public static void DrawBesizeFromRect(Rect lhs, Rect rhs)
		{
			var dir = (lhs.position - rhs.position).normalized;
			Vector3 startPos = new Vector3( lhs.x + lhs.width, lhs.y + lhs.height / 2, 0 );
			Vector3 endPos = new Vector3( rhs.x, rhs.y + rhs.height / 2, 0 );

			var distance = ( startPos - endPos ).magnitude * 0.5f;
			Vector3 startTan = startPos + Vector3.right * distance;
			Vector3 endTan = endPos + Vector3.left * distance;
			Color shadowCol = new Color( 0, 0, 0, .06f );

			for( int i = 0; i < 3; i++ ) {
				Handles.DrawBezier( startPos, endPos, startTan, endTan, shadowCol, null, ( i + 1 ) * 5 );
				
			}
			Handles.DrawBezier( startPos, endPos, startTan, endTan, Color.gray, null, 1 );
		}

		#endregion

		void HandleBoxSelection(Rect boundsScreenSpace)
		{
			bool multiSelect = keyboardState.ShiftPressed;
			bool selectedStateChanged = false;
			foreach( var node in Nodes ) {
				// node bounds in world space
				var nodeBounds = new Rect( node.DrawBounds );

				var selected = nodeBounds.Overlaps( boundsScreenSpace );
				if( multiSelect ) {
					if( selected ) {
						selectedStateChanged |= SetSelectedState( node, selected );
					}
				} else {
					selectedStateChanged |= SetSelectedState( node, selected );
				}
            

				if( selectedStateChanged ) {
					OnNodeSelectionChanged();
				}
			}
		}

		bool SetSelectedState(TNode node, bool selected)
		{
			bool stateChanged = ( node.Selected != selected );
			node.Selected = selected;
			return stateChanged;
		}

		void HandleSelect(Event e)
		{
			// Update the node selected flag
			var mousePositionWorld = TransportWorldPos( e );
			var buttonId = 0;
			if( e.type == EventType.MouseDown && e.button == buttonId ) {
				bool multiSelect = keyboardState.ShiftPressed;
				bool toggleSelect = keyboardState.ControlPressed;
				// sort the nodes front to back
				TNode[] sortedNodes = Nodes.ToArray();
//                System.Array.Sort(sortedNodes, new NodeReversedZIndexComparer());

				TNode mouseOverNode = null;

				mouseOverNode = SelectedNode( mousePositionWorld );

				foreach( var node in sortedNodes ) {
					var mouseOver = ( node.Equals( mouseOverNode ) );

					if( mouseOverNode != null && mouseOverNode.Selected && !toggleSelect ) {
						multiSelect = true;	// select multi-select so that we can drag multiple objects
					}
					if( multiSelect || toggleSelect ) {
						if( mouseOver && multiSelect ) {
							node.Selected = true;
							node.OnSelected();
						} else if( mouseOver && toggleSelect ) {
							node.Selected = !node.Selected;
						}
					} else {
						node.Selected = mouseOver;
					}

					if( node.Selected ) {
						BringToFront( node );
					}
				}

				if( mouseOverNode == null ) {
					// No nodes were selected 
					Selection.activeObject = null;
				}

				OnNodeSelectionChanged();
			}
		}

		protected virtual void BringToFront(TNode node)
		{
			/// to do list
		}

		bool draggingNodes = false;

		void HandleDrag(Event e)
		{
			int dragButton = 0;
			if( draggingNodes ) {
				if( e.type == EventType.MouseUp && e.button == dragButton ) {
					draggingNodes = false;
				} else if( e.type == EventType.MouseDrag && e.button == dragButton ) {
					// Drag all the selected nodes
					foreach( var node in Nodes ) {
						if( node.Selected ) {
//                            Undo.RecordObject(node, "Move Node");
							node.DragNode( e.delta );
						}
					}
				}
			} else {
				// Check if we have started to drag
				if( e.type == EventType.MouseDown && e.button == dragButton ) {
					// Find the node that was clicked below the mouse
					var mousePositionWorld = TransportWorldPos( e );

					TNode mouseOverNode = null;

					mouseOverNode = SelectedNode(mousePositionWorld);

					if( mouseOverNode != null && mouseOverNode.Selected ) {
						// Make sure we are not over a pin
//                        var pins = new List<GraphPin>();
//                        pins.AddRange(mouseOverNode.InputPins);
//                        pins.AddRange(mouseOverNode.OutputPins);
//                        bool isOverPin = false;
//                        GraphPin overlappingPin = null;
//                        foreach (var pin in pins)
//                        {
//                            if (pin.ContainsPoint(mousePositionWorld))
//                            {
//                                isOverPin = true;
//                                overlappingPin = pin;
//                                break;
//                            }
//                        }
//                        if (!isOverPin)
//                        {
						draggingNodes = true;
//                        }
//                        else
//                        {
//                            HandleDragPin(overlappingPin);
//                        }
					}
				}
			}
		}

		protected TNode SelectedNode(Vector2 worldPosition)
		{
			foreach( var item in Nodes ) {
				if(item.Bounds.Contains( worldPosition ))
				{
					return item;
				}
			}
			return null;
		}

		protected Vector2 TransportWorldPos(Event e)
		{
			return graphCamera.ScreenToWorld(e.mousePosition);
		}

		public override void OnXGUI()
		{
			Event e = Event.current;

			keyboardState.HandleInput( e );
			HandleSelect( e );
			HandleDrag( e );
			HandleDragPerform( e );
			graphCamera.HandleInput( e );


			for( int pos = 0; pos < Nodes.Count; pos++ ) {
				//  TODO loop in nodeCollection.Count
				Nodes[pos].DrawPosition = graphCamera.WorldToScreen( Nodes[pos].Position );
				Nodes[pos].Draw();
				
			}

			contextMenu.HandleInput( e );

			bool inputProcessed = false;
			foreach( var node in Nodes ) {
				if( node == null )
					continue;
				inputProcessed = NodeInputHandler.HandleNodeInput( node, e, graphCamera );
				if( inputProcessed ) {
					break;
				}
			}

			if( !inputProcessed ) {
				selectionBox.HandleInput( e );
			}

			selectionBox.Draw();

			if( e != null )
				Repaint();
		}

		public void HandleDragPerform(Event e)
		{
			DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
			if( e.type == EventType.DragPerform ) {
				DragAndDrop.AcceptDrag();

				foreach( var draggedObject in DragAndDrop.objectReferences ) {
					if( draggedObject is GameObject ) {
						DragPerformGameObject( draggedObject as GameObject, e);
					} else if( draggedObject is SkillStruct ) {
						DragPerformSkillStruct(draggedObject as SkillStruct, e);
					}
				}
			}
		}

		public virtual void DragPerformGameObject(GameObject go, Event e)
		{
			
		}
		public virtual void DragPerformSkillStruct(SkillStruct dataSkillStruct, Event e)
		{
			
		}

		public void OnNodeSelectionChanged()
		{
			// Fetch all selected nodes
			var selectedNodes = from node in Nodes
			                    where node.Selected
			                    select node.Asset();
			int count = selectedNodes.Count( n => true );

			if( count > 0 ) {
				Selection.objects = selectedNodes.ToArray();
			} else {
				OnNoneSelectedNode();
			}

		}

	}

	#region selection Box
	/// <summary>
	/// Manages the selection box for selecting multiple objects in the graph editor
	/// </summary>
	class GraphSelectionBox
	{
		public delegate void OnSelectionPerformed(Rect boundsScreenSpace);

		public event OnSelectionPerformed SelectionPerformed;

		// The bounds of the selection box in screen space
		Rect bounds = new Rect();

		public Rect Bounds {
			get {
				return bounds;
			}
			set {
				bounds = value;
			}
		}

		Vector2 dragStart = new Vector2();
		int dragButton = 0;
		bool dragging = false;

		public bool Dragging {
			get {
				return dragging;
			}
		}

		/// <summary>
		/// Handles user input (mouse)
		/// </summary>
		/// <param name="e"></param>
		public void HandleInput(Event e)
		{

			switch( e.type ) {
				case EventType.MouseDown:
					ProcessMouseDown( e );
				break;

				case EventType.MouseDrag:
					ProcessMouseDrag( e );
				break;

				case EventType.MouseUp:
					ProcessMouseUp( e );
				break;
			}
			// Handled captured mouse up event
			{
				var controlId = GUIUtility.GetControlID( FocusType.Passive );
				if( GUIUtility.hotControl == controlId && Event.current.rawType == EventType.MouseUp ) {
					ProcessMouseUp( e );
				}
			}
		}

		void ProcessMouseDrag(Event e)
		{
			if( dragging && e.button == dragButton ) {
				var dragEnd = e.mousePosition;
				UpdateBounds( dragStart, dragEnd );

				if( IsSelectionValid() && SelectionPerformed != null ) {
					SelectionPerformed( bounds );
				}
			}
		}

		void ProcessMouseDown(Event e)
		{
			if( e.button == dragButton ) {
				dragStart = e.mousePosition;
				UpdateBounds( dragStart, dragStart );
				dragging = true;
				GUIUtility.hotControl = GUIUtility.GetControlID( FocusType.Passive );
			}
		}

		void ProcessMouseUp(Event e)
		{
			if( e.button == dragButton && dragging ) {
				dragging = false;
				if( IsSelectionValid() && SelectionPerformed != null ) {
					SelectionPerformed( bounds );
				}
				GUIUtility.hotControl = 0;
			}
		}

		public bool IsSelectionValid()
		{
			return bounds.width > 0 && bounds.height > 0;
		}

		public void Draw()
		{
			if( !dragging || !IsSelectionValid() )
				return;

			GUI.backgroundColor = new Color( 1, 0.6f, 0, 0.6f );
			GUI.Box( bounds, "" );
		}

		void UpdateBounds(Vector2 start, Vector2 end)
		{
			var x0 = Mathf.Min( start.x, end.x );
			var x1 = Mathf.Max( start.x, end.x );
			var y0 = Mathf.Min( start.y, end.y );
			var y1 = Mathf.Max( start.y, end.y );
			bounds.Set( x0, y0, x1 - x0, y1 - y0 );
		}

	}
	#endregion

	#region Key board Listener
	/// <summary>
	/// Caches the keyboard state 
	/// </summary>
	class KeyboardState
	{
		Dictionary<KeyCode, bool> state = new Dictionary<KeyCode, bool>();
		bool shift;
		bool control;
		bool alt;

		public void SetState(KeyCode keyCode, bool pressed)
		{
			if( !state.ContainsKey( keyCode ) ) {
				state.Add( keyCode, false );
			}
			state[keyCode] = pressed;
		}

		public void HandleInput(Event e)
		{

			if( e.type == EventType.KeyDown ) {
				SetState( e.keyCode, true );
			} else if( e.type == EventType.KeyUp ) {
				SetState( e.keyCode, false );
			}

			alt = e.alt;
			shift = e.shift;
			control = e.control || e.command;
		}

		public bool GetSate(KeyCode keyCode)
		{
			if( !state.ContainsKey( keyCode ) ) {
				return false;
			}
			return state[keyCode];
		}

		public bool ControlPressed {
			get {
				return control;
			}
		}

		public bool ShiftPressed {
			get {
				return shift;
			}
		}

		public bool AltPressed {
			get {
				return alt;
			}
		}
	}

	#endregion

}
