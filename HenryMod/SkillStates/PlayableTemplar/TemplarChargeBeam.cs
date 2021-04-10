using System;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.GolemMonster;
using RoR2;
using UnityEngine;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	public class TemplarChargeBeam : BaseState
	{
		public static float baseDuration = 1f;
		public static float laserMaxWidth = 0.2f;
		private float duration;
		private uint chargePlayID;
		private GameObject chargeEffect;
		private GameObject laserEffect;
		private LineRenderer laserLineComponent;
		private Vector3 laserDirection;
		private Vector3 visualEndPosition;
		private float flashTimer;
		private bool laserOn;
		private Animator animator;

		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = TemplarChargeBeam.baseDuration / this.attackSpeedStat;
			Transform modelTransform = base.GetModelTransform();
			this.animator = base.GetModelAnimator();
			base.StartAimMode(this.duration + 2f, false);
			this.animator.SetBool("WeaponIsReady", true);
			base.characterBody.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/BanditCrosshair");
			this.chargePlayID = Util.PlayAttackSpeedSound(ChargeLaser.attackSoundString, base.gameObject, this.attackSpeedStat);
			if (modelTransform)
			{
				ChildLocator childLocator = modelTransform.GetComponent<ChildLocator>();
				if (childLocator)
				{
					Transform transform = childLocator.FindChild(MinigunState.muzzleName);
					if (transform)
					{
						if (ChargeLaser.effectPrefab)
						{
							this.chargeEffect = UnityEngine.Object.Instantiate<GameObject>(ChargeLaser.effectPrefab, transform.position, transform.rotation);
							this.chargeEffect.transform.parent = transform;
							ScaleParticleSystemDuration scaleParticleSystemDuration = this.chargeEffect.GetComponent<ScaleParticleSystemDuration>();
							if (scaleParticleSystemDuration)
							{
								scaleParticleSystemDuration.newDuration = this.duration;
							}
						}
						if (ChargeLaser.laserPrefab)
						{
							this.laserEffect = UnityEngine.Object.Instantiate<GameObject>(ChargeLaser.laserPrefab, transform.position, transform.rotation);
							this.laserEffect.transform.parent = transform;
							this.laserLineComponent = this.laserEffect.GetComponent<LineRenderer>();
						}
					}
				}
			}
			if (base.characterBody)
			{
				base.characterBody.SetAimTimer(this.duration);
			}
			this.flashTimer = 0f;
			this.laserOn = true;
		}

		public override void OnExit()
		{
			AkSoundEngine.StopPlayingID(this.chargePlayID);
			base.OnExit();
			if (this.chargeEffect)
			{
				EntityState.Destroy(this.chargeEffect);
			}
			if (this.laserEffect)
			{
				EntityState.Destroy(this.laserEffect);
			}
		}

		public override void Update()
		{
			base.Update();
			if (this.laserEffect && this.laserLineComponent)
			{
				float num = 1000f;
				Ray aimRay = base.GetAimRay();
				Vector3 position = this.laserEffect.transform.parent.position;
				Vector3 point = aimRay.GetPoint(num);
				this.laserDirection = point - position;
                if (Physics.Raycast(aimRay, out RaycastHit raycastHit, num, LayerIndex.world.mask | LayerIndex.entityPrecise.mask))
				{
					point = raycastHit.point;
				}
				this.laserLineComponent.SetPosition(0, position);
				this.laserLineComponent.SetPosition(1, point);
				float num2;
				if (this.duration - base.age > 0.5f)
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

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.fixedAge >= this.duration && base.isAuthority)
			{
                TemplarFireBeam templarFireBeam = new TemplarFireBeam
                {
                    laserDirection = this.laserDirection
                };
                this.outer.SetNextState(templarFireBeam);
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}
