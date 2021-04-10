using System;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.LemurianBruiserMonster;
using RoR2;
using UnityEngine;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	// Token: 0x02000018 RID: 24
	public class TemplarChargeMiniRocket : BaseSkillState
	{
		// Token: 0x0600007F RID: 127 RVA: 0x000061A4 File Offset: 0x000043A4
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = TemplarChargeMiniRocket.baseDuration / this.attackSpeedStat;
			Object modelAnimator = base.GetModelAnimator();
			Transform modelTransform = base.GetModelTransform();
			base.GetModelAnimator().SetBool("WeaponIsReady", true);
			base.PlayCrossfade("Gesture, Additive", "FireMinigun", 0.2f);
			base.characterBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/ToolbotGrenadeLauncherCrosshair");
			Util.PlayScaledSound(ChargeMegaFireball.attackString, base.gameObject, this.attackSpeedStat);
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

		// Token: 0x06000080 RID: 128 RVA: 0x000062DC File Offset: 0x000044DC
		public override void OnExit()
		{
			base.OnExit();
			bool flag = this.chargeInstance;
			if (flag)
			{
				EntityState.Destroy(this.chargeInstance);
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000630E File Offset: 0x0000450E
		public override void Update()
		{
			base.Update();
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00006318 File Offset: 0x00004518
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

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000083 RID: 131 RVA: 0x0000637C File Offset: 0x0000457C
		protected ref InputBankTest.ButtonState skillButtonState
		{
			get
			{
				return ref base.inputBank.skill1;
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000639C File Offset: 0x0000459C
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}

		// Token: 0x0400009F RID: 159
		public static float baseDuration = 1.25f;

		// Token: 0x040000A0 RID: 160
		private float duration;

		// Token: 0x040000A1 RID: 161
		private GameObject chargeInstance;
	}
}
