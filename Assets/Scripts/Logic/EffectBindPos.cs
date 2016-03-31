using UnityEngine;
using System.Collections;
using skill;
using System.Collections.Generic;

public class EffectBindPos : MonoBehaviour, ITargetBinder{
	#region ITargetBinder implementation
	[SerializeField]
	Transform _self = null;
	[SerializeField]
	Transform hip = null;

	public Transform GetBinder(int bindType)
	{
		switch( bindType ) {
			case -1:
				return _self;
			break;
			case 0:
				return hip;
			break;
			default:
			break;
		}
		return   null;
	}

	#endregion

	public void PlaySkill(int animation)
	{
		var stateInfo = animator.GetCurrentAnimatorClipInfo( 0 )[0].clip.name;
		var animationType = SkillRenderable.GetAnimationType( stateInfo );
		if(skillRenderableDict.ContainsKey(animationType))
			skillRenderableDict[animationType].StartSkill(animation);
	}

	public void AddSkill(SkillRenderable skillRenderable)
	{
		skillRenderableDict.Add((SkillRenderable.SkillAnimationType)skillRenderable.skillTree.animationType, skillRenderable);
	}

	private Dictionary<SkillRenderable.SkillAnimationType, SkillRenderable> skillRenderableDict = new Dictionary<SkillRenderable.SkillAnimationType, SkillRenderable>();
	[SerializeField]
	Animator animator;

	void Start()
	{
		
	}
	
}
