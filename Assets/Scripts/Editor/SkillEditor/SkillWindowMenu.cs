//
//  SkillWindowMenu.cs
//
//  Author:
//       ${wuxingogo} <52111314ly@gmail.com>
//
//  Copyright (c) 2016 ly-user
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;


namespace skill
{

	public class SkillWindowMenu
	{

		/// <summary>
		/// The type of menu action to perform
		/// </summary>
		public enum SkillMenuAction
		{
			AddGameObjectNode,
			AddStructNode,

		}

		/// <summary>
		/// The graph context menu event data
		/// </summary>
		public class SkillContextMenuEvent
		{
			public SkillNode sourcePin;
			public Vector2 mouseWorldPosition;
			public object userdata;
		}

		string[] GetMarkers(SkillWindow graph)
		{
			var markers = new List<string>();
			if( graph != null ) {
				foreach( var node in graph.Nodes ) {
//                    if (node is MarkerNode)
//                    {
//                        markers.Add(node.Caption);
//                    }
				}
			}
			var markerArray = markers.ToArray();
			System.Array.Sort( markerArray );
			return markerArray;
		}

		bool dragged;
		int dragButtonId = 1;

		bool showItemMeshNode;
		bool showItemMarkerNode;
		bool showItemMarkerEmitterNode;

		SkillNode sourcePin;
		Vector2 mouseWorldPosition;

		public delegate void OnRequestContextMenuCreation(Event e);
		public event OnRequestContextMenuCreation RequestContextMenuCreation;

		public delegate void OnMenuItemClicked(SkillMenuAction action, SkillContextMenuEvent e);
		public event OnMenuItemClicked MenuItemClicked;

		private SkillTree graph = null;
		private SkillWindow window = null;
		/// <summary>
		/// Handles mouse input
		/// </summary>
		/// <param name="e">Input event data</param>
		public void HandleInput(Event e)
		{
			switch( e.type ) {
				case EventType.MouseDown:
					if( e.button == dragButtonId ) {
						dragged = false;
					}
				break;

				case EventType.MouseDrag:
					if( e.button == dragButtonId ) {
						dragged = true;
					}
				break;

				case EventType.MouseUp:
					if( e.button == dragButtonId && !dragged ) {
						if( RequestContextMenuCreation != null ) {
							RequestContextMenuCreation( e );
						}
					}
				break;
			}

		}
		public void ShowNodeMenu(SkillNode handleNode, SkillWindow window)
		{
			sourcePin = handleNode;
			this.window = window;
			var menu = new GenericMenu();
			handleNode.AddClickMenu(menu);
			menu.ShowAsContext();
		}
		public void ShowAddCastMenu(SkillWindow graph, Action<SkillNode> OnAddSkillNode)
		{
			this.window = graph;
			this.graph = SkillWindow.skillTree;

			var menu = new GenericMenu();
			menu.AddItem( new GUIContent( "Add Cast Node" ), false, () => {
				var cs = new CastStruct();
				this.graph.castStruct.Add( cs );
				var skillNode = SkillWindow.CreateNode<SkillCastNode>( cs, null );
				OnAddSkillNode( skillNode );
			} );
			menu.ShowAsContext();

		}

	}
}

