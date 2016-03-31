using UnityEngine;
using wuxingogo.Runtime;
using System.Collections.Generic;
using wuxingogo.tools;
using skill;


public class CustomColliderRuntime : XMonoBehaviour
{
	[System.NonSerialized]
	public float curveTimer = 0;
	[System.NonSerialized]
	public float elapseTimer = 0;
	[System.NonSerialized]
	public bool isDestroy = false;
	[System.NonSerialized]
	public bool isScale = false;

	[Header( "碰撞内的目标" )]
	[Disable( false )]
	[SerializeField]
	List<ISkillCanBeTarget> targets = new List<ISkillCanBeTarget>();

	[System.NonSerialized]
	public CustomCollider colliderData = null;

	public System.Action<ISkillReleaser, ISkillCanBeTarget> onTrigger = null;

	[Disable( true )]
	[SerializeField]
	private Collider m_collider = null;
	private SectorShape m_sector = null;

	ISkillReleaser skillTree = null;

	public bool isFriendOrSelf(ISkillCanBeTarget target)
	{
		if( target == null )
			return true;
		return skillTree.isFriend( target );
//		ISkillReleaser emancipator = skillTree.GetComponent<HeadUnit>().owner;
//		return emancipator.IsFriend( target ) || emancipator.Equals( target );
	}

	public void OnTriggerEnter(Collider other)
	{
		if( isDestroy )
			return;
		
		var skillTarget = other.GetComponent<ISkillCanBeTarget>();
		if( skillTarget == null || isFriendOrSelf( skillTarget ) )
			return;
		if( targets.Count >= colliderData.maxCount )
			return;

		targets.Add( skillTarget );

		if( ( colliderData.moment & CustomCollider.ColliderMoment.OnEnter ) != CustomCollider.ColliderMoment.None ) {
			ExcuteSkill( skillTarget );
		}

	}

	public virtual void ExcuteSkill(ISkillCanBeTarget skillTarget)
	{
		if( colliderData.type == CustomCollider.ColliderType.Fan ) {
			SectorShape shape = GetComponent<SectorShape>();
			shape.Target = skillTarget.GetRuntime().transform;

			if( !shape.isInSector() )
				return;
		}

		onTrigger( skillTree, skillTarget );

		if( colliderData.isHitDestroy )
			Destroyself();
		
	}

	void OnTriggerExit(Collider other)
	{
		if( isDestroy )
			return;
		var skillTarget = other.GetComponent<ISkillCanBeTarget>();
		if( skillTarget == null || isFriendOrSelf( skillTarget ) )
			return;
		targets.Remove( skillTarget );
		if( ( colliderData.moment & CustomCollider.ColliderMoment.OnExit ) != CustomCollider.ColliderMoment.None ) {
			ExcuteSkill( skillTarget );
		}
	}

	public void Init(ISkillReleaser skillTree, CustomCollider colliderData, bool isScale, System.Action<ISkillReleaser, ISkillCanBeTarget> onTrigger)
	{
		this.colliderData = colliderData;
		this.isScale = isScale;
		this.onTrigger = onTrigger;
		this.skillTree = skillTree;
	}


	/// <summary>
	/// Init collider
	/// </summary>
	void InitCollider()
	{
			
		switch( colliderData.type ) {
			case CustomCollider.ColliderType.Box:
				BoxCollider box = gameObject.AddComponent<BoxCollider>();
				box.size = colliderData.initSize;
				box.isTrigger = true;
				m_collider = box;
			break;
			case CustomCollider.ColliderType.Fan:
				{
					SphereCollider sc = gameObject.AddComponent<SphereCollider>();
					sc.radius = colliderData.radius;
					m_sector = gameObject.AddComponent<SectorShape>();
					m_sector.m_Radius = colliderData.radius;
					m_sector.angle = colliderData.angle;

					sc.isTrigger = true;
					m_collider = sc;
				}
			break;
			case CustomCollider.ColliderType.Sphere:
				{
					SphereCollider sc = gameObject.AddComponent<SphereCollider>();
					sc.radius = colliderData.radius;
					sc.isTrigger = true;
					m_collider = sc;
				}
			break;
		}

		gameObject.layer = LayerMask.NameToLayer( "Bullet" );
	}

	void Start()
	{
		InitCollider();
		if( !colliderData.isHitDestroy ) {
			Destroyself( colliderData.lifeTime );
			if( ( colliderData.moment & CustomCollider.ColliderMoment.OnUpdate ) != CustomCollider.ColliderMoment.None ) {
				InvokeRepeating( "Updateself", 0.1f, colliderData.interval );
			}
		}

		if( colliderData.isScale ) {
			elapseTimer = 0;	
			switch( colliderData.type ) {
				case CustomCollider.ColliderType.Fan:
					colliderData.radius = GetComponent<SectorShape>().m_Radius;
				break;
				case CustomCollider.ColliderType.Box:
					colliderData.initSize = GetComponent<BoxCollider>().size;
				break;
				case CustomCollider.ColliderType.Sphere:
					colliderData.radius = GetComponent<SphereCollider>().radius;
				break;
			}

			int last = colliderData.shapeCurve.keys.Length - 1;
			var frame = colliderData.shapeCurve.keys[last];
			curveTimer = frame.time;
		}

	}

	void Update()
	{
		if( colliderData.isScale ) {
			elapseTimer += Time.deltaTime;

			float speed = colliderData.shapeCurve.Evaluate( elapseTimer );

			switch( colliderData.type ) {
				case CustomCollider.ColliderType.Fan:
					( (SphereCollider)m_collider ).radius = (float)colliderData.radius * speed;
					m_sector.m_Radius = (float)colliderData.radius * speed;
				break;
				case CustomCollider.ColliderType.Box:
					( (BoxCollider)m_collider ).size = (Vector3)colliderData.initSize * speed;
				break;
				case CustomCollider.ColliderType.Sphere:
					( (SphereCollider)m_collider ).radius = (float)colliderData.radius * speed;
				break;
			}
			if( elapseTimer > curveTimer )
				isScale = false;
		}
	}

	void Updateself()
	{
		for( int pos = 0; pos < targets.Count; pos++ ) {
			//  TODO loop in targets
			ExcuteSkill( targets[pos] );
		}
	}

	public virtual void Destroyself(float lifeTime = 0)
	{
		Invoke( "DelayDestroy", colliderData.lifeTime );
	}

	[X]
	public void DelayDestroy()
	{
		if( ( colliderData.moment & CustomCollider.ColliderMoment.OnDestroy ) != CustomCollider.ColliderMoment.None ) {
			for( int pos = 0; pos < targets.Count; pos++ ) {
				//  TODO loop in armies
				ExcuteSkill( targets[pos] );
			}

		}

		if( colliderData.isDestroyGameObject )
			Destroy( gameObject, 0.5f );
		else {
			Destroy( this );
			Destroy( m_collider );
			if( m_sector != null )
				Destroy( m_sector );
		}
		isDestroy = true;
		targets.Clear();
		CancelInvoke();
	}
}


