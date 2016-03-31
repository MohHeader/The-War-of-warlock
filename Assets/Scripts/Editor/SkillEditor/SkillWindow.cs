using node;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using System.Collections;


namespace skill
{

	public class SkillWindow : NodeEditor<SkillNode>
	{

		public static void ShowWindow(SkillTree graph)
		{
			var window = InitWindow<SkillWindow>();
			SkillWindow.skillTree = graph;
			window.OnEnable();
		}

		public static SkillTree skillTree = null;
		public static int rectIndex = -1;
		private static SkillWindow instance = null;

		public static SkillWindow GetInstance(){
			return instance;
		}

		public void OnEnable()
		{
			base.OnEnable();
			instance = this;

			if( skillTree != null ) {
				var castCollection = skillTree.castStruct;
				rectIndex = -1;
				Nodes.Clear();
				for( int pos = 0; pos < castCollection.Count; pos++ ) {
					//  TODO loop in 
					SkillCastNode castNode = CreateNode<SkillCastNode>( castCollection[pos] ) as SkillCastNode;
//					var children = castNode.GetChildrenNode();
//					Nodes.Add( castNode );
				}

//				RemoveSameNode( Nodes );
			}
		}

		void OnDisable()
		{
			base.OnDisable();
		}

		public void RemoveSameNode(List<SkillNode> list)
		{
			for( int pos = 0; pos < list.Count; pos++ ) {
				//  TODO loop in Nodes.Count
				var child = list[pos];
				for( int i = pos + 1; i < list.Count; i++ ) {
					//  TODO loop in Nodes.Count
					var rhs = list[i];
					if( child.Equals( rhs ) && i != pos ) {
						Debug.Log( i + "," + pos );
						rhs.RemoveFromParent();
						list.RemoveAt( pos );
					}
				}
			}
		}

		protected override void OnRequestContextMenuCreation(Event e)
		{
			// Make sure we are not over an existing node
			var mouseWorld = graphCamera.ScreenToWorld( e.mousePosition );
			foreach( var node in Nodes ) {
				if( node.Bounds.Contains( mouseWorld ) ) {
					// the user has clicked on a node. Handle this with a separate logic
					contextMenu.ShowNodeMenu( node, this );
					return;
				}
			}

			contextMenu.ShowAddCastMenu( this, ( skillNode) => {
				var screenCoord = e.mousePosition;
				var screenPosition = new Vector2(screenCoord.x, screenCoord.y - skillNode.Bounds.size.y);
				skillNode.bounds.position = graphCamera.ScreenToWorld(screenPosition);
				Repaint();
			} );
			
		}

		public override void OnNoneSelectedNode()
		{
			Selection.objects = new Object[]{ skillTree };
		}

		public static void UpdatePosition(SkillNode skillNode)
		{
			skillTree.UpdateRect( skillNode.marker, skillNode.Bounds );
		}

		public static TNode CreateNode<TNode>(ISkillEditorMarker nodeValue, SkillNode lastNode = null, System.Action<SkillStruct> OnInit = null) 
			where TNode : SkillNode, new()
		{
			TNode newNode = null;
			StructBase structMarker = nodeValue.GetMarker();
			newNode = FindExistNode( nodeValue ) as TNode;
			if( newNode != null ) {
				InitNode( newNode, lastNode );

				return newNode;
			}

			newNode = new TNode();
			newNode.Init( nodeValue, OnInit );

			instance.Nodes.Add( newNode );
			InitNode( newNode, lastNode );
			newNode.GetChildrenNode();
			newNode.bounds = skillTree.FindRect( structMarker );

			return newNode;
		}

		public void PerformDelete(SkillNode node)
		{
//			if( node is SkillCastNode ) {
//				var scn = node as SkillCastNode;
//				skillTree.castStruct.Remove( scn.SkillHandler() );
//				skillTree.RemoveRect( scn.SkillHandler() );
//				node.RemoveFromParent();
//				EditorUtility.SetDirty( skillTree );
//			}else if(node is SkillTargetNode || node is SkillNoTargetNode)
//			{
//				
//				foreach( var item in node.NextNodes ) {
//					Nodes.Remove( item );
//				}
//				node.RemoveFromParent();
//			}
//			Nodes.Remove( node );
			node.RemoveFromParent();
		}


		public static SkillNode FindExistNode(object asset)
		{
			if( asset is SkillEffectBase ) {
				var effectStruct = asset as SkillEffectBase;
				int index = instance.Nodes.FindIndex( t => t.identifier == effectStruct.effectGameObject );	

				if( index != -1 ) {
					Debug.Log( index );
					return instance.Nodes[index];
				}
				return null;
			}
//			if( asset is SkillHitStruct ) {
//				var hitStruct = asset as SkillHitStruct;
//				int index = instance.Nodes.FindIndex( t => t.identifier == hitStruct.hitGo );	
//
//				if( index != -1 ) {
//					Debug.Log( index );
//					return instance.Nodes[index];
//				}
//				return null;
//			} else if( asset is SkillFlyStruct ) {
//				var flyStruct = asset as SkillFlyStruct;
//				int index = instance.Nodes.FindIndex( t => t.identifier == flyStruct.flyGo );
//
//				if( index != -1 ) {
//					Debug.Log( index );
//					return instance.Nodes[index];
//				}
//				return null;
//			}
			return instance.Nodes.Find( t => t.identifier.Equals( asset ) );	
		}

		public static void InitNode(SkillNode newNode, SkillNode lastNode)
		{
			if( lastNode != null ) {
				lastNode.NextNodes.Add( newNode );
				newNode.Parents.Add( lastNode );

			}
		}

		public override void DragPerformGameObject(GameObject go, Event e)
		{
			var skillNode = SelectedNode(TransportWorldPos(e));
			if(skillNode != null)
			{
				skillNode.Handler = go;
				skillNode.MarkDirty();
			}

		}
		public override void DragPerformSkillStruct(SkillStruct dataSkillStruct, Event e)
		{
			var skillNode = SelectedNode(TransportWorldPos(e));
			if(skillNode is SkillTemporaryNode || skillNode is SkillStructNode)
			{
				foreach( var item in skillNode.Parents ) {
					var newNode = SkillWindow.CreateNode<SkillStructNode>(dataSkillStruct, item);
					var screenCoord = e.mousePosition;
					var screenPosition = new Vector2(screenCoord.x, screenCoord.y - skillNode.Bounds.size.y);
					newNode.bounds.position = graphCamera.ScreenToWorld(screenPosition);
				}
				skillNode.SetSkillStruct(dataSkillStruct);
				skillNode.RemoveFromParent();
			}
			else if(skillNode is SkillStructBaseNode)
			{
				skillNode.Handler = dataSkillStruct;
			}
		}



	}
}