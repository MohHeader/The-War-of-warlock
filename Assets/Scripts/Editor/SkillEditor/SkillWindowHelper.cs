//
//  SkillWindowHelper.cs
//
//  Author:
//       ${wuxingogo} <52111314ly@gmail.com>
//
//  Copyright (c) 2016 ly-user
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using UnityEditor;


namespace skill
{

	class SkillWindowHelper
	{
		[UnityEditor.Callbacks.OnOpenAsset( 1 )]
		public static bool OnOpenAsset(int instanceID, int line)
		{
			var graph = Selection.activeObject as SkillTree;
			if( graph != null ) {
				SkillWindow.ShowWindow( graph );
				return true; //catch open file
			}

			return false; // let unity open the file
		}
	}

	[CustomEditor( typeof( SkillTree ) )]
	public class SkillTreeEditor : XScriptObjectEditor
	{
		public override void OnXGUI()
		{
			DoButton( "Open In Window", () => {
				SkillWindow.ShowWindow( target as SkillTree );
			} );
		}
	}

}

