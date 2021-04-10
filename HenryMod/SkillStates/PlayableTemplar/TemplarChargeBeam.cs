using System;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.GolemMonster;
using RoR2;
using UnityEngine;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	// Token: 0x02000014 RID: 20
	public class TemplarChargeBeam : BaseState
	{
		// Token: 0x06000069 RID: 105 RVA: 0x00005654 File Offset: 0x00003854
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = TemplarChargeBeam.baseDuration / this.attackSpeedStat;
			Transform modelTransform = base.GetModelTransform();
			this.animator = base.GetModelAnimator();
			base.StartAimMode(this.duration + 2f, false);
			this.animator.SetBool("WeaponIsReady", true);
			base.characterBody.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/BanditCrosshair");
			this.chargePlayID = Util.PlayScaledSound(ChargeLaser.attackSoundString, base.gameObject, this.attackSpeedStat);
			bool flag = modelTransform;
			if (flag)
			{
				ChildLocator component = modelTransform.GetComponent<ChildLocator>();
				bool flag2 = component;
				if (flag2)
				{
					Transform transform = component.FindChild(MinigunState.muzzleName);
					bool flag3 = transform;
					if (flag3)
					{
						bool flag4 = ChargeLaser.effectPrefab;
						if (flag4)
						{
							this.chargeEffect = UnityEngine.Object.Instantiate<GameObject>(ChargeLaser.effectPrefab, transform.position, transform.rotation);
							this.chargeEffect.transform.parent = transform;
							ScaleParticleSystemDuration component2 = this.chargeEffect.GetComponent<ScaleParticleSystemDuration>();
							bool flag5 = component2;
							if (flag5)
							{
								component2.newDuration = this.duration;
							}
						}
						bool flag6 = ChargeLaser.laserPrefab;
						if (flag6)
						{
							this.laserEffect = UnityEngine.Object.Instantiate<GameObject>(ChargeLaser.laserPrefab, transform.position, transform.rotation);
							this.laserEffect.transform.parent = transform;
							this.laserLineComponent = this.laserEffect.GetComponent<LineRenderer>();
						}
					}
				}
			}
			bool flag7 = base.characterBody;
			if (flag7)
			{
				base.characterBody.SetAimTimer(this.duration);
			}
			this.flashTimer = 0f;
			this.laserOn = true;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00005824 File Offset: 0x00003A24
		public override void OnExit()
		{
			AkSoundEngine.StopPlayingID(this.chargePlayID);
			base.OnExit();
			bool flag = this.chargeEffect;
			if (flag)
			{
				EntityState.Destroy(this.chargeEffect);
			}
			bool flag2 = this.laserEffect;
			if (flag2)
			{
				EntityState.Destroy(this.laserEffect);
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00005880 File Offset: 0x00003A80
		public override void Update()
		{
			base.Update();
			bool flag = this.laserEffect && this.laserLineComponent;
			if (flag)
			{
				float num = 1000f;
				Ray aimRay = base.GetAimRay();
				Vector3 position = this.laserEffect.transform.parent.position;
				Vector3 point = aimRay.GetPoint(num);
				this.laserDirection = point - position;
                bool flag2 = Physics.Raycast(aimRay, out RaycastHit raycastHit, num, LayerIndex.world.mask | LayerIndex.entityPrecise.mask);
                if (flag2)
				{
					point = raycastHit.point;
				}
				this.laserLineComponent.SetPosition(0, position);
				this.laserLineComponent.SetPosition(1, point);
				bool flag3 = this.duration - base.age > 0.5f;
				float num2;
				if (flag3)
				{
					num2 = base.age / this.duration;
				}
				else
				{
					this.flashTimer -= Time.deltaTime;
					bool flag4 = this.flashTimer <= 0f;
					if (flag4)
					{
						this.laserOn = !this.laserOn;
						this.flashTimer = 0.0333333351f;
					}
					num2 = (this.laserOn ? 1f : 0f);
				}
				num2 *= TemplarChargeBeam.laserMaxWidth;
				this.laserLineComponent.startWidth = num2;
				this.laserLineComponent.endWidth = num2;
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00005A04 File Offset: 0x00003C04
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			bool flag = base.fixedAge >= this.duration && base.isAuthority;
			if (flag)
			{
                TemplarFireBeam templarFireBeam = new TemplarFireBeam
                {
                    laserDirection = this.laserDirection
                };
                this.outer.SetNextState(templarFireBeam);
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00005A58 File Offset: 0x00003C58
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}

		// Token: 0x0400007F RID: 127
		public static float baseDuration = 1f;

		// Token: 0x04000080 RID: 128
		public static float laserMaxWidth = 0.2f;

		// Token: 0x04000081 RID: 129
		private float duration;

		// Token: 0x04000082 RID: 130
		private uint chargePlayID;

		// Token: 0x04000083 RID: 131
		private GameObject chargeEffect;

		// Token: 0x04000084 RID: 132
		private GameObject laserEffect;

		// Token: 0x04000085 RID: 133
		private LineRenderer laserLineComponent;

		// Token: 0x04000086 RID: 134
		private Vector3 laserDirection;

		// Token: 0x04000087 RID: 135
		private Vector3 visualEndPosition;

		// Token: 0x04000088 RID: 136
		private float flashTimer;

		// Token: 0x04000089 RID: 137
		private bool laserOn;

		// Token: 0x0400008A RID: 138
		private Animator animator;
	}
}
