using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using skill;
using System;
using Scripts.Node;
using wuxingogo.Runtime;

public class SkillRenderable : XMonoBehaviour, ISkillReleaser {
    
    public enum SkillAnimationType
    {
        技能1,
        技能2,
        技能3,
        普攻1,
        普攻2,
        普攻3,
        普攻4,
        飞行技能1,
        飞行技能2,
        飞行技能3,
        飞行普攻1,
        飞行普攻2,
        飞行普攻3,
        飞行普攻4,
        未定义
    }
    
    public static SkillAnimationType GetAnimationType(string animationName)
    {
        var values = System.Enum.GetValues(typeof(SkillAnimationType));
        foreach(var v in values)
        {
            string typeName = GetAnimationName((SkillAnimationType)v);
            if (animationName.Contains(typeName))
                return (SkillAnimationType)v;
        }
        return SkillAnimationType.未定义;
    }
    
    public static string GetAnimationName(SkillAnimationType animationType)
    {
        switch (animationType)
        {
            case SkillAnimationType.技能1:
                return "GroundSkill1";
            case SkillAnimationType.技能2:
                return "GroundSkill2";
            case SkillAnimationType.技能3:
                return "GroundSkill3";
            case SkillAnimationType.普攻1:
                return "GroundAttack1";
            case SkillAnimationType.普攻2:
                return "GroundAttack2";
            case SkillAnimationType.普攻3:
                return "GroundAttack3";
            case SkillAnimationType.普攻4:
                return "GroundAttack4";
            case SkillAnimationType.飞行技能1:
                return "FlySkill1";
            case SkillAnimationType.飞行技能2:
                return "FlySkill2";
            case SkillAnimationType.飞行技能3:
                return "FlySkill3";
            case SkillAnimationType.飞行普攻1:
                return "FlyAttack1";
            case SkillAnimationType.飞行普攻2:
                return "FlyAttack2";
            case SkillAnimationType.飞行普攻3:
                return "FlyAttack3";
            case SkillAnimationType.飞行普攻4:
                return "FlyAttack4";

        }
        return "";
    }

	public SkillTree skillTree = null;

    public void BindSkill(Animator animator)
    {
		RuntimeAnimatorController rac = animator.runtimeAnimatorController;
		AnimationClip[] clips = rac.animationClips;
		int clipsCount = clips.Length;


		string needClipName = "";
		var animationType = (SkillAnimationType)skillTree.animationType;
		Debug.Log(animationType.ToString());
		needClipName = GetAnimationName(animationType);

		for( int i = 0; i < clipsCount; ++i ) {
			AnimationClip c = clips[i];
			string clipName = c.name;
			if( clipName.Contains( needClipName ) ) {
				c.events = null;

				AnimationEvent ae = new AnimationEvent();

				for( int pos = 0; pos < skillTree.castStruct.Count; pos++ ) {
					ae.time = Mathf.Min( c.length, skillTree.castStruct[pos].startTime );
					ae.functionName = "PlaySkill";
					ae.intParameter = pos;
					c.AddEvent( ae );
				}
                
			}
		}
    }
    [SerializeField]
    Warlock warlock = null;
    [X]
    public void StartSkill(int index)
    {

//		for( int pos = 0; pos < skillTree.castStruct.Count; pos++ ) {
//			//  TODO loop in Length
//			skillTree.castStruct[pos].Init(gameObject, this, GetComponent<EffectBindPos>());
//		}
		skillTree.castStruct[index].Init(gameObject, this, warlock.GetBinder());
    }

//    public void OnStartSkill(int skillID)
//    {
//        caster.owner.DataSystem.OnStartSkill(skillID);
//    }
//    public void OnSkillHit(int skillID,Army target)
//    {
//        caster.owner.DataSystem.OnSkillHit(skillID, target);
//    }
//    public void ChangeToFlyState()
//    {
//        if (skillInstance != null)
//        {
//            skillInstance.ChangeToFlyState();
//        }
//    }
//    [HideInInspector, SerializeField]
//    public string FilePath = "";
//    // Update is called once per frame
//    void OnDestroy (){
//		if( OnDestroyEvent != null )
//			OnDestroyEvent();
//    }

	#region ISkillReleaser implementation

	public MonoBehaviour GetBehaviour()
	{
		return this;
	}

	public ISkillCanBeTarget GetSelfTarget()
	{
		return warlock;
	}

	public bool isFriend(ISkillCanBeTarget other)
	{
		return GetSelfTarget() == other;
	}

	#endregion

	void Start()
	{
		
	}
}
