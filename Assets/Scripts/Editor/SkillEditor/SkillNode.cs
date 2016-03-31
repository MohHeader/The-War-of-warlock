//
//  SkillNode.cs
//
//  Author:
//       ${wuxingogo} <52111314ly@gmail.com>
//
//  Copyright (c) 2016 ly-user
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System.Collections;
using node;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


namespace skill
{

	public abstract class SkillNode : Node
	{
		public SkillNode()
		{
			
		}

		public virtual void Init(ISkillEditorMarker para, System.Action<SkillStruct> OnInit = null)
		{
			OnDragSkillStructInSelf = OnInit;
			isInit = false;
		}

		System.Action<SkillStruct> OnDragSkillStructInSelf = null;

		public void SetSkillStruct(SkillStruct skillStruct)
		{
			if( OnDragSkillStructInSelf != null )
				OnDragSkillStructInSelf( skillStruct );
		}

		protected virtual bool isInit {
			get;
			set;
		}

		public List<SkillNode> Parents = new List<SkillNode>();
		public List<SkillNode> NextNodes = new List<SkillNode>();

		public bool ContainChild(SkillNode child)
		{
			return NextNodes.FindIndex( t => t.identifier.Equals( child.identifier ) ) != -1;
		}

		public bool ContainParent(SkillNode child)
		{
			return Parents.FindIndex( t => t.identifier.Equals( child.identifier ) ) != -1;
		}

		public virtual string Title {
			get {
				return Asset().name;
			}
		}

		public virtual Texture ThumbnailTexture {
			get {
				Texture thumb = AssetPreview.GetAssetPreview( Asset() );
				if( thumb == null ) {
					thumb = AssetPreview.GetMiniTypeThumbnail( typeof( GameObject ) );
				}
				return thumb;
			}
		}

		public virtual void MarkDirty()
		{
			var o = Asset();
			if( o != null && o is ScriptableObject ) {
				EditorUtility.SetDirty( o as ScriptableObject );
			} else {
				foreach( var item in Parents ) {
					item.MarkDirty();
				}
			}
		}

		public TComponent GetParent<TComponent>() where TComponent :SkillNode
		{
			foreach( var item in Parents ) {
				if( item is TComponent ) {
					return item as TComponent;
				}
			}
			return null;
		}

		public TComponent GetChild<TComponent>() where TComponent :SkillNode
		{
			foreach( var item in NextNodes ) {
				if( item is TComponent ) {
					return item as TComponent;
				}
			}
			return null;
		}

		public List<TComponent> GetParents<TComponent>() where TComponent :SkillNode
		{
			var list = new List<TComponent>();
			foreach( var item in Parents ) {
				if( item is TComponent ) {
					list.Add( item as TComponent );
				}
			}
			return list;
		}

		public List<TComponent> GetChildren<TComponent>() where TComponent :SkillNode
		{
			var list = new List<TComponent>();
			foreach( var item in NextNodes ) {
				if( item is TComponent ) {
					list.Add( item as TComponent );
				}
			}
			return list;
		}

		public virtual IEnumerable<SkillNode> GetChildrenNode()
		{
			return new SkillNode[]{ this };
		}

		public virtual IEnumerable<SkillNode> RemoveSameNode(IEnumerable<SkillNode> lhs, IEnumerable<SkillNode> rhs)
		{
			var list = lhs.ToList();
			list.ForEach( child => {
				if( !list.Contains( child ) ) {
					list.Add( child );
				}
			} );
			return list;
		}

		public virtual object identifier {
			get;
			protected set;
		}

		public virtual Object Handler {
			set {
				identifier = value;
			}
		}

		public virtual StructBase marker {
			get {
				return null;
			}
		}

		protected override void OnChangeBounds()
		{
			SkillWindow.UpdatePosition( this );
		}

		public virtual void AddClickMenu(GenericMenu genericMenu)
		{
			genericMenu.AddItem( new GUIContent( "Delete" ), false, RemoveFromParent );
		}

		public virtual void RemoveFromParent()
		{
			foreach( var item in Parents ) {
				item.NextNodes.Remove( this );
			}

			var array = NextNodes.ToArray();
			for( int pos = 0; pos < array.Length; pos++ ) {
				//  TODO loop in NextNodes.Count
				array[pos].RemoveFromParent();
			}
			SkillWindow.GetInstance().Nodes.Remove( this );
			SkillWindow.skillTree.RemoveRect( marker );
		}

		//		public override void OnSelected()
		//		{
		//			foreach( var item in Parents ) {
		//				Debug.Log(item.Asset().ToString());
		//			}
		//			foreach( var item in NextNodes ) {
		//				Debug.Log(item.Asset().ToString());
		//			}
		//		}

		public override void Draw()
		{
			base.Draw();
			GUI.Box( DrawBounds, ThumbnailTexture, XStyles.GetInstance().window );

			GUI.Label( DrawBounds, Title, XStyles.GetInstance().label );
			for( int pos = 0; pos < NextNodes.Count; pos++ ) {
				//  TODO loop in NextNodes.Count
				SkillWindow.DrawBesizeFromRect( new Rect( DrawBounds.x, DrawBounds.y + ( DrawBounds.height / NextNodes.Count * pos ), DrawBounds.width, DrawBounds.height / NextNodes.Count )
				, NextNodes[pos].DrawBounds );
			
			}
			if( Selected ) {
				EditorGUI.DrawRect( DrawBounds, new Color( 0, 0, 0.5f, 0.3f ) );
			}
				
		}

	}
}

