using UnityEngine;
using System.Collections;
using wuxingogo.Runtime;
using skill;


[System.Serializable]
public class MoveableObject
{
	[Header( "技能步骤ID" )]
	public int stepID = 0;
	[Header( "Y曲线" )]
	public AnimationCurve yCurve = AnimationCurve.EaseInOut( 0, 0, 1, 0 );
	[Header( "X曲线" )]
	public AnimationCurve xCurve = AnimationCurve.EaseInOut( 0, 0, 1, 0 );


	[Header( "时间比例" )]
	public float speedZ = 1;
	[Header( "是否跟随" )]
	[Disable( true )]public bool isFollow = false;
	[Header( "是否使用物理碰撞" )]
	[Disable( true )] public bool isPhysics = false;

	[System.NonSerialized]
	public Transform moveTarget = null;

	[Header( "移动距离(无目标)" )]
	public float distance = 0.0f;
	[Disable( true )]
	public Vector3 targetPos = Vector3.zero;

	public void Init(System.Action<Transform, bool> onFinish = null, ITargetBinder skillTarget = null)
	{
		MoveableObjectRuntime mor = moveTarget.gameObject.AddComponent<MoveableObjectRuntime>();
		mor.mo = this;
		if( skillTarget != null )
			mor.skillTarget = skillTarget;
		else {
			mor.finishPosition = moveTarget.forward.normalized * distance + moveTarget.position;
		}
	
		mor.moveTarget = moveTarget.GetChild( 0 );
		mor.OnHitEvent = onFinish;
		mor.xCurve = this.xCurve;
		mor.yCurve = this.yCurve;
		mor.isFollow = this.isFollow;
		mor.speedZ = this.speedZ;

	}

	/// <summary>
	/// 移动人物,把人拖拽到一个物体下,然后对物体增加MoveableObjectRuntime,然后开始拖拽,最后把人物再拽回来
	/// </summary>
	public void InitMoveOther(GameObject moveGo, ISkillCanBeTarget target, ISkillReleaser skillTree)
	{
		
		MoveableObjectRuntime mor = moveGo.AddComponent<MoveableObjectRuntime>();
		mor.mo = this;

		Vector3 distanceVec = ( target.GetRuntime().transform.position - skillTree.GetBehaviour().transform.position );
		Debug.Log( "distanceVec is " + distanceVec );
		mor.finishPosition = distanceVec.normalized * distance + target.GetRuntime().transform.position;
		Debug.Log( "distanceVec.normalized is " + distanceVec.normalized );
		mor.moveTarget = target.GetRuntime().transform;
		mor.xCurve = this.xCurve;
		mor.yCurve = this.yCurve;
		mor.isFollow = this.isFollow;
		mor.speedZ = this.speedZ;

		target.GetRuntime().transform.SetParent( moveGo.transform );
		mor.OnHitEvent = (t, b) => {

			target.GetRuntime().transform.SetParent( null );
			mor.moveTarget = null;
		};

	}


	/// <summary>
	/// 移动人物,把人拖拽到一个物体下,然后对物体增加MoveableObjectRuntime,然后开始拖拽,最后把人物再拽回来
	/// </summary>
	public void InitMoveSelf(GameObject moveGo, ISkillCanBeTarget skillTarget, ISkillReleaser skillTree)
	{

		skillTarget.canMoveable = true;
		MoveableObjectRuntime mor = moveGo.AddComponent<MoveableObjectRuntime>();
		mor.mo = this;

//		Vector3 distanceVec = (target.transform.position - skillTree.transform.position );

		mor.finishPosition = skillTarget.GetRuntime().transform.forward * distance + skillTarget.GetRuntime().transform.position;
		Debug.Log( "mor.targetPos is " + mor.finishPosition );
		mor.moveTarget = skillTarget.GetRuntime().transform;
		mor.xCurve = this.xCurve;
		mor.yCurve = this.yCurve;
		mor.isFollow = this.isFollow;
		mor.speedZ = this.speedZ;

		skillTarget.GetRuntime().transform.SetParent( moveGo.transform );
		mor.OnHitEvent = (t, b) => {
			if( b ) {
				skillTarget.canMoveable = false;
				skillTarget.GetRuntime().transform.SetParent( null );
			}
			mor.moveTarget = null;
			GameObject.Destroy( mor.gameObject );
		};

	}

}
