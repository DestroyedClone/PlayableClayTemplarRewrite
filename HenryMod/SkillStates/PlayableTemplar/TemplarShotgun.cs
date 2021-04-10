using System;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.Sniper.SniperWeapon;
using PlayableTemplar;
using RoR2;
using UnityEngine;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	// Token: 0x0200001C RID: 28
	public class TemplarShotgun : BaseSkillState
	{
		// Token: 0x0600009B RID: 155 RVA: 0x00006D94 File Offset: 0x00004F94
		public override void OnEnter()
		{
			base.OnEnter();
			base.characterBody.skillLocator.primary.rechargeStopwatch = 0f;
			base.AddRecoil(-1f * TemplarShotgun.recoilAmplitude, -2f * TemplarShotgun.recoilAmplitude, -0.5f * TemplarShotgun.recoilAmplitude, 0.5f * TemplarShotgun.recoilAmplitude);
			this.maxDuration = TemplarShotgun.baseMaxDuration / this.attackSpeedStat;
			this.minDuration = TemplarShotgun.baseMinDuration / this.attackSpeedStat;
			Ray aimRay = base.GetAimRay();
			base.StartAimMode(2f, false);
			Util.PlayScaledSound(FireRifle.attackSoundString, base.gameObject, 0.8f);
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

		// Token: 0x0600009C RID: 156 RVA: 0x000070D7 File Offset: 0x000052D7
		public override void OnExit()
		{
			base.OnExit();
			base.PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
			base.GetModelAnimator().SetBool("WeaponIsReady", false);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x0000710C File Offset: 0x0000530C
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

		// Token: 0x0600009E RID: 158 RVA: 0x0000716C File Offset: 0x0000536C
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

		// Token: 0x040000BF RID: 191
		public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoBoost");

		// Token: 0x040000C0 RID: 192
		public static float damageCoefficient = PlayableTemplar.blunderbussDamageCoefficient.Value;

		// Token: 0x040000C1 RID: 193
		public static float force = 5f;

		// Token: 0x040000C2 RID: 194
		public static float bulletRadius = 1.5f;

		// Token: 0x040000C3 RID: 195
		public static float baseMaxDuration = 0.75f;

		// Token: 0x040000C4 RID: 196
		public static float baseMinDuration = 0.5f;

		// Token: 0x040000C5 RID: 197
		public static float recoilAmplitude = 5f;

		// Token: 0x040000C6 RID: 198
		public static float spreadBloomValue = PlayableTemplar.blunderbussSpread.Value;

		// Token: 0x040000C7 RID: 199
		public static uint pelletCount = (uint)PlayableTemplar.blunderbussPelletCount.Value;

		// Token: 0x040000C8 RID: 200
		public static float procCoefficient = PlayableTemplar.blunderbussProcCoefficient.Value;

		// Token: 0x040000C9 RID: 201
		private float maxDuration;

		// Token: 0x040000CA RID: 202
		private float minDuration;

		// Token: 0x040000CB RID: 203
		private bool buttonReleased;
	}
}
