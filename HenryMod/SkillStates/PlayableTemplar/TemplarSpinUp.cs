using System;
using EntityStates.ClayBruiser.Weapon;
using RoR2;
using UnityEngine;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	public class TemplarMinigunSpinUp : TemplarMinigunState
	{
		private GameObject chargeInstance;
		private float duration;

		public override void OnEnter()
		{
			base.OnEnter();
			base.characterBody.SetSpreadBloom(2f, false);
			base.characterBody.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/BanditCrosshair");
			this.duration = MinigunSpinUp.baseDuration / this.attackSpeedStat;
			Util.PlaySound(MinigunSpinUp.sound, base.gameObject);
			base.GetModelAnimator().SetBool("WeaponIsReady", true);
			bool flag = this.muzzleTransform && MinigunSpinUp.chargeEffectPrefab;
			if (flag)
			{
				this.chargeInstance = UnityEngine.Object.Instantiate<GameObject>(MinigunSpinUp.chargeEffectPrefab, this.muzzleTransform.position, this.muzzleTransform.rotation);
				this.chargeInstance.transform.parent = this.muzzleTransform;
				ScaleParticleSystemDuration component = this.chargeInstance.GetComponent<ScaleParticleSystemDuration>();
				bool flag2 = component;
				if (flag2)
				{
					component.newDuration = this.duration;
				}
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			bool flag = base.fixedAge >= this.duration && base.isAuthority;
			if (flag)
			{
				this.outer.SetNextState(new TemplarMinigunFire());
			}
		}

		public override void OnExit()
		{
			base.OnExit();
			bool flag = this.chargeInstance;
			if (flag)
			{
				EntityState.Destroy(this.chargeInstance);
			}
		}

	}
}
