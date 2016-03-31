//
//  Warlock.cs
//
//  Author:
//       ${wuxingogo} <52111314ly@gmail.com>
//
//  Copyright (c) 2016 ly-user
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using UnityEngine;
using skill;
using wuxingogo.Runtime;
using UnityEngine.Networking;


namespace Scripts.Node
	{

	public class Warlock : Node, IMoveable, ISkillCanBeTarget
	{
		#region IMoveable implementation

		public Node Owner()
		{
			return this;
		}

		public void OnStartMovement()
		{
			SetAction(AnimaType.Walk);
			SetSpeed(moveSpeed);
		}

		public void OnEndMovement()
		{
			SetAction(AnimaType.Idle);
			SetSpeed(0.0f);
		}


		#region ISkillCanBeTarget implementation

		[SerializeField]
		EffectBindPos bindPos = null;

		public ITargetBinder GetBinder()
		{
			return bindPos;
		}

		public void OnHitSkill(int skillID)
		{

		}

		public void OnHitSkill(ISkillReleaser releaser)
		{
			
		}

		public MonoBehaviour GetRuntime()
		{
			return this;
		}

		public bool canMoveable
		{
			get ;
			set;
		}

		public void PlaySkillAction(int action)
		{
			SetAction((AnimaType)action);
		}

		#endregion

		#endregion

		public enum AnimaType
		{

			Idle = 0,
			Walk = 1,
			Jump = 2,
			Dazuo = 3,
			Shuangxiu = 4,
			Attack = 6,
			Dead = 10,
			//Dance = 11,

			Skill1 = 7,
			Skill2 = 8,
			BeAttacked = 9,
			Skill3 = 13,
		}

		[SerializeField]
		Animator animator = null;
		[SerializeField] SkillRenderable[] skillCollection = null;

		void SetAction(AnimaType type)
		{
			animator.SetInteger("Action", (int)type);
		}

		void Start()
		{
			for (int pos = 0; pos < skillCollection.Length; pos++)
			{
				//  TODO loop in skillCollection.Length
				skillCollection[pos].BindSkill(animator);
				bindPos.AddSkill(skillCollection[pos]);
			}
		
			headDir = realUnit.forward;
		}

		void Update()
		{
			realUnit.forward = Vector3.Lerp(realUnit.forward, headDir, twistingSpeed);

			var targetPos = transform.position + realUnit.forward * Mathf.Lerp(updatingSpeed, targetSpeed, acceleration);
			transform.position = targetPos;

		}

		[SerializeField][SyncVar]
		float twistingSpeed = 0.05f;
		[SerializeField][Disable][SyncVar]
		Vector3 headDir;

		public void TwistHead(Vector3 direction)
		{
			headDir = direction;
		}

		float targetSpeed = 0f;
		float updatingSpeed = 0.0f;
		[SerializeField][Disable(true)]
		float acceleration = 0.4f;
		float moveSpeed = 0.2f;

		public void SetSpeed(float targetSpeed)
		{
			this.targetSpeed = targetSpeed;
		}
	}
	}

