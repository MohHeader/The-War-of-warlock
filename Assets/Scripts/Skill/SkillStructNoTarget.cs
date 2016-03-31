using UnityEngine;
using System.Collections;
using skill;



[CreateAssetMenu( fileName = "SkillStructNoTarget", menuName = "Wuxingogo/SkillStructNoTarget" )]
public class SkillStructNoTarget : SkillStruct
{
	public SkillStructNoTarget()
	{
		type = SkillType.NoTarget;
	}

	public SkillFlyStruct flyStruct = null;

	public SkillHitStruct hitStruct = null;

	void AlignTransfrom(Transform lhs, Transform rhs)
	{
		lhs.position = rhs.position;
		lhs.rotation = rhs.rotation;
	}

	public override void Init(ISkillReleaser skillTree, ITargetBinder bindPos)
	{
//		GameObject colliderGo = new GameObject( "HelperCollider" );
//		AlignTransfrom( colliderGo.transform, bindPos.transform );
//		collider.Init(skillTree, colliderGo, (HelperSkill sk, SkillTarget target) => {
		if( !flyStruct.isNull ) {
			/// 从技能发射点出来
			GameObject go = flyStruct.CreateEffect( bindPos );

			flyStruct.Init( skillTree, go, (t, isHit) => {
				if( !hitStruct.isNull && t != null ) {
					GameObject go1 = hitStruct.CreateHitEffect( t );
					hitStruct.Init( skillTree, go1 );
					Destroy( go1, 5.0f );
				}

			} );

		} else {
			if( !hitStruct.isNull ) {
				GameObject go = hitStruct.CreateHitEffect( bindPos );
				hitStruct.Init( skillTree, go );
				Destroy( go, 5.0f );
			}
		}

	}



	public override void Init(ISkillReleaser skillTree, Transform noTarget)
	{
		if( !flyStruct.isNull ) {
			/// 从技能发射点出来
			GameObject go = flyStruct.CreateEffect( noTarget );

			flyStruct.Init( skillTree, go, (t, isHit) => {

//				GameObject go1 = hitStruct.CreateHitEffect( null, t.gameObject );
//				hitStruct.Init( skillTree, go1 );
//				Destroy(go, 5.0f);
			} );

		} else {
			GameObject go = hitStruct.CreateHitEffect( noTarget );
			hitStruct.Init( skillTree, noTarget.gameObject );
			Destroy( go, 5.0f );

		}
	}

	//	public override void Init(ISkillReleaser skillTree, GameObject emancipator, GameObject self = null, ISkillCanBeTarget target = null)
	//	{
	//		if( !isNull ) {
	//			Init( skillTree,self );
	//		}
	//	}

	public override bool isNull {
		get {
			return flyStruct.isNull && hitStruct.isNull;
		}
	}
}

