using System;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.Toolbot;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PlayableTemplar.SkillStates
{
	public class ChargeRailgun : BaseSkillState
	{
		private float minChargeDuration;
		private float chargeDuration;
		private GameObject chargeInstance;
		private bool released;
		private Transform muzzleTransform;

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
				ScaleParticleSystemDuration scaleParticleSystemDuration = this.chargeInstance.GetComponent<ScaleParticleSystemDuration>();
				if (scaleParticleSystemDuration)
				{
					scaleParticleSystemDuration.newDuration = this.minChargeDuration;
				}
			}
		}

		public override void OnExit()
		{
			base.characterBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");
			if (this.chargeInstance)
			{
				EntityState.Destroy(this.chargeInstance);
			}
			base.PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
			base.GetModelAnimator().SetBool("WeaponIsReady", false);
			base.OnExit();
		}

		public override void Update()
		{
			base.Update();
			base.characterBody.SetSpreadBloom(1f - base.age / this.chargeDuration, true);
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			base.StartAimMode(2f, false);
			float num = base.fixedAge - this.chargeDuration;
			if (num >= 0f)
			{
				float perfectChargeWindow = ChargeSpear.perfectChargeWindow;
			}
			float charge = this.chargeDuration / base.fixedAge;
			if (base.isAuthority)
			{
				if (!this.released && (!base.inputBank || !base.inputBank.skill1.down))
				{
					this.released = true;
				}
				if (this.released && base.fixedAge >= this.minChargeDuration)
				{
					this.outer.SetNextState(new FireSpear
					{
						charge = charge
					});
				}
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Skill;
		}
	}
}
