using System;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.LemurianBruiserMonster;
using PlayableTemplar;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	public class TemplarFireMiniRocket : BaseSkillState
	{
		public static GameObject rocketPrefab;
		public static GameObject effectPrefab;
		public static int projectileCount = 1;
		public static float totalYawSpread = 1f;
		public static float baseDuration = 0.25f;
		public static float baseFireDuration = 0.25f;
		public static float damageCoefficient = Modules.Config.miniBazookaDamageCoefficient.Value;
		public static float force = 25f;
		public static float recoilAmplitude = 7.5f;
		private float duration;
		private float fireDuration;
		private int projectilesFired;
		private bool jelly;

		public override void OnEnter()
		{
			base.OnEnter();
			base.StartAimMode(2f, false);
			this.jelly = false;
			TemplarFireMiniRocket.rocketPrefab = Modules.Projectiles.templarRocket;
			TemplarFireMiniRocket.effectPrefab = FireMegaFireball.muzzleflashEffectPrefab;
			bool value = Modules.Config.jellyfishEvent.Value;
			if (value)
			{
				bool flag = UnityEngine.Random.Range(0, 100) <= 5;
				if (flag)
				{
					TemplarFireMiniRocket.rocketPrefab = Resources.Load<GameObject>("Prefabs/CharacterBodies/JellyfishBody");
					this.jelly = true;
				}
				else
				{
					TemplarFireMiniRocket.rocketPrefab = Modules.Projectiles.templarRocket;
				}
			}
			base.AddRecoil(-1f * TemplarFireMiniRocket.recoilAmplitude, -2f * TemplarFireMiniRocket.recoilAmplitude, -0.5f * TemplarFireMiniRocket.recoilAmplitude, 0.5f * TemplarFireMiniRocket.recoilAmplitude);
			this.duration = TemplarFireMiniRocket.baseDuration / this.attackSpeedStat;
			this.fireDuration = TemplarFireMiniRocket.baseFireDuration / this.attackSpeedStat;
			Util.PlaySound(FireMegaFireball.attackString, base.gameObject);
		}

		public override void OnExit()
		{
			base.OnExit();
			base.PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
			base.GetModelAnimator().SetBool("WeaponIsReady", false);
			base.characterBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");
		}

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
					Vector3 vector = Util.ApplySpread(aimRay.direction, 0f, 0f, 1f, 1f, 0f, 0f);
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

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}
