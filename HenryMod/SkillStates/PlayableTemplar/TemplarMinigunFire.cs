using System;
using EntityStates.ClayBruiser.Weapon;
using PlayableTemplar;
using RoR2;
using UnityEngine;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	public class TemplarMinigunFire : TemplarMinigunState
	{
		public static float baseDamageCoefficient = Modules.Config.minigunDamageCoefficient.Value;
		public static float baseForce = Modules.Config.minigunForce.Value;
		public static float baseProcCoefficient = Modules.Config.minigunProcCoefficient.Value;
		public static float recoilAmplitude = 2f;
		public static float minFireRate = Modules.Config.minigunMinFireRate.Value;
		public static float maxFireRate = Modules.Config.minigunMaxFireRate.Value;
		public static float fireRateGrowth = Modules.Config.minigunFireRateGrowth.Value;
		private float fireTimer;
		private Transform muzzleVfxTransform;
		private float baseFireRate;
		private float baseBulletsPerSecond;
		private Run.FixedTimeStamp critEndTime;
		private Run.FixedTimeStamp lastCritCheck;
		private float currentFireRate;

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

		private void OnFireShared()
		{
			Util.PlaySound(MinigunFire.fireSound, base.gameObject);
			bool isAuthority = base.isAuthority;
			if (isAuthority)
			{
				this.OnFireAuthority();
			}
		}

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
				procChainMask = default,
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
	}
}
