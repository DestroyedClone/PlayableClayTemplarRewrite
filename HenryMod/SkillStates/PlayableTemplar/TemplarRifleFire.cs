using System;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.Squid.SquidWeapon;
using PlayableTemplar;
using RoR2;
using UnityEngine;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	public class TemplarRifleFire : TemplarRifleState
	{
		public static float baseDamageCoefficient = Modules.Config.rifleDamageCoefficient.Value;
		public static float baseForcePerSecond = Modules.Config.rifleForce.Value;
		public static float baseProcCoefficient = Modules.Config.rifleProcCoefficient.Value;
		public static float fireRate = Modules.Config.rifleFireRate.Value;
		public static float recoilAmplitude = 0.75f;
		private float fireTimer;
		private Transform muzzleVfxTransform;
		private float baseFireRate;
		private float baseBulletsPerSecond;
		private Run.FixedTimeStamp critEndTime;
		private Run.FixedTimeStamp lastCritCheck;

		public override void OnEnter()
		{
			base.OnEnter();
			base.GetModelAnimator().SetBool("WeaponIsReady", true);
			bool flag = this.muzzleTransform && MinigunFire.muzzleVfxPrefab;
			if (flag)
			{
				this.muzzleVfxTransform = UnityEngine.Object.Instantiate<GameObject>(MinigunFire.muzzleVfxPrefab, this.muzzleTransform).transform;
			}
			base.characterBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/StandardCrosshair");
			this.baseFireRate = 1f / MinigunFire.baseFireInterval;
			this.baseBulletsPerSecond = (float)MinigunFire.baseBulletCount * 2f * this.baseFireRate;
			this.critEndTime = Run.FixedTimeStamp.negativeInfinity;
			this.lastCritCheck = Run.FixedTimeStamp.negativeInfinity;
			Util.PlaySound(MinigunFire.startSound, base.gameObject);
			base.PlayCrossfade("Gesture, Additive", "FireMinigun", 0.2f);
		}

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
				procChainMask = default,
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
	}
}
