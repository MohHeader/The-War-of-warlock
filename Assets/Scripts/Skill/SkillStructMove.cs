using UnityEngine;
using System.Collections;
using skill;
using wuxingogo.Runtime;


[CreateAssetMenu( fileName = "SkillStructMove", menuName = "Wuxingogo/SkillStructMove" )]
public class SkillStructMove : SkillStruct
{
	public MoveableObject moveCurve = null;
	public SkillCollider collider = null;
	public MoveType moveType = MoveType.释放者;

	public enum MoveType
	{
		自身,
		释放者}

	;

	public SkillStructMove()
	{
		type = SkillType.Move;
	}

	public override void Init(ISkillReleaser skillTree, ISkillCanBeTarget ISkillCanBeTarget)
	{
		if( ISkillCanBeTarget.canMoveable ) {
			return;
		}
		var MoveISkillCanBeTarget = new GameObject( "MoveISkillCanBeTarget" );
		AlignTransfrom( MoveISkillCanBeTarget.transform, ISkillCanBeTarget.GetRuntime().transform );

		switch( moveType ) {
			case MoveType.释放者:
				moveCurve.InitMoveOther( MoveISkillCanBeTarget, ISkillCanBeTarget, skillTree );
			break;
			case MoveType.自身:
				moveCurve.InitMoveSelf( MoveISkillCanBeTarget, ISkillCanBeTarget, skillTree );
			break;
			default:
				Debug.LogError( "Move Type Error!" );
				Debug.Break();
			break;
		}
	}

	//	public override void Init(ISkillReleaser skillTree, GameObject emancipator, ITargetMovement bindPos, GameObject self = null, ISkillCanBeTarget target = null)
	//	{
	//		if( !isNull ) {
	//			Init( skillTree, emancipator, self );
	//		}
	//	}
}

