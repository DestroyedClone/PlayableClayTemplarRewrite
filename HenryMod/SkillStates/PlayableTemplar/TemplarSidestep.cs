using System;
using EntityStates.ClayBruiserMonster;
using EntityStates.Commando;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace EntityStates.Templar
{
	// Token: 0x0200001E RID: 30
	public class TemplarSidestep : BaseState
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x00007580 File Offset: 0x00005780
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
					Object.Instantiate<GameObject>(SpawnState.spawnEffectPrefab, transform);
				}
			}
			this.RecalculateSpeed();
			bool flag5 = base.characterMotor && base.characterDirection;
			if (flag5)
			{
				CharacterMotor characterMotor = base.characterMotor;
				characterMotor.velocity.y = characterMotor.velocity.y * 0.2f;
				base.characterMotor.velocity = this.forwardDirection * this.rollSpeed;
			}
			Vector3 b = base.characterMotor ? base.characterMotor.velocity : Vector3.zero;
			this.previousPosition = base.transform.position - b;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000077ED File Offset: 0x000059ED
		private void RecalculateSpeed()
		{
			this.rollSpeed = (4f + 0.3f * this.moveSpeedStat) * Mathf.Lerp(TemplarSidestep.initialSpeedCoefficient, TemplarSidestep.finalSpeedCoefficient, base.fixedAge / this.duration);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00007828 File Offset: 0x00005A28
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

		// Token: 0x060000AA RID: 170 RVA: 0x00007978 File Offset: 0x00005B78
		public override void OnExit()
		{
			bool flag = base.cameraTargetParams;
			if (flag)
			{
				base.cameraTargetParams.fovOverride = -1f;
			}
			base.OnExit();
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000079AE File Offset: 0x00005BAE
		public override void OnSerialize(NetworkWriter writer)
		{
			base.OnSerialize(writer);
			writer.Write(this.forwardDirection);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000079C6 File Offset: 0x00005BC6
		public override void OnDeserialize(NetworkReader reader)
		{
			base.OnDeserialize(reader);
			this.forwardDirection = reader.ReadVector3();
		}

		// Token: 0x040000D5 RID: 213
		[SerializeField]
		public float duration = 0.3f;

		// Token: 0x040000D6 RID: 214
		public static GameObject dodgeEffect;

		// Token: 0x040000D7 RID: 215
		public static float initialSpeedCoefficient = 10f;

		// Token: 0x040000D8 RID: 216
		public static float finalSpeedCoefficient = 0.25f;

		// Token: 0x040000D9 RID: 217
		private float rollSpeed;

		// Token: 0x040000DA RID: 218
		private Vector3 forwardDirection;

		// Token: 0x040000DB RID: 219
		private Animator animator;

		// Token: 0x040000DC RID: 220
		private Vector3 previousPosition;
	}
}
