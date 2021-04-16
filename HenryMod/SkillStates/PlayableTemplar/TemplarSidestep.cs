using System;
using EntityStates.ClayBruiserMonster;
using EntityStates.Commando;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	public class TemplarSidestep : BaseSkillState
	{
		[SerializeField]
		public float duration = 0.3f;
		public static GameObject dodgeEffect;
		public static float initialSpeedCoefficient = 10f;
		public static float finalSpeedCoefficient = 0.25f;
		private float rollSpeed;
		private Vector3 forwardDirection;
		private Animator animator;
		private Vector3 previousPosition;

		public override void OnEnter()
		{
			base.OnEnter();
			Util.PlaySound(DodgeState.dodgeSoundString, base.gameObject);
			this.animator = base.GetModelAnimator();
			ChildLocator component = this.animator.GetComponent<ChildLocator>();
			bool flag = base.isAuthority && base.inputBank && base.characterDirection;
			if (flag)
			{
				this.forwardDirection = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
			}
			Vector3 rhs = base.characterDirection ? base.characterDirection.forward : this.forwardDirection;
			Vector3 rhs2 = Vector3.Cross(Vector3.up, rhs);
			float num = Vector3.Dot(this.forwardDirection, rhs);
			float num2 = Vector3.Dot(this.forwardDirection, rhs2);
			this.animator.SetFloat("forwardSpeed", num, 0.1f, Time.fixedDeltaTime);
			this.animator.SetFloat("rightSpeed", num2, 0.1f, Time.fixedDeltaTime);
			bool flag2 = Mathf.Abs(num) > Mathf.Abs(num2);
			if (flag2)
			{
				base.PlayAnimation("Body", (num > 0f) ? "DodgeForward" : "DodgeBackward", "Dodge.playbackRate", this.duration);
			}
			else
			{
				base.PlayAnimation("Body", (num2 > 0f) ? "DodgeRight" : "DodgeLeft", "Dodge.playbackRate", this.duration);
			}
			bool flag3 = SpawnState.spawnEffectPrefab;
			if (flag3)
			{
				Transform transform = component.FindChild("chest");
				bool flag4 = transform;
				if (flag4)
				{
					UnityEngine.Object.Instantiate<GameObject>(SpawnState.spawnEffectPrefab, transform);
				}
			}
			this.RecalculateSpeed();
			bool flag5 = base.characterMotor && base.characterDirection;
			if (flag5)
			{
				CharacterMotor characterMotor = base.characterMotor;
				characterMotor.velocity.y *= 0.2f;
				base.characterMotor.velocity = this.forwardDirection * this.rollSpeed;
			}
			Vector3 b = base.characterMotor ? base.characterMotor.velocity : Vector3.zero;
			this.previousPosition = base.transform.position - b;
		}

		private void RecalculateSpeed()
		{
			this.rollSpeed = (4f + 0.3f * this.moveSpeedStat) * Mathf.Lerp(TemplarSidestep.initialSpeedCoefficient, TemplarSidestep.finalSpeedCoefficient, base.fixedAge / this.duration);
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.RecalculateSpeed();
			bool flag = base.cameraTargetParams;
			if (flag)
			{
				base.cameraTargetParams.fovOverride = Mathf.Lerp(DodgeState.dodgeFOV, 60f, base.fixedAge / this.duration);
			}
			Vector3 normalized = (base.transform.position - this.previousPosition).normalized;
			bool flag2 = base.characterMotor && base.characterDirection && normalized != Vector3.zero;
			if (flag2)
			{
				Vector3 vector = normalized * this.rollSpeed;
				float y = vector.y;
				vector.y = 0f;
				float d = Mathf.Max(Vector3.Dot(vector, this.forwardDirection), 0f);
				vector = this.forwardDirection * d;
				vector.y += Mathf.Max(y, 0f);
				base.characterMotor.velocity = vector;
			}
			this.previousPosition = base.transform.position;
			bool flag3 = base.fixedAge >= this.duration && base.isAuthority;
			if (flag3)
			{
				this.outer.SetNextStateToMain();
			}
		}

		public override void OnExit()
		{
			bool flag = base.cameraTargetParams;
			if (flag)
			{
				base.cameraTargetParams.fovOverride = -1f;
			}
			base.OnExit();
		}

		public override void OnSerialize(NetworkWriter writer)
		{
			base.OnSerialize(writer);
			writer.Write(this.forwardDirection);
		}

		public override void OnDeserialize(NetworkReader reader)
		{
			base.OnDeserialize(reader);
			this.forwardDirection = reader.ReadVector3();
		}
	}
}
