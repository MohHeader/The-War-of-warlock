//
//  XBehaviorNode.cs
//
//  Author:
//       ${wuxingogo} <52111314ly@gmail.com>
//
//  Copyright (c) 2015 wuxingogo
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using UnityEditor;
using UnityEngine;
using wuxingogo.Runtime;
using System.Collections.Generic;

namespace XBehaviorEditor
{

	public class XBehaviorStateNode : XBaseWindow
	{
		public Rect GraphRect = new Rect( Screen.width / 2, Screen.height / 2, 280, 150 );
		
		
		public XBehaviorState state = null;

		public int actionID = -1;
		public int eventID = -1;
		
		private bool isDirty = true;
		
		private List<Rect> jointRects = new List<Rect>();

		public XBehaviorStateNode(XBehaviorState state)
		{
			this.state = state;
		}
		
		public virtual void Draw(int id){
			GraphRect = GUI.Window(id, GraphRect, Drag, state.Name);
		}	
		
		private void Drag(int id){
			DrawGraph();
			GUI.DragWindow();	
		}
		
		public virtual void DrawGraph(){
			_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos); 
			
			state.Name = EditorGUILayout.TextField(state.Name);
			
			DrawActions();
			
			DrawEvents();
			
			EditorGUILayout.EndScrollView();
		}
		
		public void DrawActions(){
			if(null != state.actions){
				for( int pos = 0; pos < state.actions.Count; pos++ ) {
					//  TODO loop in state.actions.Count
					
					if(CreateSpaceButton(state.actions[pos].name)){
						if(actionID == pos){
							actionID = -1;
						}else{
							actionID = pos;
						}
						
					}
					if( pos == actionID ){
						DrawProperty(new SerializedObject(state.actions[actionID]));
						if(CreateSpaceButton("Delete")){
							state.actions.RemoveAt(actionID);
							actionID = -1;
						}
					}
				}
			}
		}
		
		public void DrawEvents(){
			// draw event
			
			
			bool isFix = false; //  GUILayoutUtility.GetRect(20,20) DeEquals Rect(20,20) at First time
			for( int pos = 0; pos < state.events.Count; pos++ ) {
				//  TODO loop in state.events.Count
				BeginHorizontal();
				if(CreateSpaceButton(state.events[pos].name)){
					if(eventID == pos){
						eventID = -1;
						XBehaviorEditor.GetInstance().ClearTransition();
					}else{
						eventID = pos;
					}
				}
				
				GUIStyle style = GUI.skin.horizontalSliderThumb;
				
				
				if(GUILayout.Button("", style)){
					// selected joint node
					
					if(eventID == pos){
						eventID = -1;
						XBehaviorEditor.GetInstance().ClearTransition();
					}else{
						eventID = pos;
						XBehaviorEditor.GetInstance().SetBehaviorEvent(state.events[pos]);
					}
				}
				

				if(jointRects.Count <= pos){
					jointRects.Add(new Rect());
				}
				jointRects[pos] = GUILayoutUtility.GetLastRect();
				
				if(eventID == pos && jointRects.Count > pos){
				
					EndHorizontal();
					EditorGUI.DrawRect(jointRects[pos], Color.black);
					
					BeginHorizontal();
					state.events[pos].name = CreateStringField(state.events[pos].name);
					
					if(CreateSpaceButton("Delete")){
						state.events.RemoveAt(pos);
						state.events.TrimExcess();
						jointRects.RemoveAt(pos);
						jointRects.TrimExcess();
						eventID = -1;
						return;
					}
				}
				EndHorizontal();
			}
		}
		
		public void AddFSMAction(XBehaviorAction action){
			state.AddAction(action);
		}
		public void AddFSMEvent(XBehaviorEvent behEvent){
			state.AddEvent(behEvent);
			isDirty = true;
		}
		public void DrawProperty(SerializedObject obj){
			SerializedProperty property = null;
			property = obj.GetIterator();
			while(property.NextVisible(true)){
				EditorGUILayout.PropertyField(property, new GUILayoutOption[0]);
			}
			obj.ApplyModifiedProperties();
		}
		
		public Vector3 GetJointPos(int idx){
			if(jointRects.Count <= idx)
				return new Vector3(0,0,0);
			return new Vector3(GraphRect.x + jointRects[idx].x + jointRects[idx].width, GraphRect.y + jointRects[idx].y + jointRects[idx].height * 2.0f - _scrollPos.y, 0);
		}
	}
	
	
}
