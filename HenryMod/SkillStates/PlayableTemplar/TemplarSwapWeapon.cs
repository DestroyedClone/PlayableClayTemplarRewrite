using System;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.ParentMonster;
using EntityStates.ScavMonster;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	// Token: 0x0200001D RID: 29
	public class TemplarSwapWeapon : BaseSkillState
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x00007240 File Offset: 0x00005440
		public override void OnEnter()
		{
			base.OnEnter();
			SkillDef skillDef = base.characterBody.skillLocator.primary.skillDef;
			bool flag = skillDef.skillNameToken == "TEMPLAR_PRIMARY_MINIGUN_NAME";
			if (flag)
			{
				TemplarSwapWeapon.currentWeapon = 0;
			}
			else
			{
				bool flag2 = skillDef.skillNameToken == "TEMPLAR_PRIMARY_PRECISEMINIGUN_NAME";
				if (flag2)
				{
					TemplarSwapWeapon.currentWeapon = 1;
				}
				else
				{
					bool flag3 = skillDef.skillNameToken == "TEMPLAR_PRIMARY_RAILGUN_NAME";
					if (flag3)
					{
						TemplarSwapWeapon.currentWeapon = 2;
					}
					else
					{
						bool flag4 = skillDef.skillNameToken == "TEMPLAR_PRIMARY_BAZOOKA_NAME";
						if (flag4)
						{
							TemplarSwapWeapon.currentWeapon = 2;
						}
						else
						{
							bool flag5 = skillDef.skillNameToken == "TEMPLAR_PRIMARY_FLAMETHROWER_NAME";
							if (flag5)
							{
								TemplarSwapWeapon.currentWeapon = 3;
							}
						}
					}
				}
			}
			TemplarSwapWeapon.currentWeapon++;
			bool flag6 = TemplarSwapWeapon.currentWeapon > 3;
			if (flag6)
			{
				TemplarSwapWeapon.currentWeapon = 0;
			}
			base.characterBody.skillLocator.primary.SetBaseSkill(base.characterBody.skillLocator.primary.skillFamily.variants[TemplarSwapWeapon.currentWeapon].skillDef);
			bool flag7 = TemplarSwapWeapon.currentWeapon == 0;
			if (flag7)
			{
				base.characterBody.crosshairPrefab = TemplarSwapWeapon.crosshair1;
			}
			else
			{
				bool flag8 = TemplarSwapWeapon.currentWeapon == 1;
				if (flag8)
				{
					base.characterBody.crosshairPrefab = TemplarSwapWeapon.crosshair2;
				}
				else
				{
					bool flag9 = TemplarSwapWeapon.currentWeapon == 2;
					if (flag9)
					{
						base.characterBody.crosshairPrefab = TemplarSwapWeapon.crosshair3;
					}
					else
					{
						bool flag10 = TemplarSwapWeapon.currentWeapon == 3;
						if (flag10)
						{
							base.characterBody.crosshairPrefab = TemplarSwapWeapon.crosshair4;
						}
					}
				}
			}
			Util.PlaySound(FindItem.sound, base.gameObject);
			base.GetModelAnimator().SetBool("WeaponIsReady", true);
			this.muzzleTransform = base.FindModelChild(MinigunState.muzzleName);
			bool flag11 = this.muzzleTransform && MinigunSpinUp.chargeEffectPrefab;
			if (flag11)
			{
				this.swapInstance = Object.Instantiate<GameObject>(LoomingPresence.blinkPrefab, this.muzzleTransform.position, this.muzzleTransform.rotation);
				this.swapInstance.transform.parent = this.muzzleTransform;
			}
			this.duration = TemplarSwapWeapon.baseDuration / this.attackSpeedStat;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000747C File Offset: 0x0000567C
		public override void OnExit()
		{
			base.PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
			base.GetModelAnimator().SetBool("WeaponIsReady", false);
			base.characterBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");
			base.OnExit();
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000074D0 File Offset: 0x000056D0
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			bool flag = base.fixedAge >= this.duration && base.isAuthority;
			if (flag)
			{
				this.outer.SetNextStateToMain();
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00007510 File Offset: 0x00005710
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Skill;
		}

		// Token: 0x040000CC RID: 204
		public static float baseDuration = 0.35f;

		// Token: 0x040000CD RID: 205
		public static int currentWeapon;

		// Token: 0x040000CE RID: 206
		public static GameObject crosshair1 = Resources.Load<GameObject>("prefabs/crosshair/BanditCrosshair");

		// Token: 0x040000CF RID: 207
		public static GameObject crosshair2 = Resources.Load<GameObject>("prefabs/crosshair/StandardCrosshair");

		// Token: 0x040000D0 RID: 208
		public static GameObject crosshair3 = Resources.Load<GameObject>("prefabs/crosshair/BanditCrosshair");

		// Token: 0x040000D1 RID: 209
		public static GameObject crosshair4 = Resources.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");

		// Token: 0x040000D2 RID: 210
		private float duration;

		// Token: 0x040000D3 RID: 211
		private Transform muzzleTransform;

		// Token: 0x040000D4 RID: 212
		private GameObject swapInstance;
	}
}
