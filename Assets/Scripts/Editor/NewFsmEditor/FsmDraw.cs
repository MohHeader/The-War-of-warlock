//
//  FsmDraw.cs
//
//  Author:
//       ${wuxingogo} <52111314ly@gmail.com>
//
//  Copyright (c) 2016 ly-user
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using UnityEngine;
using System.Collections;
using UnityEditor;


namespace behaviour
{

	public class FsmDraw : XBaseWindow
	{
		public void DrawBox(FsmState fsmState)
		{
			GUI.Box( new Rect( fsmState.position, new Vector2( 100, 100 ) ), "", XStyles.GetInstance().window );
			fsmState.name = GUI.TextField( new Rect( fsmState.position, new Vector2( 100, 20 ) ), fsmState.name, XStyles.GetInstance().box );

			for( int i = 0; i < fsmState.Count; i++ ) {
				//  TODO loop in fsmState.Count
				var rect = GetEventRect( fsmState[i] );
				if( GUI.Button( rect, fsmState[i].name, XStyles.GetInstance().button ) ) {
					FsmEditor.selectEvent = fsmState[i];
				}
			}
		}

		internal Rect GetEventRect(FsmEvent fsmEvent)
		{
			var fsmState = fsmEvent.owner;
			var index = fsmEvent.owner.FindIndex( fsmEvent );
			return new Rect( fsmState.position.x, fsmState.position.y + 25 + ( 80 / fsmState.Count * index ), 100, 100 / 2 / fsmState.Count );
		}

		public void DragLine(FsmEvent fsmEvent, Vector2 mousePosition)
		{
			if( fsmEvent != null ) {
				var rect = GetEventRect( fsmEvent );
				DrawLine(rect, mousePosition);
			}
		}

		internal void DrawLine(Rect rect, Vector2 position)
		{
			
			var dir = (rect.position - position).normalized;
			Vector3 startPos = new Vector3( rect.position.x, rect.position.y, 0 );
			Vector3 endPos = new Vector3( position.x, position.y, 0 );

			var distance = ( startPos - endPos ).magnitude * 0.5f;
			Vector3 startTan = startPos + Vector3.right * distance;
			Vector3 endTan = endPos + Vector3.left * distance;
			Color shadowCol = new Color( 0, 0, 0, .06f );

			for( int i = 0; i < 3; i++ ) {
				Handles.DrawBezier( startPos, endPos, startTan, endTan, shadowCol, null, ( i + 1 ) * 5 );
				
			}
			Handles.DrawBezier( startPos, endPos, startTan, endTan, Color.gray, null, 1 );
		}
	}
}
