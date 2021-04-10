using System;
using EntityStates.ClayBoss;
using EntityStates.ClayBoss.ClayBossWeapon;
using EntityStates.ClayBruiser.Weapon;
using PlayableTemplar;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace PlayableTemplar.SkillStates
{
	// Token: 0x02000009 RID: 9
	public class TemplarThrowClaybomb : BaseSkillState
	{
		// Token: 0x0600002E RID: 46 RVA: 0x00003CAC File Offset: 0x00001EAC
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = this.baseDuration / this.attackSpeedStat;
			this.fireDuration = 0.2f * this.duration;
			base.characterBody.SetAimTimer(2f);
			this.animator = base.GetModelAnimator();
			this.muzzleName = MinigunState.muzzleName;
			this.animator.SetBool("WeaponIsReady", true);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003D20 File Offset: 0x00001F20
		public override void OnExit()
		{
			base.OnExit();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003D2C File Offset: 0x00001F2C
		private void ThrowBomb()
		{
			bool flag = !this.hasFired;
			if (flag)
			{
				this.hasFired = true;
				Util.PlaySound(FireBombardment.shootSoundString, base.gameObject);
				Ray aimRay = base.GetAimRay();
				EffectManager.SimpleMuzzleFlash(FireTarball.effectPrefab, base.gameObject, this.muzzleName, false);
				bool isAuthority = base.isAuthority;
				if (isAuthority)
				{
					ProjectileManager.instance.FireProjectile(PlayableTemplar.templarGrenade, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, this.damageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, this.throwForce);
				}
				this.animator.SetBool("WeaponIsReady", false);
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003DFC File Offset: 0x00001FFC
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			bool flag = base.fixedAge >= this.fireDuration;
			if (flag)
			{
				this.ThrowBomb();
			}
			bool flag2 = base.fixedAge >= this.duration && base.isAuthority;
			if (flag2)
			{
				this.outer.SetNextStateToMain();
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003E58 File Offset: 0x00002058
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}

		// Token: 0x0400003A RID: 58
		public float damageCoefficient = 1f;

		// Token: 0x0400003B RID: 59
		public float baseDuration = 0.4f;

		// Token: 0x0400003C RID: 60
		public float throwForce = 85f;

		// Token: 0x0400003D RID: 61
		private float duration;

		// Token: 0x0400003E RID: 62
		private float fireDuration;

		// Token: 0x0400003F RID: 63
		private bool hasFired;

		// Token: 0x04000040 RID: 64
		private Animator animator;

		// Token: 0x04000041 RID: 65
		private string muzzleName;
	}
}
