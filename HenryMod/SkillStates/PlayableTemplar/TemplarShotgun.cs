using System;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.Sniper.SniperWeapon;
using PlayableTemplar;
using RoR2;
using UnityEngine;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	public class TemplarShotgun : BaseSkillState
	{
		public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoBoost");
		public static float damageCoefficient = Modules.Config.blunderbussDamageCoefficient.Value;
		public static float force = 5f;
		public static float bulletRadius = 1.5f;
		public static float baseMaxDuration = 0.75f;
		public static float baseMinDuration = 0.5f;
		public static float recoilAmplitude = 5f;
		public static float spreadBloomValue = Modules.Config.blunderbussSpread.Value;
		public static uint pelletCount = (uint)Modules.Config.blunderbussPelletCount.Value;
		public static float procCoefficient = Modules.Config.blunderbussProcCoefficient.Value;
		private float maxDuration;
		private float minDuration;
		private bool buttonReleased;

		public override void OnEnter()
		{
			base.OnEnter();
			base.characterBody.skillLocator.primary.rechargeStopwatch = 0f;
			base.AddRecoil(-1f * TemplarShotgun.recoilAmplitude, -2f * TemplarShotgun.recoilAmplitude, -0.5f * TemplarShotgun.recoilAmplitude, 0.5f * TemplarShotgun.recoilAmplitude);
			this.maxDuration = TemplarShotgun.baseMaxDuration / this.attackSpeedStat;
			this.minDuration = TemplarShotgun.baseMinDuration / this.attackSpeedStat;
			Ray aimRay = base.GetAimRay();
			base.StartAimMode(2f, false);
			Util.PlayAttackSpeedSound(FireRifle.attackSoundString, base.gameObject, 0.8f);
			base.GetModelAnimator().SetBool("WeaponIsReady", true);
			base.PlayCrossfade("Gesture, Additive", "FireMinigun", 0.2f);
			string muzzleName = MinigunState.muzzleName;
			bool flag = MinigunFire.muzzleVfxPrefab;
			bool flag2 = flag;
			if (flag2)
			{
				EffectManager.SimpleMuzzleFlash(MinigunFire.muzzleVfxPrefab, base.gameObject, muzzleName, false);
			}
			bool isAuthority = base.isAuthority;
			bool flag3 = isAuthority;
			if (flag3)
			{
				new BulletAttack
				{
					owner = base.gameObject,
					weapon = base.gameObject,
					origin = aimRay.origin,
					aimVector = aimRay.direction,
					minSpread = 0f,
					maxSpread = 0f,
					bulletCount = 1U,
					procCoefficient = TemplarShotgun.procCoefficient,
					damageType = DamageType.Generic,
					damage = TemplarShotgun.damageCoefficient * this.damageStat,
					force = TemplarShotgun.force,
					falloffModel = BulletAttack.FalloffModel.DefaultBullet,
					tracerEffectPrefab = TemplarShotgun.tracerEffectPrefab,
					muzzleName = muzzleName,
					hitEffectPrefab = MinigunFire.bulletHitEffectPrefab,
					isCrit = Util.CheckRoll(this.critStat, base.characterBody.master),
					HitEffectNormal = false,
					radius = TemplarShotgun.bulletRadius,
					smartCollision = true,
					stopperMask = LayerIndex.world.mask
				}.Fire();
				bool flag4 = TemplarShotgun.pelletCount > 1U;
				bool flag5 = flag4;
				if (flag5)
				{
					new BulletAttack
					{
						owner = base.gameObject,
						weapon = base.gameObject,
						origin = aimRay.origin,
						aimVector = aimRay.direction,
						minSpread = TemplarShotgun.spreadBloomValue / (TemplarShotgun.pelletCount - 1f),
						maxSpread = TemplarShotgun.spreadBloomValue,
						bulletCount = TemplarShotgun.pelletCount - 1U,
						procCoefficient = TemplarShotgun.procCoefficient,
						damageType = DamageType.Generic,
						damage = TemplarShotgun.damageCoefficient * this.damageStat,
						force = TemplarShotgun.force,
						falloffModel = BulletAttack.FalloffModel.DefaultBullet,
						tracerEffectPrefab = TemplarShotgun.tracerEffectPrefab,
						muzzleName = muzzleName,
						hitEffectPrefab = MinigunFire.bulletHitEffectPrefab,
						isCrit = Util.CheckRoll(this.critStat, base.characterBody.master),
						HitEffectNormal = false,
						radius = TemplarShotgun.bulletRadius,
						smartCollision = true,
						stopperMask = LayerIndex.world.mask
					}.Fire();
				}
				base.characterBody.AddSpreadBloom(TemplarShotgun.spreadBloomValue);
			}
		}
		
		public override void OnExit()
		{
			base.OnExit();
			base.PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
			base.GetModelAnimator().SetBool("WeaponIsReady", false);
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.buttonReleased |= !base.inputBank.skill1.down;
			bool flag = base.fixedAge >= this.maxDuration && base.isAuthority;
			bool flag2 = flag;
			if (flag2)
			{
				this.outer.SetNextStateToMain();
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			bool flag = this.buttonReleased && base.fixedAge >= this.minDuration;
			bool flag2 = flag;
			InterruptPriority result;
			if (flag2)
			{
				result = InterruptPriority.Any;
			}
			else
			{
				result = InterruptPriority.Skill;
			}
			return result;
		}
	}
}
