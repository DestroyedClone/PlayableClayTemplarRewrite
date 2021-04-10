using System;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.LemurianBruiserMonster;
using PlayableTemplar;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace EntityStates.Templar
{
	// Token: 0x02000019 RID: 25
	public class TemplarFireMiniRocket : BaseState
	{
		// Token: 0x06000087 RID: 135 RVA: 0x000063C4 File Offset: 0x000045C4
		public override void OnEnter()
		{
			base.OnEnter();
			base.StartAimMode(2f, false);
			this.jelly = false;
			TemplarFireMiniRocket.rocketPrefab = PlayableTemplar.templarRocket;
			TemplarFireMiniRocket.effectPrefab = FireMegaFireball.muzzleflashEffectPrefab;
			bool value = PlayableTemplar.jellyfishEvent.Value;
			if (value)
			{
				bool flag = Random.Range(0, 100) <= 5;
				if (flag)
				{
					TemplarFireMiniRocket.rocketPrefab = Resources.Load<GameObject>("Prefabs/CharacterBodies/JellyfishBody");
					this.jelly = true;
				}
				else
				{
					TemplarFireMiniRocket.rocketPrefab = PlayableTemplar.templarRocket;
				}
			}
			base.AddRecoil(-1f * TemplarFireMiniRocket.recoilAmplitude, -2f * TemplarFireMiniRocket.recoilAmplitude, -0.5f * TemplarFireMiniRocket.recoilAmplitude, 0.5f * TemplarFireMiniRocket.recoilAmplitude);
			this.duration = TemplarFireMiniRocket.baseDuration / this.attackSpeedStat;
			this.fireDuration = TemplarFireMiniRocket.baseFireDuration / this.attackSpeedStat;
			Util.PlaySound(FireMegaFireball.attackString, base.gameObject);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000064B4 File Offset: 0x000046B4
		public override void OnExit()
		{
			base.OnExit();
			base.PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
			base.GetModelAnimator().SetBool("WeaponIsReady", false);
			base.characterBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00006508 File Offset: 0x00004708
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			string muzzleName = MinigunState.muzzleName;
			bool isAuthority = base.isAuthority;
			if (isAuthority)
			{
				int num = Mathf.FloorToInt(base.fixedAge / this.fireDuration * (float)TemplarFireMiniRocket.projectileCount);
				bool flag = this.projectilesFired <= num && this.projectilesFired < TemplarFireMiniRocket.projectileCount;
				if (flag)
				{
					EffectManager.SimpleMuzzleFlash(TemplarFireMiniRocket.effectPrefab, base.gameObject, muzzleName, false);
					Ray aimRay = base.GetAimRay();
					float speedOverride = FireMegaFireball.projectileSpeed * 2f;
					float bonusYaw = (float)Mathf.FloorToInt((float)this.projectilesFired - (float)(TemplarFireMiniRocket.projectileCount - 1) / 2f) / (float)(TemplarFireMiniRocket.projectileCount - 1) * TemplarFireMiniRocket.totalYawSpread;
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
						base.characterBody.SendConstructTurret(base.characterBody, position, Util.QuaternionSafeLookRotation(vector), MasterCatalog.FindAiMasterIndexForBody(TemplarFireMiniRocket.rocketPrefab.GetComponent<CharacterBody>().bodyIndex));
					}
					else
					{
						ProjectileManager.instance.FireProjectile(TemplarFireMiniRocket.rocketPrefab, aimRay.origin, Util.QuaternionSafeLookRotation(vector), base.gameObject, this.damageStat * TemplarFireMiniRocket.damageCoefficient, TemplarFireMiniRocket.force, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, speedOverride);
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

		// Token: 0x0600008A RID: 138 RVA: 0x0000672C File Offset: 0x0000492C
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}

		// Token: 0x040000A2 RID: 162
		public static GameObject rocketPrefab;

		// Token: 0x040000A3 RID: 163
		public static GameObject effectPrefab;

		// Token: 0x040000A4 RID: 164
		public static int projectileCount = 1;

		// Token: 0x040000A5 RID: 165
		public static float totalYawSpread = 1f;

		// Token: 0x040000A6 RID: 166
		public static float baseDuration = 0.25f;

		// Token: 0x040000A7 RID: 167
		public static float baseFireDuration = 0.25f;

		// Token: 0x040000A8 RID: 168
		public static float damageCoefficient = PlayableTemplar.miniBazookaDamageCoefficient.Value;

		// Token: 0x040000A9 RID: 169
		public static float force = 25f;

		// Token: 0x040000AA RID: 170
		public static float recoilAmplitude = 7.5f;

		// Token: 0x040000AB RID: 171
		private float duration;

		// Token: 0x040000AC RID: 172
		private float fireDuration;

		// Token: 0x040000AD RID: 173
		private int projectilesFired;

		// Token: 0x040000AE RID: 174
		private bool jelly;
	}
}
