using UnityEngine;
using wuxingogo.Runtime;
using System.Collections.Generic;


namespace behaviour
{

	public class FsmState : XScriptableObject, ISerializationCallbackReceiver
	{
		[Disable]
		public FSM owner = null;
		[SerializeField]
		protected List<FsmEvent> eventCollection = new List<FsmEvent>();

		protected void ClearEvent()
		{
			eventCollection.Clear();
		}

		#region ISerializationCallbackReceiver implementation


		public void OnBeforeSerialize()
		{
		}


		public void OnAfterDeserialize()
		{
			InitDeserialize();
		}

		public virtual void InitDeserialize()
		{
		}

		public virtual void OnExit()
		{
			
		}

		public virtual void OnEnter()
		{
			
		}

		public virtual void OnUpdate()
		{
			for( int i = 0; i < eventCollection.Count; i++ ) {
				//  TODO loop in eventCollection.Count
				if(eventCollection[i].CanDoNext)
				{
					owner.SetState(eventCollection[i].target);
				}
				break;
			}
		}

		public FsmEvent this[int index]
		{
			get{
				return eventCollection[index];
			}
		}

		public int Count
		{
			get{
				return eventCollection.Count;
			}
		}

		#endregion

		[X]
		public void AddEvent(FsmEvent fsmEvent)
		{
			if( !eventCollection.Contains( fsmEvent ) ) {
				fsmEvent.owner = this;
				eventCollection.Add( fsmEvent );
			}
		}

		public int FindIndex(FsmEvent fsmEvent)
		{
			return eventCollection.FindIndex((t)=> t.Equals(fsmEvent));
		}

		#if UNITY_EDITOR
		public Vector2 position = Vector2.zero;
		#endif
	}
}