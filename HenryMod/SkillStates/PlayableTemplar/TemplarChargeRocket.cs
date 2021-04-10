using System;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.LemurianBruiserMonster;
using RoR2;
using UnityEngine;

namespace EntityStates.Templar
{
	// Token: 0x0200001A RID: 26
	public class TemplarChargeRocket : BaseState
	{
		// Token: 0x0600008D RID: 141 RVA: 0x0000679C File Offset: 0x0000499C
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = TemplarChargeRocket.baseDuration / this.attackSpeedStat;
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

		// Token: 0x0600008E RID: 142 RVA: 0x000068D4 File Offset: 0x00004AD4
		public override void OnExit()
		{
			base.OnExit();
			bool flag = this.chargeInstance;
			if (flag)
			{
				EntityState.Destroy(this.chargeInstance);
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00006906 File Offset: 0x00004B06
		public override void Update()
		{
			base.Update();
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00006910 File Offset: 0x00004B10
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			base.StartAimMode(2f, false);
			bool flag = base.fixedAge >= this.duration && base.isAuthority && !this.skillButtonState.down;
			if (flag)
			{
				TemplarFireRocket nextState = new TemplarFireRocket();
				this.outer.SetNextState(nextState);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00006974 File Offset: 0x00004B74
		protected ref InputBankTest.ButtonState skillButtonState
		{
			get
			{
				return ref base.inputBank.skill4;
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00006994 File Offset: 0x00004B94
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}

		// Token: 0x040000AF RID: 175
		public static float baseDuration = 0.5f;

		// Token: 0x040000B0 RID: 176
		private float duration;

		// Token: 0x040000B1 RID: 177
		private GameObject chargeInstance;
	}
}
