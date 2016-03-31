//
//  NodeHelper.cs
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


namespace node
{

	class NodeInputHandler
	{
		/// <summary>
		/// Handles user input (keyboard and mouse)
		/// </summary>
		/// <param name="e">Input event</param>
		/// <param name="camera">Graph camera to convert to / from screen to world coordinates</param>
		/// <returns>true if the input was processed, false otherwise.</returns>
		public static bool HandleNodeInput(Node node, Event e, XWindowCamera camera)
		{
			bool inputProcessed = false;
			if( !node.IsDragging ) {
				// let the pins handle the input first
//                foreach (var pin in node.InputPins)
//                {
//                    if (inputProcessed) break;
//                    inputProcessed |= HandlePinInput(pin, e, camera);
//                }
//                foreach (var pin in node.OutputPins)
//                {
//                    if (inputProcessed) break;
//                    inputProcessed |= HandlePinInput(pin, e, camera);
//                }
			}

			var mousePosition = e.mousePosition;
			var mousePositionWorld = camera.ScreenToWorld( mousePosition );
			int dragButton = 0;
			// If the pins didn't already handle the input, then let the node handle it
			if( !inputProcessed ) {
				bool insideRect = node.Bounds.Contains( mousePositionWorld );
				if( e.type == EventType.MouseDown && insideRect && e.button == dragButton ) {
					node.IsDragging = true;
					inputProcessed = true;
				} else if( e.type == EventType.MouseUp && insideRect && e.button == dragButton ) {
					node.IsDragging = false;
				}
			}

			if( node.IsDragging && !node.Selected ) {
				node.IsDragging = false;
			}

			if( node.IsDragging && e.type == EventType.MouseDrag ) {
				inputProcessed = true;
			}

			return inputProcessed;
		}
	}

}

