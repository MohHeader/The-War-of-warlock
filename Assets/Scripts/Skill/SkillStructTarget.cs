//
//  SkillStructTarget.cs
//
//  Author:
//       ${wuxingogo} <52111314ly@gmail.com>
//
//  Copyright (c) 2016 ly-user
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using UnityEngine;
using skill;



[CreateAssetMenu( fileName = "SkillStructTarget", menuName = "Wuxingogo/SkillStructTarget" )]
public class SkillStructTarget : SkillStruct
{
	public SkillStructTarget()
	{
		type = SkillType.Target;
	}


	public SkillFlyStruct flyStruct = null;

	public SkillHitStruct hitStruct = null;

	public override void Init(ISkillReleaser skillTree, ISkillCanBeTarget source, ISkillCanBeTarget dest)
	{
		if( !flyStruct.isNull ) {
			/// 从技能发射点出来
			GameObject go = flyStruct.CreateEffect( source.GetBinder() );

			flyStruct.Init( skillTree, dest, go, (t, isHit) => {
				if( isHit )
					dest.OnHitSkill( skillTree );
				if( !hitStruct.isNull ) {
					GameObject go1 = hitStruct.CreateHitEffect( t );
					hitStruct.Init( skillTree, go1 );
					Destroy( go1, 5.0f );
				}

			} );

		} else {
			if( !hitStruct.isNull ) {
				GameObject go = hitStruct.CreateHitEffect( dest.GetBinder() );
				hitStruct.Init( skillTree, go );
				Destroy( go, 5.0f );
			}
		}
	}

	public override void Init(ISkillReleaser skillTree, ISkillCanBeTarget target)
	{
		GameObject go = hitStruct.CreateHitEffect( target.GetBinder() );
		hitStruct.Init( skillTree, go );
		Destroy( go, 5.0f );
	}

	/// <summary>
	/// 打到墙上
	/// </summary>
	/// <param name="skillTree">Skill tree.</param>
	/// <param name="target">Target.</param>
	public override void Init(ISkillReleaser skillTree, Transform target)
	{
		if( !flyStruct.isNull ) {
			/// 从技能发射点出来
			GameObject go = flyStruct.CreateEffect( target );

			flyStruct.Init( skillTree, go, (t, isHit) => {
				if( hitStruct.isNull ) {
					GameObject go1 = hitStruct.CreateHitEffect( t );
					hitStruct.Init( skillTree, t.gameObject );
					Destroy( go1, 5.0f );
				}
			} );

		} else {
			if( !hitStruct.isNull ) {
				GameObject go = hitStruct.CreateHitEffect( target );
				hitStruct.Init( skillTree, target.gameObject );
				Destroy( go, 5.0f );
			}
		}
	}

	//	private void Init(ISkillReleaser skillTree, ITargetMovement bindPos, ISkillCanBeTarget target, GameObject self)
	//	{
	//		if( !flyStruct.isNull ) {
	//			/// 从技能发射点出来
	//			GameObject go = flyStruct.CreateEffect( bindPos, self );
	//
	//			flyStruct.Init( skillTree, target.GetMovement(), go, (t) => {
	//
	//				GameObject go1 = hitStruct.CreateHitEffect( target.GetMovement() , go);
	//				hitStruct.Init( skillTree, target.GetMovement(), go1 );
	//				Destroy(go1, 5.0f);
	//
	//			} );
	//
	//		} else {
	//			GameObject go = hitStruct.CreateHitEffect( target.GetMovement() );
	//			hitStruct.Init( skillTree, go );
	//			Destroy(go, 5.0f);
	//		}
	//	}

	//	public override void Init(ISkillReleaser skillTree, GameObject emancipator, ITargetMovement bindPos, GameObject self = null, ISkillCanBeTarget target = null)
	//	{
	////		if( !isNull ) {
	////			Init( skillTree, bindPos, target,self );
	////		}
	//	}

	

	public override bool isNull {
		get {
			return flyStruct.isNull && hitStruct.isNull;
		}
	}
}


