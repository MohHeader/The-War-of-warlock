//
//  SkillModel.cs
//
//  Author:
//       ${wuxingogo} <52111314ly@gmail.com>
//
//  Copyright (c) 2016 ly-user
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using UnityEngine;
using System.Collections.Generic;
using wuxingogo.Runtime;
using System.Collections;


namespace skill
{

	[System.Serializable]
	public class SkillFlyStruct : SkillEffectBase
	{

		/// FlyCurve
		public MoveableObject flyCurve = null;
		public bool isFollowPos = false;
		public override string EffectType {
			get {
				return "FlyStruct";
			}
		}

		public void FinishFlySkllTarget(ISkillCanBeTarget target)
		{
			// TODO LIST
		}



		public void Init(ISkillReleaser skillTree, ISkillCanBeTarget ISkillCanBeTarget, GameObject realityGo, System.Action<Transform, bool> onHit = null)
		{
			
			flyCurve.moveTarget = realityGo.transform;

			flyCurve.Init( onHit, ISkillCanBeTarget.GetBinder() );

			if( collider != null && !collider.isNull ) {
				///飞行效果装置碰撞一定要在effect下一层加,第一层是移动脚本位置,本身不会动.
				collider.Init( skillTree, realityGo.transform.GetChild( 0 ).gameObject );
			}

		}

		/// <param name="skillTree">Skill tree.</param>
		/// <param name="realityGo">Reality GameObject.</param>
		/// <param name="onHit">目标Transform,是否打中ISkillCanBeTarget</param>
		public void Init(ISkillReleaser skillTree, GameObject realityGo, System.Action<Transform, bool> onHit = null)
		{
			
			flyCurve.moveTarget = realityGo.transform;

			flyCurve.Init( onHit );

			if( collider != null && !collider.isNull ) {
				///飞行效果装置碰撞一定要在effect下一层加,第一层是移动脚本位置,本身不会动.
				collider.Init( skillTree, realityGo.transform.GetChild( 0 ).gameObject );
			}

		}

		/// <summary>
		/// Creates the effect.
		/// </summary>
		/// Target是自身绑定点,如果有则从绑定点位置创建.
		/// isFollowPos是否跟随绑定点,跟随必须有绑定点.(约束)
		public GameObject CreateEffect(ITargetBinder Target)
		{
			GameObject go = null;
			go = GameObject.Instantiate( effectGameObject ) as GameObject;
			Transform ptf = null;

			ptf = Target.GetBinder( hitBindPos );
			go.transform.rotation = Target.GetBinder(-1).rotation;
			if( isFollowPos )
				go.transform.SetParent( Target.GetBinder(-1) );
			else
				go.transform.parent = null;
			go.transform.position = ptf.position;
			go.SetActive( true );
			return go;
		}

		/// <summary>
		/// Creates the effect.
		/// </summary>
		/// Target是自身绑定点,如果有则从绑定点位置创建.
		/// isFollowPos是否跟随绑定点,跟随必须有绑定点.(约束)
		public GameObject CreateEffect(Transform transform)
		{
			GameObject go = null;
			go = GameObject.Instantiate( effectGameObject ) as GameObject;
			Transform ptf = null;

			go.transform.rotation = transform.rotation;

			if( isFollowPos )
				go.transform.SetParent( transform );
			else
				go.transform.parent = null;
			go.transform.position = ptf.position;
			go.SetActive( true );
			return go;
		}

	}

	[System.Serializable]
	public class SkillCollider
	{
		public CustomCollider collider = null;
		public SkillStruct skillData = null;

		public void Init(ISkillReleaser skillTree, GameObject emancipator)
		{
			collider.Init( skillTree, emancipator, OnHitTarget );
		}

		public bool isNull {
			get {
				return skillData == null ? true : skillData.isNull;
			}
		}

		void OnHitTarget(ISkillReleaser skillTree, ISkillCanBeTarget target)
		{
			skillData.Init( skillTree, target );
			target.OnHitSkill( skillTree );
		}

		void OnHitNoTarget(ISkillReleaser skillTree, Transform target)
		{
			skillData.Init( skillTree, target );
		}

	}

	[System.Serializable]
	public class SkillHitStruct : SkillEffectBase
	{
		#region ISkillEditorMarker implementation

		public StructBase GetMarker()
		{
			return this;
		}

		#endregion

		public override string EffectType {
			get {
				return "HitStruct";
			}
		}

		public void Init(ISkillReleaser skillTree, GameObject realityGo)
		{
			if( collider != null && !collider.isNull ) {
				collider.Init( skillTree, realityGo );
			}
		}

		public GameObject CreateHitEffect(Transform self)
		{
			GameObject go = null;
			go = GameObject.Instantiate( effectGameObject ) as GameObject;
			Transform ptf = null;

			ptf = self;
			go.transform.rotation = self.rotation;
			
			go.transform.parent = null;
			go.transform.position = ptf.position;
			go.SetActive( true );
			return go;
		}

		public GameObject CreateHitEffect(ITargetBinder Owner)
		{
			GameObject go = null;
			go = GameObject.Instantiate( effectGameObject ) as GameObject;
			Transform ptf = null;

			ptf = Owner.GetBinder( hitBindPos );
			go.transform.rotation = Owner.GetBinder(-1).rotation;
			go.transform.parent = null;
			go.transform.position = ptf.transform.position;
			go.SetActive( true );
			return go;
		}
	}

	[System.Serializable]
	public class CastStruct : StructBase, ISkillEditorMarker
	{
		#region ISkillEditorMarker implementation

		public StructBase GetMarker()
		{
			return this;
		}

		#endregion

		public GameObject gameObject = null;
		/// <summary>
		/// 选取敌人 + 技能数据
		/// </summary>
		public int hitBindPos = -1;
		public SkillEffectType skillEffectType = SkillEffectType.指定目标;

		public float startTime = 0.0f;
		public float endTime = 4.0f;

		public SkillStruct skillStruct = null;

		public CustomCollider collider = null;

		public bool isNull {
			get {
				return null == gameObject;
			}
		}

		void AlignTransfrom(Transform lhs, Transform rhs)
		{
			lhs.position = rhs.position;
			lhs.rotation = rhs.rotation;
		}

		public void Init(GameObject emancipator, ISkillReleaser skillTree, ITargetBinder bindPos)
		{
			GameObject castGo = CreateCastEffect( bindPos, hitBindPos, gameObject );

			skillTree.GetBehaviour().StartCoroutine(Initself(emancipator, skillTree, bindPos,castGo));
		}

		IEnumerator Initself(GameObject emancipator, ISkillReleaser skillTree, ITargetBinder bindPos, GameObject castGo)
		{
			yield return new WaitForSeconds( endTime );
			switch( skillEffectType ) {
				case SkillEffectType.指定目标:
					GameObject colliderGo = new GameObject( "HelperCollider" );
					AlignTransfrom( colliderGo.transform, bindPos.GetBinder(-1) );
					collider.Init( skillTree, colliderGo, (ISkillReleaser sk, ISkillCanBeTarget target1) => {
						skillStruct.Init( sk, skillTree.GetSelfTarget(), target1 );
					} );

				break;
				case SkillEffectType.指定地点:
					skillStruct.Init( skillTree, bindPos );
				break;
				case SkillEffectType.移动:
					skillStruct.Init( skillTree, skillTree.GetSelfTarget() );
				break;
			}
			Object.Destroy( castGo, 5.0f );
		}

		GameObject CreateCastEffect(ITargetBinder bind, int bindPos, GameObject effectPrefab)
		{
			GameObject go = null;
			if( effectPrefab != null )
				go = GameObject.Instantiate( effectPrefab ) as GameObject;
			else
				go = new GameObject();

			Transform ptf = bind.GetBinder( bindPos );

			go.transform.parent = ptf;
			go.transform.localPosition = Vector3.zero;
			go.transform.localRotation = Quaternion.identity;
			go.transform.localScale = Vector3.one;
			
			go.SetActive( true );      
			return go;
		}

		GameObject CreateFlyCastEffect(GameObject emancipator, ITargetBinder bind, int bindPos)
		{
			GameObject go = null;
			if( gameObject != null )
				go = GameObject.Instantiate( gameObject ) as GameObject;
			else
				go = new GameObject();

			Transform ptf = bind.GetBinder( bindPos );

			go.transform.position = ptf.position;
			go.transform.rotation = emancipator.transform.rotation;
			go.SetActive( true );      
			return go;
		}
	
	}

	public enum SkillEffectType
	{
		指定目标,
		指定地点,
		移动
	}
}

