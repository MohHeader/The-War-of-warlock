using UnityEngine;
using System.Collections;
using behaviour;


public class StartState : FsmState
{

	[SerializeField]
	FsmEvent onGameEnter;

	[SerializeField]
	FsmEvent onExitGame;

	public override void InitDeserialize()
	{
		ClearEvent();
		AddEvent( onGameEnter );
		AddEvent( onExitGame );
	}

}
