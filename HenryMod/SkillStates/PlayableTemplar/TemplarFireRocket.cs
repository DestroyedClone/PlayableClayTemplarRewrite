using System;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.LemurianBruiserMonster;
using PlayableTemplar;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace EntityStates.Templar
{
	// Token: 0x0200001B RID: 27
	public class TemplarFireRocket : BaseState
	{
		// Token: 0x06000095 RID: 149 RVA: 0x000069BC File Offset: 0x00004BBC
		public override void OnEnter()
		{
			base.OnEnter();
			base.StartAimMode(2f, false);
			this.jelly = false;
			TemplarFireRocket.rocketPrefab = PlayableTemplar.templarRocket;
			TemplarFireRocket.effectPrefab = FireMegaFireball.muzzleflashEffectPrefab;
			bool value = PlayableTemplar.jellyfishEvent.Value;
			if (value)
			{
				bool flag = Random.Range(0, 100) <= 25;
				if (flag)
				{
					TemplarFireRocket.rocketPrefab = Resources.Load<GameObject>("Prefabs/CharacterBodies/JellyfishBody");
					this.jelly = true;
				}
				else
				{
					TemplarFireRocket.rocketPrefab = PlayableTemplar.templarRocket;
				}
			}
			base.AddRecoil(-1f * TemplarFireRocket.recoilAmplitude, -2f * TemplarFireRocket.recoilAmplitude, -0.5f * TemplarFireRocket.recoilAmplitude, 0.5f * TemplarFireRocket.recoilAmplitude);
			this.duration = TemplarFireRocket.baseDuration / this.attackSpeedStat;
			this.fireDuration = TemplarFireRocket.baseFireDuration / this.attackSpeedStat;
			Util.PlaySound(FireMegaFireball.attackString, base.gameObject);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00006AAC File Offset: 0x00004CAC
		public override void OnExit()
		{
			base.OnExit();
			base.PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
			base.GetModelAnimator().SetBool("WeaponIsReady", false);
			base.characterBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00006B00 File Offset: 0x00004D00
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			string muzzleName = MinigunState.muzzleName;
			bool isAuthority = base.isAuthority;
			if (isAuthority)
			{
				int num = Mathf.FloorToInt(base.fixedAge / this.fireDuration * (float)TemplarFireRocket.projectileCount);
				bool flag = this.projectilesFired <= num && this.projectilesFired < TemplarFireRocket.projectileCount;
				if (flag)
				{
					EffectManager.SimpleMuzzleFlash(TemplarFireRocket.effectPrefab, base.gameObject, muzzleName, false);
					Ray aimRay = base.GetAimRay();
					float speedOverride = FireMegaFireball.projectileSpeed * 2f;
					float bonusYaw = (float)Mathf.FloorToInt((float)this.projectilesFired - (float)(TemplarFireRocket.projectileCount - 1) / 2f) / (float)(TemplarFireRocket.projectileCount - 1) * TemplarFireRocket.totalYawSpread;
					bonusYaw = 0f;
					Vector3 vector = Util.ApplySpread(aimRay.direction, 0f, 0f, 1f, 1f, bonusYaw, 0f);
					bool flag2 = this.jelly;
					if (flag2)
					{
						Vector3 position = aimRay.origin;
						Transform modelTransform = base.GetModelTransform();
						bool flag3 = modelTransform;
						if (flag3)
						{
							ChildLocator component = modelTransform.GetComponent<ChildLocator>();
							bool flag4 = component;
							if (flag4)
							{
								bool flag5 = component.FindChild(MinigunState.muzzleName);
								if (flag5)
								{
									position = component.FindChild(MinigunState.muzzleName).position + vector;
								}
							}
						}
						base.characterBody.SendConstructTurret(base.characterBody, position, Util.QuaternionSafeLookRotation(vector), MasterCatalog.FindAiMasterIndexForBody(TemplarFireRocket.rocketPrefab.GetComponent<CharacterBody>().bodyIndex));
					}
					else
					{
						ProjectileManager.instance.FireProjectile(TemplarFireRocket.rocketPrefab, aimRay.origin, Util.QuaternionSafeLookRotation(vector), base.gameObject, this.damageStat * TemplarFireRocket.damageCoefficient, TemplarFireRocket.force, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, speedOverride);
					}
					this.projectilesFired++;
				}
			}
			bool flag6 = base.fixedAge >= this.duration && base.isAuthority;
			if (flag6)
			{
				this.outer.SetNextStateToMain();
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00006D24 File Offset: 0x00004F24
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}

		// Token: 0x040000B2 RID: 178
		public static GameObject rocketPrefab;

		// Token: 0x040000B3 RID: 179
		public static GameObject effectPrefab;

		// Token: 0x040000B4 RID: 180
		public static int projectileCount = 1;

		// Token: 0x040000B5 RID: 181
		public static float totalYawSpread = 1f;

		// Token: 0x040000B6 RID: 182
		public static float baseDuration = 0.25f;

		// Token: 0x040000B7 RID: 183
		public static float baseFireDuration = 0.2f;

		// Token: 0x040000B8 RID: 184
		public static float damageCoefficient = PlayableTemplar.bazookaDamageCoefficient.Value;

		// Token: 0x040000B9 RID: 185
		public static float force = 25f;

		// Token: 0x040000BA RID: 186
		public static float recoilAmplitude = 10f;

		// Token: 0x040000BB RID: 187
		private float duration;

		// Token: 0x040000BC RID: 188
		private float fireDuration;

		// Token: 0x040000BD RID: 189
		private int projectilesFired;

		// Token: 0x040000BE RID: 190
		private bool jelly;
	}
}
