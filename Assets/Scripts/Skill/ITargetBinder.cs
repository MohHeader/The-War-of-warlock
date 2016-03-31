//
//  ITargetMovement.cs
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
	
	public interface ITargetBinder
	{
		/// <summary>
		/// -1	自身
		/// 0		跨骨中心
		/// </summary>
		Transform GetBinder(int bindType);
	}
}

