
using EntityStates.ClayBruiser.Weapon;
using EntityStates.LemurianBruiserMonster;
using RoR2;
using UnityEngine;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	public class TemplarChargeMiniRocket : BaseSkillState
	{
		public static float baseDuration = 1.25f;
		private float duration;
		private GameObject chargeInstance;

		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = TemplarChargeMiniRocket.baseDuration / this.attackSpeedStat;
			Animator modelAnimator = base.GetModelAnimator();
			Transform modelTransform = base.GetModelTransform();
			base.GetModelAnimator().SetBool("WeaponIsReady", true);
			base.PlayCrossfade("Gesture, Additive", "FireMinigun", 0.2f);
			base.characterBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/ToolbotGrenadeLauncherCrosshair");
			Util.PlayAttackSpeedSound(ChargeMegaFireball.attackString, base.gameObject, this.attackSpeedStat);
			bool flag = modelTransform;
			if (flag)
			{
				ChildLocator component = modelTransform.GetComponent<ChildLocator>();
				bool flag2 = component;
				if (flag2)
				{
					Transform transform = component.FindChild(MinigunState.muzzleName);
					bool flag3 = transform && ChargeMegaFireball.chargeEffectPrefab;
					if (flag3)
					{
						this.chargeInstance = Object.Instantiate<GameObject>(ChargeMegaFireball.chargeEffectPrefab, transform.position, transform.rotation);
						this.chargeInstance.transform.parent = transform;
						ScaleParticleSystemDuration component2 = this.chargeInstance.GetComponent<ScaleParticleSystemDuration>();
						bool flag4 = component2;
						if (flag4)
						{
							component2.newDuration = this.duration;
						}
					}
				}
			}
		}

		public override void OnExit()
		{
			base.OnExit();
			bool flag = this.chargeInstance;
			if (flag)
			{
				EntityState.Destroy(this.chargeInstance);
			}
		}

		public override void Update()
		{
			base.Update();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			base.StartAimMode(2f, false);
			bool flag = base.fixedAge >= this.duration && base.isAuthority && !this.skillButtonState.down;
			if (flag)
			{
				TemplarFireMiniRocket nextState = new TemplarFireMiniRocket();
				this.outer.SetNextState(nextState);
			}
		}
		protected ref InputBankTest.ButtonState skillButtonState
		{
			get
			{
				return ref base.inputBank.skill1;
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}
