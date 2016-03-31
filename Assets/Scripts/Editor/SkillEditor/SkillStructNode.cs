//
//  SkillTargetNode.cs
//
//  Author:
//       ${wuxingogo} <52111314ly@gmail.com>
//
//  Copyright (c) 2016 ly-user
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using UnityEngine;


namespace skill
{

	public class SkillStructNode : SkillNode
	{
		SkillStruct assets = null;

		public SkillStruct SkillHandler()
		{
			return assets;
		}

		public override string Title {
			get {
				return title;
			}
		}

		private string title = "";

		public override void Init(ISkillEditorMarker targetStruct, System.Action<SkillStruct> OnInit = null)
		{
			base.Init( targetStruct );
			if( targetStruct != null ) {
				assets = (SkillStruct)targetStruct;
				identifier = assets;
				title = "Struct " + assets.type;
			} else {
				title = "None";
			}
		}

		private Texture thumb = null;

		public override UnityEngine.Texture ThumbnailTexture {
			get {
				if( thumb == null ) {
					thumb = EditorGUIUtility.IconContent( "ScriptableObject Icon" ).image;
				}
				return thumb;
			}
		}

		public override StructBase marker {
			get {
				return assets.marker;
			}
		}

		public override UnityEngine.Object Asset()
		{
			return assets;
		}

		public override void AddClickMenu(GenericMenu genericMenu)
		{
//			if(assets.flyStruct.flyGo == null)
//				genericMenu.AddItem(new GUIContent("AddFly GameObject"), false, AddNode, "FlyGameObject");
//			if(assets.flyStruct.collider.skillData == null)
//				genericMenu.AddItem(new GUIContent("AddFly Collider"), false, AddNode, "FlyCollider");
//			if(assets.hitStruct.hitGo == null)
//				genericMenu.AddItem(new GUIContent("AddHit GameObject"), false, AddNode, "HitGameObject");
//			if(assets.hitStruct.collider.skillData == null)
//				genericMenu.AddItem(new GUIContent("AddFly Collider"), false, AddNode, "HitCollider");
			var sst = assets as SkillStructTarget;
			var ssnt = assets as SkillStructNoTarget;
			var flyAndHit = GetChildren<SkillStructBaseNode>();
			SkillStructBaseNode flyNode = null;
			SkillStructBaseNode hitNode = null;
			foreach( var item in flyAndHit ) {
				if( item.Title == "FlyStruct" ) {
					flyNode = item;
				} else if( item.Title == "HitStruct" ) {
					hitNode = item;
				}
			}
			if( flyNode == null )
				genericMenu.AddItem( new GUIContent( "Add Fly Node" ), false, () => {
					if(sst != null)
						CreateFlyStruct(sst.flyStruct, true);
					else if(ssnt != null)
						CreateFlyStruct(ssnt.flyStruct, true);
				} );
			if( hitNode == null )
				genericMenu.AddItem( new GUIContent( "Add Hit Node" ), false, () => {
					if(sst != null)
						CreateHittruct(sst.hitStruct, true);
					else if(ssnt != null)
						CreateHittruct(ssnt.hitStruct, true);
				} );
			base.AddClickMenu( genericMenu );
		}

		public override IEnumerable<SkillNode> GetChildrenNode()
		{
			if( isInit ) {
				return new SkillNode[0];
			}
			isInit = true;
			var list = new List<SkillNode>();
			list.AddRange( base.GetChildrenNode() );

			switch( assets.type ) {
				case SkillStruct.SkillType.Target:
					{
						var sst = assets as SkillStructTarget;
						CreateFlyStruct( sst.flyStruct );
						CreateHittruct( sst.hitStruct );
					}
				break;
				case SkillStruct.SkillType.NoTarget:
					{
						var sst = assets as SkillStructNoTarget;
						CreateFlyStruct( sst.flyStruct );
						CreateHittruct( sst.hitStruct );
					}
				break;
				case SkillStruct.SkillType.Move:
					{
						var sst = assets as SkillStructMove;
					}
				break;
			}


			return list;
		}

		private void CreateFlyStruct(SkillFlyStruct flyStruct, bool canNull = false)
		{
			if( !flyStruct.isNull || canNull) {
				var flyNode = SkillWindow.CreateNode<SkillStructBaseNode>( flyStruct, this );

				if( flyStruct.collider.skillData != null ) {
					var flyCollisionNode = SkillWindow.CreateNode<SkillStructNode>( flyStruct.collider.skillData, flyNode );
				}
			}
		}

		private void CreateHittruct(SkillHitStruct hitStruct, bool canNull = false)
		{
			if( !hitStruct.isNull || canNull) {
				var flyNode = SkillWindow.CreateNode<SkillStructBaseNode>( hitStruct, this );

				if( hitStruct.collider.skillData != null ) {
					var flyCollisionNode = SkillWindow.CreateNode<SkillStructNode>( hitStruct.collider.skillData, flyNode );
				}
			}
		}
	}


}

