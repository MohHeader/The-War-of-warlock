//
//  SkillEffectBase.cs
//
//  Author:
//       ${wuxingogo} <52111314ly@gmail.com>
//
//  Copyright (c) 2016 ly-user
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using UnityEngine;

namespace skill
{
	[System.Serializable]
	public class SkillEffectBase : StructBase, ISkillEditorMarker
	{
		#region ISkillEditorMarker implementation

		public StructBase GetMarker()
		{
			return this;
		}

		#endregion

		public float delayTime = 0.0f;
		public GameObject effectGameObject = null;
		public int hitBindPos = -1;

		public SkillCollider collider = null;

		public bool isNull {
			get {
				return null == effectGameObject;
			}
		}
		public virtual string EffectType {
			get{
				return "None";
			}
		}

		public SkillEffectBase()
		{
		}
	}
}

