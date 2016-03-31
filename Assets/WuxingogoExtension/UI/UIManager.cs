//
//  UIManager.cs
//
//  Author:
//       ${wuxingogo} <52111314ly@gmail.com>
//
//  Copyright (c) 2016 ly-user
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using UnityEngine;


namespace Scripts.UI
{
	public class UIManager : MonoBehaviour
	{
		private static UIManager instance = null;
		public static UIManager GetInstance()
		{
			return instance;
		}
		void Awake()
		{
			instance = this;
		}

		public JoystickUI joy = null;


	}
}

