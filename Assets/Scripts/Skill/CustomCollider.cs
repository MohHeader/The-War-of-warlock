using UnityEngine;
using System.Collections;
using wuxingogo.Runtime;
using System.Collections.Generic;


namespace skill
{

	[System.Serializable]
	public class CustomCollider
	{
		[System.Flags]
		public enum ColliderMoment
		{
			None = 0x0,
			OnExit = 0x2,
			OnEnter = 0x4,
			OnUpdate = 0x8,
			OnDestroy = 0x10
		}

		public enum ColliderType
		{
			Sphere = 0x0,
			Box = 0x2,
			Fan = 0x4
		}

		[Header( "技能步骤ID" )]
		public int stepID = 0;

		[EnumFlagAttribute]
		public ColliderMoment moment = ColliderMoment.None;

		public ColliderType type = ColliderType.Box;

		public bool isHitDestroy = false;
		public bool isDestroyGameObject = false;
		public float lifeTime = 0;

		[Disable( true )]
		[Header( "只有isScale == true 才可触发曲线变幻" )]
		public bool isScale = true;
		[Header( "形状变化曲线" )]
		public AnimationCurve shapeCurve = AnimationCurve.Linear( 0, 0, 1, 1 );

		public Vector3 initSize = Vector3.one;
		public float radius = 0;
		public float angle = 0;
		public float interval = 0;
		public int maxCount = 3;


		public void Init(ISkillReleaser skillTree, GameObject emancipator, System.Action<ISkillReleaser, ISkillCanBeTarget> onHit)
		{
			if( emancipator != null ) {
				CustomColliderRuntime runtime = emancipator.AddComponent<CustomColliderRuntime>();
				runtime.Init( skillTree, this, isScale, onHit );

			}
		}



	}
}




