using System;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.Squid.SquidWeapon;
using PlayableTemplar;
using RoR2;
using UnityEngine;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	// Token: 0x0200000E RID: 14
	public class TemplarRifleFire : TemplarRifleState
	{
		// Token: 0x06000049 RID: 73 RVA: 0x00004840 File Offset: 0x00002A40
		public override void OnEnter()
		{
			base.OnEnter();
			base.GetModelAnimator().SetBool("WeaponIsReady", true);
			bool flag = this.muzzleTransform && MinigunFire.muzzleVfxPrefab;
			if (flag)
			{
				this.muzzleVfxTransform = Object.Instantiate<GameObject>(MinigunFire.muzzleVfxPrefab, this.muzzleTransform).transform;
			}
			base.characterBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/StandardCrosshair");
			this.baseFireRate = 1f / MinigunFire.baseFireInterval;
			this.baseBulletsPerSecond = (float)MinigunFire.baseBulletCount * 2f * this.baseFireRate;
			this.critEndTime = Run.FixedTimeStamp.negativeInfinity;
			this.lastCritCheck = Run.FixedTimeStamp.negativeInfinity;
			Util.PlaySound(MinigunFire.startSound, base.gameObject);
			base.PlayCrossfade("Gesture, Additive", "FireMinigun", 0.2f);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00004920 File Offset: 0x00002B20
		private void UpdateCrits()
		{
			this.critStat = base.characterBody.crit;
			bool flag = this.lastCritCheck.timeSince >= 0.1f;
			if (flag)
			{
				this.lastCritCheck = Run.FixedTimeStamp.now;
				bool flag2 = base.RollCrit();
				if (flag2)
				{
					this.critEndTime = Run.FixedTimeStamp.now + 0.4f;
				}
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00004988 File Offset: 0x00002B88
		public override void OnExit()
		{
			Util.PlaySound(MinigunFire.endSound, base.gameObject);
			bool flag = this.muzzleVfxTransform;
			if (flag)
			{
				EntityState.Destroy(this.muzzleVfxTransform.gameObject);
				this.muzzleVfxTransform = null;
			}
			base.PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
			base.OnExit();
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000049F0 File Offset: 0x00002BF0
		private void OnFireShared()
		{
			Util.PlaySound(MinigunFire.fireSound, base.gameObject);
			bool isAuthority = base.isAuthority;
			if (isAuthority)
			{
				this.OnFireAuthority();
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00004A24 File Offset: 0x00002C24
		private void OnFireAuthority()
		{
			this.UpdateCrits();
			bool isCrit = !this.critEndTime.hasPassed;
			base.AddRecoil(-0.5f * TemplarRifleFire.recoilAmplitude, -0.5f * TemplarRifleFire.recoilAmplitude, -0.5f * TemplarRifleFire.recoilAmplitude, 0.5f * TemplarRifleFire.recoilAmplitude);
			float damage = TemplarRifleFire.baseDamageCoefficient * this.damageStat;
			float force = TemplarRifleFire.baseForcePerSecond / this.baseBulletsPerSecond;
			float procCoefficient = TemplarRifleFire.baseProcCoefficient;
			Ray aimRay = base.GetAimRay();
			new BulletAttack
			{
				bulletCount = (uint)MinigunFire.baseBulletCount,
				aimVector = aimRay.direction,
				origin = aimRay.origin,
				damage = damage,
				damageColorIndex = DamageColorIndex.Default,
				damageType = DamageType.Generic,
				falloffModel = BulletAttack.FalloffModel.DefaultBullet,
				maxDistance = MinigunFire.bulletMaxDistance * 0.75f,
				force = force,
				hitMask = LayerIndex.CommonMasks.bullet,
				minSpread = MinigunFire.bulletMinSpread,
				maxSpread = MinigunFire.bulletMaxSpread,
				isCrit = isCrit,
				owner = base.gameObject,
				muzzleName = MinigunState.muzzleName,
				smartCollision = false,
				procChainMask = default(ProcChainMask),
				procCoefficient = procCoefficient,
				radius = 0f,
				sniper = false,
				stopperMask = LayerIndex.CommonMasks.bullet,
				weapon = null,
				tracerEffectPrefab = MinigunFire.bulletTracerEffectPrefab,
				spreadPitchScale = 0.35f,
				spreadYawScale = 0.35f,
				queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
				hitEffectPrefab = FireSpine.hitEffectPrefab,
				HitEffectNormal = FireSpine.hitEffectPrefab
			}.Fire();
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00004BCC File Offset: 0x00002DCC
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.baseFireRate = 1f / (MinigunFire.baseFireInterval * 1f);
			this.baseBulletsPerSecond = (float)MinigunFire.baseBulletCount * 1f * this.baseFireRate * 1f;
			this.fireTimer -= Time.fixedDeltaTime;
			bool flag = this.fireTimer <= 0f;
			if (flag)
			{
				this.attackSpeedStat = base.characterBody.attackSpeed;
				float num = TemplarRifleFire.fireRate * (MinigunFire.baseFireInterval / this.attackSpeedStat);
				this.fireTimer += num;
				this.OnFireShared();
			}
			bool flag2 = base.isAuthority && !base.skillButtonState.down;
			if (flag2)
			{
				this.outer.SetNextState(new TemplarRifleSpinDown());
			}
		}

		// Token: 0x04000059 RID: 89
		public static float baseDamageCoefficient = PlayableTemplar.rifleDamageCoefficient.Value;

		// Token: 0x0400005A RID: 90
		public static float baseForcePerSecond = PlayableTemplar.rifleForce.Value;

		// Token: 0x0400005B RID: 91
		public static float baseProcCoefficient = PlayableTemplar.rifleProcCoefficient.Value;

		// Token: 0x0400005C RID: 92
		public static float fireRate = PlayableTemplar.rifleFireRate.Value;

		// Token: 0x0400005D RID: 93
		public static float recoilAmplitude = 0.75f;

		// Token: 0x0400005E RID: 94
		private float fireTimer;

		// Token: 0x0400005F RID: 95
		private Transform muzzleVfxTransform;

		// Token: 0x04000060 RID: 96
		private float baseFireRate;

		// Token: 0x04000061 RID: 97
		private float baseBulletsPerSecond;

		// Token: 0x04000062 RID: 98
		private Run.FixedTimeStamp critEndTime;

		// Token: 0x04000063 RID: 99
		private Run.FixedTimeStamp lastCritCheck;
	}
}
