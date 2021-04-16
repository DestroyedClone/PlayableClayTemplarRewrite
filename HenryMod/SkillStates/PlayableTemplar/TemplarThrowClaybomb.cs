using System;
using EntityStates.ClayBoss;
using EntityStates.ClayBoss.ClayBossWeapon;
using EntityStates.ClayBruiser.Weapon;
using PlayableTemplar;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	public class TemplarThrowClaybomb : BaseSkillState
	{
		public float damageCoefficient = 1f;
		public float baseDuration = 0.4f;
		public float throwForce = 85f;
		private float duration;
		private float fireDuration;
		private bool hasFired;
		private Animator animator;
		private string muzzleName;

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

		public override void OnExit()
		{
			base.OnExit();
		}

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
					ProjectileManager.instance.FireProjectile(Modules.Projectiles.templarGrenade, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, this.damageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, this.throwForce);
				}
				this.animator.SetBool("WeaponIsReady", false);
			}
		}

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

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}

	}
}
