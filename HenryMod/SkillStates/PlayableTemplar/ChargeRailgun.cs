using System;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.Toolbot;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PlayableTemplar.SkillStates
{
	// Token: 0x02000016 RID: 22
	public class ChargeRailgun : BaseSkillState
	{
		// Token: 0x06000076 RID: 118 RVA: 0x00005E58 File Offset: 0x00004058
		public override void OnEnter()
		{
			base.OnEnter();
			base.characterBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/SMGCrosshair");
			this.minChargeDuration = 0.75f * (ChargeSpear.baseMinChargeDuration / this.attackSpeedStat);
			this.chargeDuration = 0.75f * (ChargeSpear.baseChargeDuration / this.attackSpeedStat);
			Util.PlaySound(MinigunSpinUp.sound, base.gameObject);
			base.GetModelAnimator().SetBool("WeaponIsReady", true);
			this.muzzleTransform = base.FindModelChild(MinigunState.muzzleName);
			bool flag = this.muzzleTransform && MinigunSpinUp.chargeEffectPrefab;
			if (flag)
			{
				this.chargeInstance = UnityEngine.Object.Instantiate<GameObject>(MinigunSpinUp.chargeEffectPrefab, this.muzzleTransform.position, this.muzzleTransform.rotation);
				this.chargeInstance.transform.parent = this.muzzleTransform;
				ScaleParticleSystemDuration component = this.chargeInstance.GetComponent<ScaleParticleSystemDuration>();
				bool flag2 = component;
				if (flag2)
				{
					component.newDuration = this.minChargeDuration;
				}
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00005F68 File Offset: 0x00004168
		public override void OnExit()
		{
			base.characterBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");
			bool flag = this.chargeInstance;
			if (flag)
			{
				EntityState.Destroy(this.chargeInstance);
			}
			base.PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
			base.GetModelAnimator().SetBool("WeaponIsReady", false);
			base.OnExit();
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00005FD7 File Offset: 0x000041D7
		public override void Update()
		{
			base.Update();
			base.characterBody.SetSpreadBloom(1f - base.age / this.chargeDuration, true);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00006004 File Offset: 0x00004204
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			base.StartAimMode(2f, false);
			float num = base.fixedAge - this.chargeDuration;
			bool flag = num >= 0f;
			if (flag)
			{
				float perfectChargeWindow = ChargeSpear.perfectChargeWindow;
			}
			float charge = this.chargeDuration / base.fixedAge;
			bool isAuthority = base.isAuthority;
			if (isAuthority)
			{
				bool flag2 = !this.released && (!base.inputBank || !base.inputBank.skill1.down);
				if (flag2)
				{
					this.released = true;
				}
				bool flag3 = this.released && base.fixedAge >= this.minChargeDuration;
				if (flag3)
				{
					this.outer.SetNextState(new FireSpear
					{
						charge = charge
					});
				}
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000060E0 File Offset: 0x000042E0
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Skill;
		}

		// Token: 0x04000099 RID: 153
		private float minChargeDuration;

		// Token: 0x0400009A RID: 154
		private float chargeDuration;

		// Token: 0x0400009B RID: 155
		private GameObject chargeInstance;

		// Token: 0x0400009C RID: 156
		private bool released;

		// Token: 0x0400009D RID: 157
		private Transform muzzleTransform;
	}
}
