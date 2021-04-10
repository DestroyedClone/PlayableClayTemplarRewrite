using System;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.GolemMonster;
using RoR2;
using UnityEngine;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	// Token: 0x02000015 RID: 21
	public class TemplarFireBeam : BaseSkillState
	{
		// Token: 0x06000070 RID: 112 RVA: 0x00005A8C File Offset: 0x00003C8C
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = TemplarFireBeam.baseDuration / this.attackSpeedStat;
			this.modifiedAimRay = base.GetAimRay();
			this.modifiedAimRay.direction = this.laserDirection;
			this.animator = base.GetModelAnimator();
			Transform modelTransform = base.GetModelTransform();
			Ray aimRay = base.GetAimRay();
			base.StartAimMode(2f, false);
			float num = this.recoilAmplitude / this.attackSpeedStat;
			base.AddRecoil(-1f * num, -1.5f * num, -0.25f * num, 0.25f * num);
			base.characterBody.AddSpreadBloom(this.bloom);
			Util.PlayScaledSound(FireLaser.attackSoundString, base.gameObject, 2f);
			string muzzleName = MinigunState.muzzleName;
			bool flag = FireLaser.effectPrefab;
			if (flag)
			{
				EffectManager.SimpleMuzzleFlash(FireLaser.effectPrefab, base.gameObject, muzzleName, false);
			}
			bool isAuthority = base.isAuthority;
			if (isAuthority)
			{
				float num2 = 1000f;
				Vector3 vector = this.modifiedAimRay.origin + this.modifiedAimRay.direction * num2;
				RaycastHit raycastHit;
				bool flag2 = Physics.Raycast(this.modifiedAimRay, out raycastHit, num2, LayerIndex.world.mask | LayerIndex.defaultLayer.mask | LayerIndex.entityPrecise.mask);
				if (flag2)
				{
					vector = raycastHit.point;
				}
				new BulletAttack
				{
					owner = base.gameObject,
					weapon = base.gameObject,
					origin = aimRay.origin,
					aimVector = aimRay.direction,
					minSpread = 0f,
					maxSpread = 0f,
					bulletCount = (uint)TemplarFireBeam.bulletCount,
					procCoefficient = TemplarFireBeam.procCoefficient,
					damageType = DamageType.BypassOneShotProtection,
					damage = TemplarFireBeam.damageCoefficient * this.damageStat,
					force = TemplarFireBeam.force,
					falloffModel = BulletAttack.FalloffModel.None,
					tracerEffectPrefab = FireLaser.tracerEffectPrefab,
					muzzleName = MinigunState.muzzleName,
					hitEffectPrefab = FireLaser.hitEffectPrefab,
					isCrit = Util.CheckRoll(this.critStat, base.characterBody.master),
					HitEffectNormal = false,
					radius = TemplarFireBeam.radius,
					smartCollision = true,
					stopperMask = LayerIndex.world.mask
				}.Fire();
			}
			base.PlayCrossfade("Gesture, Additive", "FireMinigun", 0.75f * this.duration);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00005D34 File Offset: 0x00003F34
		public override void OnExit()
		{
			base.OnExit();
			base.characterBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");
			base.PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
			this.animator.SetBool("WeaponIsReady", false);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00005D88 File Offset: 0x00003F88
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			bool flag = base.fixedAge >= this.duration && base.isAuthority;
			if (flag)
			{
				this.outer.SetNextStateToMain();
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00005DC8 File Offset: 0x00003FC8
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}

		// Token: 0x0400008B RID: 139
		public static float damageCoefficient = 8f;

		// Token: 0x0400008C RID: 140
		public static float procCoefficient = 1f;

		// Token: 0x0400008D RID: 141
		public static float force = 500f;

		// Token: 0x0400008E RID: 142
		public static float minSpread = 0f;

		// Token: 0x0400008F RID: 143
		public static float maxSpread = 0f;

		// Token: 0x04000090 RID: 144
		public static int bulletCount = 1;

		// Token: 0x04000091 RID: 145
		public static float baseDuration = 0.5f;

		// Token: 0x04000092 RID: 146
		public static float radius = 1.25f;

		// Token: 0x04000093 RID: 147
		public float recoilAmplitude = 10f;

		// Token: 0x04000094 RID: 148
		public float bloom = 6f;

		// Token: 0x04000095 RID: 149
		public Vector3 laserDirection;

		// Token: 0x04000096 RID: 150
		private float duration;

		// Token: 0x04000097 RID: 151
		private Ray modifiedAimRay;

		// Token: 0x04000098 RID: 152
		private Animator animator;
	}
}
