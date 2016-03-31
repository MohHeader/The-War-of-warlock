using UnityEngine;
using wuxingogo.Runtime;
using skill;


public class MoveableObjectRuntime : XMonoBehaviour
{
	public Transform moveTarget = null;
	public Transform finishTarget = null;
	/// <summary>
	/// 目标,是否打中
	/// </summary>
	public System.Action<Transform, bool> OnHitEvent = null;

	[Space]
	[Disable( true )] public Vector3 finishPosition = Vector3.zero;
	[SerializeField][Disable] float elapseTimer = 0;
	[SerializeField][Disable] float Timer = 1;
	[SerializeField][Disable] float oldDistance = 0;
	[System.NonSerialized]
	public MoveableObject mo = null;

	public AnimationCurve xCurve = AnimationCurve.Linear( 0, 0, 1, 0 );
	public AnimationCurve yCurve = AnimationCurve.Linear( 0, 0, 1, 0 );
	float oldSpeed = 0;
	public float speedZ = 1;
	public bool isFollow = false;

	public ITargetBinder skillTarget = null;

	void Start()
	{
		ResetTarget();
	}

	void ClearPos()
	{
		moveTarget.localPosition = Vector3.zero;
		moveTarget.localRotation = Quaternion.identity;
		moveTarget.localScale = Vector3.one;
	}

	public void ResetTarget()
	{
		if( skillTarget != null ) {
			finishTarget = skillTarget.GetBinder( 0 );
		}
		if( null != finishTarget ) {
			finishPosition = finishTarget.position;
			this.transform.forward = ( finishPosition - this.transform.position ).normalized;
		}



		int last = yCurve.keys.Length - 1;
		var frame = yCurve.keys[last];
		Timer = frame.time;
		oldDistance = ( finishPosition - moveTarget.position ).magnitude;
		oldSpeed = oldDistance / frame.time;

		elapseTimer = 0;
		this.enabled = true;
	}

	void Update()
	{
    	
		elapseTimer += Time.deltaTime * speedZ;

		Vector3 result = Vector3.zero;

		if( isFollow ) {
			float distance = ( finishTarget.position - this.transform.position ).magnitude;
			float newSpeed = distance / oldDistance * oldSpeed;
			this.transform.forward = ( finishTarget.position - this.transform.position ).normalized;

			result = new Vector3( xCurve.Evaluate( elapseTimer ), yCurve.Evaluate( elapseTimer ), elapseTimer * newSpeed );
		} else {
			
			result = new Vector3( xCurve.Evaluate( elapseTimer ), yCurve.Evaluate( elapseTimer ), elapseTimer * oldSpeed );
		}
		RaycastHit hitinfo;
		int layer = 1 << LayerMask.NameToLayer( "Blocks" ); 

		Vector3 dir = result - moveTarget.localPosition;
		Vector3 localDirection = this.transform.TransformDirection( dir.normalized );
		Debug.DrawRay( moveTarget.position, localDirection, Color.red, 2f );
		if( Physics.Raycast( moveTarget.position, localDirection, out hitinfo, 2, layer ) ) {
			if( OnHitEvent != null )
				OnHitEvent( moveTarget, false );
			Finish();
			return;
		} else {
			moveTarget.localPosition = result;
		}

		if( elapseTimer > Timer ) {
			if( OnHitEvent != null )
				OnHitEvent( finishTarget, true );
			Finish();
		}
	}

	private void Finish()
	{
		
		this.enabled = false;
		Destroy( this.gameObject );
	}
}