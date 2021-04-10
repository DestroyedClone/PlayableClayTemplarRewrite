using System;
using EntityStates.ClayBruiser.Weapon;
using PlayableTemplar;
using RoR2;
using UnityEngine;

namespace EntityStates.Templar
{
	// Token: 0x0200000A RID: 10
	public class TemplarMinigunFire : TemplarMinigunState
	{
		// Token: 0x06000034 RID: 52 RVA: 0x00003E98 File Offset: 0x00002098
		public override void OnEnter()
		{
			base.OnEnter();
			bool flag = this.muzzleTransform && MinigunFire.muzzleVfxPrefab;
			if (flag)
			{
				this.muzzleVfxTransform = Object.Instantiate<GameObject>(MinigunFire.muzzleVfxPrefab, this.muzzleTransform).transform;
			}
			this.baseFireRate = 1f / MinigunFire.baseFireInterval;
			this.baseBulletsPerSecond = (float)MinigunFire.baseBulletCount * this.baseFireRate;
			this.currentFireRate = TemplarMinigunFire.minFireRate;
			this.critEndTime = Run.FixedTimeStamp.negativeInfinity;
			this.lastCritCheck = Run.FixedTimeStamp.negativeInfinity;
			Util.PlaySound(MinigunFire.startSound, base.gameObject);
			base.PlayCrossfade("Gesture, Additive", "FireMinigun", 0.2f);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003F54 File Offset: 0x00002154
		private void UpdateCrits()
		{
			this.critStat = base.characterBody.crit;
			bool flag = this.lastCritCheck.timeSince >= 0.2f;
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

		// Token: 0x06000036 RID: 54 RVA: 0x00003FBC File Offset: 0x000021BC
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

		// Token: 0x06000037 RID: 55 RVA: 0x00004024 File Offset: 0x00002224
		private void OnFireShared()
		{
			Util.PlaySound(MinigunFire.fireSound, base.gameObject);
			bool isAuthority = base.isAuthority;
			if (isAuthority)
			{
				this.OnFireAuthority();
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00004058 File Offset: 0x00002258
		private void OnFireAuthority()
		{
			this.UpdateCrits();
			bool isCrit = !this.critEndTime.hasPassed;
			base.characterBody.AddSpreadBloom(0.25f);
			base.AddRecoil(-0.6f * TemplarMinigunFire.recoilAmplitude, -0.8f * TemplarMinigunFire.recoilAmplitude, -0.3f * TemplarMinigunFire.recoilAmplitude, 0.3f * TemplarMinigunFire.recoilAmplitude);
			this.currentFireRate = Mathf.Clamp(this.currentFireRate + TemplarMinigunFire.fireRateGrowth, TemplarMinigunFire.minFireRate, TemplarMinigunFire.maxFireRate);
			float damage = TemplarMinigunFire.baseDamageCoefficient * this.damageStat;
			float force = TemplarMinigunFire.baseForce;
			float procCoefficient = TemplarMinigunFire.baseProcCoefficient;
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
				maxDistance = MinigunFire.bulletMaxDistance,
				force = force,
				hitMask = LayerIndex.CommonMasks.bullet,
				minSpread = MinigunFire.bulletMinSpread,
				maxSpread = MinigunFire.bulletMaxSpread * 1.5f,
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
				spreadPitchScale = 1f,
				spreadYawScale = 1f,
				queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
				hitEffectPrefab = MinigunFire.bulletHitEffectPrefab,
				HitEffectNormal = MinigunFire.bulletHitEffectNormal
			}.Fire();
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00004224 File Offset: 0x00002424
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.baseFireRate = 1f / MinigunFire.baseFireInterval;
			this.baseBulletsPerSecond = (float)MinigunFire.baseBulletCount * this.baseFireRate;
			this.fireTimer -= Time.fixedDeltaTime;
			bool flag = this.fireTimer <= 0f;
			if (flag)
			{
				this.attackSpeedStat = base.characterBody.attackSpeed;
				float num = MinigunFire.baseFireInterval / this.attackSpeedStat / this.currentFireRate;
				this.fireTimer += num;
				this.OnFireShared();
			}
			bool flag2 = base.isAuthority && !base.skillButtonState.down;
			if (flag2)
			{
				this.outer.SetNextState(new TemplarMinigunSpinDown());
			}
		}

		// Token: 0x04000042 RID: 66
		public static float baseDamageCoefficient = PlayableTemplar.minigunDamageCoefficient.Value;

		// Token: 0x04000043 RID: 67
		public static float baseForce = PlayableTemplar.minigunForce.Value;

		// Token: 0x04000044 RID: 68
		public static float baseProcCoefficient = PlayableTemplar.minigunProcCoefficient.Value;

		// Token: 0x04000045 RID: 69
		public static float recoilAmplitude = 2f;

		// Token: 0x04000046 RID: 70
		public static float minFireRate = PlayableTemplar.minigunMinFireRate.Value;

		// Token: 0x04000047 RID: 71
		public static float maxFireRate = PlayableTemplar.minigunMaxFireRate.Value;

		// Token: 0x04000048 RID: 72
		public static float fireRateGrowth = PlayableTemplar.minigunFireRateGrowth.Value;

		// Token: 0x04000049 RID: 73
		private float fireTimer;

		// Token: 0x0400004A RID: 74
		private Transform muzzleVfxTransform;

		// Token: 0x0400004B RID: 75
		private float baseFireRate;

		// Token: 0x0400004C RID: 76
		private float baseBulletsPerSecond;

		// Token: 0x0400004D RID: 77
		private Run.FixedTimeStamp critEndTime;

		// Token: 0x0400004E RID: 78
		private Run.FixedTimeStamp lastCritCheck;

		// Token: 0x0400004F RID: 79
		private float currentFireRate;
	}
}
