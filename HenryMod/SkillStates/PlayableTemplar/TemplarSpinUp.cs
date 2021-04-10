using System;
using EntityStates.ClayBruiser.Weapon;
using RoR2;
using UnityEngine;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	// Token: 0x0200000C RID: 12
	public class TemplarMinigunSpinUp : TemplarMinigunState
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00004424 File Offset: 0x00002624
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
				this.chargeInstance = Object.Instantiate<GameObject>(MinigunSpinUp.chargeEffectPrefab, this.muzzleTransform.position, this.muzzleTransform.rotation);
				this.chargeInstance.transform.parent = this.muzzleTransform;
				ScaleParticleSystemDuration component = this.chargeInstance.GetComponent<ScaleParticleSystemDuration>();
				bool flag2 = component;
				if (flag2)
				{
					component.newDuration = this.duration;
				}
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00004518 File Offset: 0x00002718
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			bool flag = base.fixedAge >= this.duration && base.isAuthority;
			if (flag)
			{
				this.outer.SetNextState(new TemplarMinigunFire());
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000455C File Offset: 0x0000275C
		public override void OnExit()
		{
			base.OnExit();
			bool flag = this.chargeInstance;
			if (flag)
			{
				EntityState.Destroy(this.chargeInstance);
			}
		}

		// Token: 0x04000051 RID: 81
		private GameObject chargeInstance;

		// Token: 0x04000052 RID: 82
		private float duration;
	}
}
