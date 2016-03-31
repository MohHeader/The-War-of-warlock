//
//  SkillGameObjectNode.cs
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
using Object = UnityEngine.Object;


namespace skill
{

	public class SkillStructBaseNode : SkillNode
	{
		
		SkillEffectBase effectStruct = null;
		GameObject gameObject = null;
		Action<GameObject> OnChangeGameObject = null;
		public override string Title {
			get {
				return title;
			}
		}
		private string title = "";
		public SkillStructBaseNode()
		{
			title = "None";
		}
		public override void Init(ISkillEditorMarker dataMarker, System.Action<SkillStruct> OnInit = null)
		{
			base.Init( dataMarker );

			if( dataMarker != null ) {
				this.effectStruct = dataMarker as SkillEffectBase;
				
				this.gameObject = effectStruct.effectGameObject;
				this.identifier = dataMarker;
				OnChangeGameObject = (g) => {
					effectStruct.effectGameObject = g;
					this.gameObject = g;
				};
				OnChangeHandler = (s)=>{
					effectStruct.collider.skillData = s as SkillStruct;
				};
				title = effectStruct.EffectType;
//				if( dataMarker is SkillFlyStruct ) {
//					var flyStruct = dataMarker as SkillFlyStruct;
//					this.gameObject = flyStruct.flyGo;
//					this.identifier = gameObject;
//					OnChangeGameObject = (g) => {
//						flyStruct.flyGo = g;
//						this.gameObject = g;
//					};
//					OnChangeHandler = (s) => {
//						flyStruct.collider.skillData = s as SkillStruct;
//					};
//					title = "FlyState";
//				} else if( dataMarker is SkillHitStruct ) {
//					var hitStruct = dataMarker as SkillHitStruct;
//					this.gameObject = hitStruct.hitGo;
//					this.identifier = gameObject;
//					OnChangeGameObject = (g) => {
//						hitStruct.hitGo = g;
//						this.gameObject = g;
//					};
//					OnChangeHandler = (s) => {
//						hitStruct.collider.skillData = s as SkillStruct;
//					};
//					title = "HitState";
//				}
			}
		}

		private Action<Object> OnChangeHandler = null;
		public virtual Object Handler{
			set{
				if( OnChangeHandler != null )
					OnChangeHandler( value );
			}
		}

		public override void AddClickMenu(UnityEditor.GenericMenu genericMenu)
		{
			SkillStruct dataStruct = effectStruct.collider.skillData;

			if(dataStruct == null && GetChild<SkillTemporaryNode>() == null)
				genericMenu.AddItem(new GUIContent("Skill Temporary"), false, AddNode);
			base.AddClickMenu(genericMenu);
		}
		private void AddNode(){
			SkillWindow.CreateNode<SkillTemporaryNode>(new SkillTemporaryNode(), this, (t)=> {
				effectStruct.collider.skillData = t;
			});
		}
		public override object identifier {
			get {
				return base.identifier;
			}
			protected set {
				base.identifier = value;
				if(OnChangeGameObject != null)
					OnChangeGameObject( value as GameObject );
			}
		}

		public override void RemoveFromParent()
		{
			OnChangeGameObject( null );
			base.RemoveFromParent();
		}
		public override StructBase marker {
			get {
				return effectStruct;
			}
		}

		public override UnityEngine.Object Asset()
		{
			return gameObject;
		}
	}
}

