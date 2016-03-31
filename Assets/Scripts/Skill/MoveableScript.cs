using UnityEngine;
using System.Collections;
using wuxingogo.Runtime;


public class MoveableScript : XMonoBehaviour
{

	[SerializeField] AnimationCurve curve = AnimationCurve.EaseInOut( 0, 0, 1, 0 );
	private float Timer = 0.0f;
	public Transform effectTransform = null;
	public Vector3 targetPos = Vector3.zero;
	private float oldDistance = 0.0f;
	private float oldSpeed = 0.0f;
	private float elapseTimer = 0.0f;
	public float speedZ = 1.0f;

	void Start()
	{
		targetPos = effectTransform.TransformPoint( targetPos );


		int last = curve.keys.Length - 1;
		var frame = curve.keys[last];
		Timer = frame.time;
		oldDistance = ( targetPos - effectTransform.position ).magnitude;
		oldSpeed = oldDistance / frame.time;
	}

	void Update()
	{
		elapseTimer += Time.deltaTime * speedZ;
		effectTransform.localPosition = new Vector3( curve.Evaluate( elapseTimer ), curve.Evaluate( elapseTimer ), elapseTimer * oldSpeed );

		if( elapseTimer > Timer ) {
			this.enabled = false;
		}
	}
}
