////
////  SkillTargetNode.cs
////
////  Author:
////       ${wuxingogo} <52111314ly@gmail.com>
////
////  Copyright (c) 2016 ly-user
////
////  You should have received a copy of the GNU Lesser General Public License
////  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//using System;
//using System.Collections.Generic;
//using System.Collections;
//using UnityEditor;
//using UnityEngine;
//
//
//namespace skill
//{
//
//	public class SkillTargetNode : SkillNode
//	{
//		SkillStructTarget assets = null;
//
//		public SkillStructTarget SkillHandler()
//		{
//			return assets;
//		}
//
//		protected override string Title {
//			get {
//				return title;
//			}
//		}
//
//		private string title = "";
//
//		public SkillTargetNode(SkillStructTarget targetStruct)
//		{
//			assets = targetStruct;
//			identifier = targetStruct;
//			title = "Target Node";
//		}
//
//		private Texture thumb = null;
//
//		public override UnityEngine.Texture ThumbnailTexture {
//			get {
//				if( thumb == null ) {
//					thumb = EditorGUIUtility.IconContent( "ScriptableObject Icon" ).image;
//				}
//				return thumb;
//			}
//		}
//
//		public override StructBase marker {
//			get {
//				return assets.marker;
//			}
//		}
//
//		public override UnityEngine.Object Asset()
//		{
//			return assets;
//		}
//
//		public override void AddClickMenu(GenericMenu genericMenu)
//		{
//			if(assets.flyStruct.flyGo == null)
//				genericMenu.AddItem(new GUIContent("AddFly GameObject"), false, AddNode, "FlyGameObject");
//			if(assets.flyStruct.collider.skillData == null)
//				genericMenu.AddItem(new GUIContent("AddFly Collider"), false, AddNode, "FlyCollider");
//			if(assets.hitStruct.hitGo == null)
//				genericMenu.AddItem(new GUIContent("AddHit GameObject"), false, AddNode, "HitGameObject");
//			if(assets.hitStruct.collider.skillData == null)
//				genericMenu.AddItem(new GUIContent("AddFly Collider"), false, AddNode, "HitCollider");
//			base.AddClickMenu(genericMenu);
//		}
//
//		public virtual void AddNode(object userData){
//			string para = (string)userData;
//			switch( para ) {
//				case "FlyGameObject":
//					SkillWindow.CreateNode( assets.flyStruct, this );
//				break;
//				case "FlyCollider":
//				break;
//				case "HitGameObject":
//					SkillWindow.CreateNode( assets.hitStruct, this );
//				break;
//				case "HitCollider":
//				break;
//				default:
//				break;
//			}
//		}
//
//		public override IEnumerable<SkillNode> GetChildrenNode()
//		{
//			if( isInit ) {
//				return new SkillNode[0];
//			}
//			isInit = true;
//			var list = new List<SkillNode>();
//			list.AddRange( base.GetChildrenNode() );
//			if( !assets.flyStruct.isNull ) {
//				var flyNode = SkillWindow.CreateNode( assets.flyStruct, this );
//				list.AddRange( flyNode.GetChildrenNode() );
//
//				if( assets.flyStruct.collider.skillData != null ) {
//					var flyCollisionNode = SkillWindow.CreateNode( assets.flyStruct.collider.skillData, flyNode );
//					list.AddRange( flyCollisionNode.GetChildrenNode() );
//				}
//			}
//
//			if( !assets.hitStruct.isNull ) {
//				var hitNode = SkillWindow.CreateNode( assets.hitStruct, this );
//				list.AddRange( hitNode.GetChildrenNode() );
//
//				if( assets.hitStruct.collider.skillData != null ) {
//					var hitCollisionNode = SkillWindow.CreateNode( assets.hitStruct.collider.skillData, hitNode );
//					list.AddRange( hitNode.GetChildrenNode() );
//				}
//			}
//
//			return list;
//		}
//
//	}
//}
//
