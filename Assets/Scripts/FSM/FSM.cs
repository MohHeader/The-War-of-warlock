using UnityEngine;
using System.Collections.Generic;
using wuxingogo.Runtime;
using System;
using System.Linq;


namespace behaviour
{

	public class FSM : XMonoBehaviour
	{
		public List<FsmState> stateCollection = new List<FsmState>();

		public FsmState currState = null;

		public FsmState TryGetState(FsmState fsmState)
		{
			return stateCollection.Find( fs => fs.Equals( fsmState ) );
		}

		public void AddState(FsmState fsmState)
		{
			stateCollection.Add( fsmState );
		}

		public void RemoveState(FsmState fsmState)
		{
			if( stateCollection.Contains( fsmState ) ) {
				stateCollection.Remove( fsmState );
				if( currState == fsmState )
					currState = null;
			}
			else
				Debug.Log( string.Format( "{0} No Contain {1} State", this.ToString(), fsmState.ToString() ) );
		}

		public void SetDefaultState(FsmState fsmState)
		{
			currState = fsmState;
		}

		public void SetState(FsmState fsmState)
		{
			currState.OnExit();
			currState = fsmState;
			currState.OnEnter();
		}

		void Update()
		{
			if( currState != null )
				currState.OnUpdate();
		}
	}

	[System.Serializable]
	public class FsmEvent
	{
		public string name = "default event";
		[Disable]
		public FsmState owner = null;
		public FsmState target = null;

		public virtual bool CanDoNext {
			get {
				return true;
			}
			set {

			}
		}
	}
}
