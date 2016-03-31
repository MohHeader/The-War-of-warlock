//
//  Node.cs
//
//  Author:
//       ${wuxingogo} <52111314ly@gmail.com>
//
//  Copyright (c) 2016 ly-user
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using UnityEngine;
using UnityEditor;
using System;


namespace node
{

	public abstract class Node
	{
		public abstract UnityEngine.Object Asset();

		public Rect Bounds{
			get{
				return bounds;
			}
			set{
				bounds = value;
				OnChangeBounds();
			}
		}
		public Rect bounds = new Rect(0,0,100,100);

		protected virtual void OnChangeBounds(){

		}

		public Node()
		{
			
		}


		Vector2 position = Vector2.zero;

		public Vector2 Position {
			get {
				return position;
			}
			set {
				position = value;

			}
		}

		protected Vector2 drawPosition = Vector2.zero;

		public Vector2 DrawPosition {
			get {
				return drawPosition;
			}
			set {
				drawPosition = value;
			}
		}

		[SerializeField]
		protected bool selected = false;

		public virtual void OnSelected(){
			
		}

		public bool Selected {
			get {
				return selected;
			}
			set {
				selected = value;
			}
		}

		private bool isDragging = false;

		public bool IsDragging {
			get {
				return isDragging;
			}
			set {
				isDragging = value;
			}
		}

		public void DragNode(Vector2 delta)
		{
			Bounds = new Rect( Bounds.position + delta, Bounds.size );
		}

		public Rect DrawBounds{
			get{
				return new Rect( Bounds.position + drawPosition, Bounds.size );
			}
		}

		public virtual void Draw()
		{
//			GUI.Box(  DrawBounds, "" );
		}

	}
}

