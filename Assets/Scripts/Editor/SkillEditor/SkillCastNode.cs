//
//  SkillCastNode.cs
//
//  Author:
//       ${wuxingogo} <52111314ly@gmail.com>
//
//  Copyright (c) 2016 ly-user
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using node;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace skill
{

	public class SkillCastNode : SkillNode
	{
		CastStruct assets = null;
		SkillTree skillTree = null;
		public CastStruct SkillHandler()
		{
			return assets;
		}
		public override string Title {
			get {
				return "CastNode";
			}
		}
		public override void Init(ISkillEditorMarker para, System.Action<SkillStruct> OnInit = null)
		{
			base.Init(para);
			if(para != null)
			{
				this.skillTree = SkillWindow.skillTree;
				this.assets = (CastStruct)para;
				identifier = (CastStruct)para;
			}
		}

		public override UnityEngine.Object Handler {
			set {
				assets.gameObject = value as GameObject;
			}
		}
//		public override void MarkDirty()
//		{
//			EditorUtility.SetDirty(skillTree);
//		}
		public override StructBase marker {
			get {
				return assets;
			}
		}
		public override void RemoveFromParent()
		{
			skillTree.castStruct.Remove( SkillHandler() );

			base.RemoveFromParent();
		}
		public override UnityEngine.Object Asset()
		{
			return assets != null ? assets.gameObject : null;
		}

		public override void AddClickMenu(GenericMenu genericMenu)
		{
			if(assets.skillStruct == null && GetChild<SkillTemporaryNode>() == null)
				genericMenu.AddItem(new GUIContent("Skill Temporary"), false, AddNode);
			base.AddClickMenu(genericMenu);
		}

		private void AddNode(){
			SkillWindow.CreateNode<SkillTemporaryNode>(new SkillTemporaryNode(), this);
		}

		public override IEnumerable<SkillNode> GetChildrenNode()
		{
			var list = new List<SkillNode>();
			list.AddRange(base.GetChildrenNode());
			if(assets != null && assets.skillStruct != null)
			{
				var skillNode = SkillWindow.CreateNode<SkillStructNode>(assets.skillStruct, this);
				list.AddRange(skillNode.GetChildrenNode());
			}
			return list;
		}

	}
}

