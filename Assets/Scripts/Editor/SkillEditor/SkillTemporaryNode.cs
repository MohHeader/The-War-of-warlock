//
//  SkillTemporaryNode.cs
//
//  Author:
//       ${wuxingogo} <52111314ly@gmail.com>
//
//  Copyright (c) 2016 ly-user
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;


namespace skill
{

	public class SkillTemporaryNode : SkillNode, ISkillEditorMarker
	{
		#region ISkillEditorMarker implementation
		public StructBase GetMarker()
		{
			return marker;
		}
		#endregion

		public override StructBase marker {
			get {
				return _marker;
			}
		}
		private StructBase _marker = null;
		public SkillTemporaryNode()
		{
			_marker = new StructBase();
			_marker.GenerateGUID();
		}

		public override string Title {
			get {
				return "TemporaryNode";
			}
		}


		public override void Init(ISkillEditorMarker para, System.Action<SkillStruct> OnInit = null)
		{
			base.Init(para, OnInit);
			this._marker = para.GetMarker();
			identifier = para;

		}

		public override UnityEngine.Object Asset()
		{
			return null;
		}
	}
}

