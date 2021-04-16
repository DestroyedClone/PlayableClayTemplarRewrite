
using EntityStates.ClayBruiser.Weapon;
using EntityStates.LemurianBruiserMonster;
using PlayableTemplar;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	public class TemplarFlamethrower : BaseSkillState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			this.stopwatch = 0f;
			this.entryDuration = TemplarFlamethrower.baseEntryDuration / this.attackSpeedStat;
			this.exitDuration = TemplarFlamethrower.baseExitDuration / this.attackSpeedStat;
			this.animator = base.GetModelAnimator();
			Transform modelTransform = base.GetModelTransform();
			this.childLocator = modelTransform.GetComponent<ChildLocator>();
			this.animator.SetBool("WeaponIsReady", true);
			this.flamethrowerDuration = 5f;
			bool flag = NetworkServer.active && base.characterBody;
			if (flag)
			{
				base.characterBody.AddBuff(Modules.Buffs.armorBuff);
			}
			float num = this.attackSpeedStat * TemplarFlamethrower.tickFrequency;
			this.tickDamageCoefficient = TemplarFlamethrower.damageCoefficientPerTick;
		}

		public override void OnExit()
		{
			Util.PlaySound(Flamebreath.endAttackSoundString, base.gameObject);
			bool flag = this.flamethrowerEffectInstance;
			if (flag)
			{
				EntityState.Destroy(this.flamethrowerEffectInstance.gameObject);
			}
			bool flag2 = NetworkServer.active && base.characterBody;
			if (flag2)
			{
				base.characterBody.RemoveBuff(Modules.Buffs.armorBuff);
			}
			this.animator.SetBool("WeaponIsReady", false);
			base.PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.2f);
			base.OnExit();
		}

		private void FireFlame(string muzzleString)
		{
			base.GetAimRay();
			bool flag = base.isAuthority && this.muzzleTransform;
			if (flag)
			{
                BulletAttack bulletAttack = new BulletAttack
                {
                    owner = base.gameObject,
                    weapon = base.gameObject,
                    origin = this.muzzleTransform.position,
                    aimVector = this.muzzleTransform.forward,
                    minSpread = 0f,
                    maxSpread = TemplarFlamethrower.maxSpread,
                    damage = this.tickDamageCoefficient * this.damageStat,
                    force = TemplarFlamethrower.force,
                    muzzleName = muzzleString,
                    hitEffectPrefab = Flamebreath.impactEffectPrefab,
                    isCrit = base.RollCrit(),
                    radius = TemplarFlamethrower.radius,
                    falloffModel = BulletAttack.FalloffModel.None
                };
                LayerIndex background = LayerIndex.world;
				bulletAttack.stopperMask = background.mask;
				bulletAttack.procCoefficient = TemplarFlamethrower.procCoefficientPerTick;
				bulletAttack.maxDistance = TemplarFlamethrower.maxDistance;
				bulletAttack.smartCollision = true;
				bulletAttack.damageType = DamageType.BypassOneShotProtection;
				bulletAttack.Fire();
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.stopwatch += Time.fixedDeltaTime;
			base.StartAimMode(0.5f, false);
			base.characterBody.isSprinting = false;
			bool flag = this.stopwatch >= this.entryDuration && this.stopwatch < this.entryDuration + this.flamethrowerDuration && !this.hasBegunFlamethrower;
			if (flag)
			{
				this.hasBegunFlamethrower = true;
				Util.PlaySound(Flamebreath.startAttackSoundString, base.gameObject);
				bool flag2 = this.childLocator;
				if (flag2)
				{
					this.muzzleTransform = this.childLocator.FindChild(MinigunState.muzzleName);
					this.flamethrowerEffectInstance = Object.Instantiate<GameObject>(Flamebreath.flamethrowerEffectPrefab, this.muzzleTransform).transform;
					this.flamethrowerEffectInstance.transform.localPosition = Vector3.zero;
					this.flamethrowerEffectInstance.GetComponent<ScaleParticleSystemDuration>().newDuration = 2f;
					foreach (ParticleSystem particleSystem in this.flamethrowerEffectInstance.GetComponentsInChildren<ParticleSystem>())
					{
						bool flag3 = particleSystem;
						if (flag3)
						{
							particleSystem.main.loop = true;
						}
					}
				}
			}
			bool flag4 = this.stopwatch >= this.entryDuration && this.hasBegunFlamethrower && !base.inputBank.skill1.down;
			if (flag4)
			{
				this.hasBegunFlamethrower = false;
				bool flag5 = this.flamethrowerEffectInstance;
				if (flag5)
				{
					EntityState.Destroy(this.flamethrowerEffectInstance.gameObject);
				}
				this.outer.SetNextStateToMain();
			}
			else
			{
				bool flag6 = this.hasBegunFlamethrower;
				if (flag6)
				{
					this.flamethrowerStopwatch += Time.fixedDeltaTime;
					bool flag7 = this.flamethrowerStopwatch > TemplarFlamethrower.tickFrequency / this.attackSpeedStat;
					if (flag7)
					{
						this.flamethrowerStopwatch -= TemplarFlamethrower.tickFrequency / this.attackSpeedStat;
						this.FireFlame(MinigunState.muzzleName);
						base.PlayCrossfade("Gesture, Additive", "FireMinigun", 0.2f);
					}
					this.flamethrowerDuration = this.stopwatch + TemplarFlamethrower.baseExitDuration;
				}
				else
				{
					bool flag8 = this.flamethrowerEffectInstance;
					if (flag8)
					{
						EntityState.Destroy(this.flamethrowerEffectInstance.gameObject);
					}
				}
				bool flag9 = this.stopwatch >= this.flamethrowerDuration + this.entryDuration && base.isAuthority;
				if (flag9)
				{
					this.outer.SetNextStateToMain();
				}
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000055C0 File Offset: 0x000037C0
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Skill;
		}

		// Token: 0x04000069 RID: 105
		public static float maxDistance = 32f;

		// Token: 0x0400006A RID: 106
		public static float radius = 1.5f;

		// Token: 0x0400006B RID: 107
		public static float baseEntryDuration = 0.1f;

		// Token: 0x0400006C RID: 108
		public static float baseExitDuration = 0.75f;

		// Token: 0x0400006D RID: 109
		public static float damageCoefficientPerTick = 2.5f;

		// Token: 0x0400006E RID: 110
		public static float procCoefficientPerTick = 0.4f;

		// Token: 0x0400006F RID: 111
		public static float tickFrequency = 0.25f;

		// Token: 0x04000070 RID: 112
		public static float force = 1f;

		// Token: 0x04000071 RID: 113
		public static float maxSpread = 0.1f;

		// Token: 0x04000072 RID: 114
		public static GameObject flamethrowerEffect = Resources.Load<GameObject>("Prefabs/Effects/DroneFlamethrowerEffect");

		// Token: 0x04000073 RID: 115
		private float tickDamageCoefficient;

		// Token: 0x04000074 RID: 116
		private float flamethrowerStopwatch;

		// Token: 0x04000075 RID: 117
		private float stopwatch;

		// Token: 0x04000076 RID: 118
		private float entryDuration;

		// Token: 0x04000077 RID: 119
		private float exitDuration;

		// Token: 0x04000078 RID: 120
		private float flamethrowerDuration;

		// Token: 0x04000079 RID: 121
		private bool hasBegunFlamethrower;

		// Token: 0x0400007A RID: 122
		private ChildLocator childLocator;

		// Token: 0x0400007B RID: 123
		private Transform flamethrowerEffectInstance;

		// Token: 0x0400007C RID: 124
		private Transform muzzleTransform;

		// Token: 0x0400007D RID: 125
		private const float flamethrowerEffectBaseDistance = 32f;

		// Token: 0x0400007E RID: 126
		private Animator animator;
	}
}
