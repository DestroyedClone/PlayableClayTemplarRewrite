using System;
using EntityStates.ClayBruiser.Weapon;
using PlayableTemplar;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	public class TemplarMinigunState : BaseSkillState
	{
		private static readonly BuffIndex slowBuff;
		private BuffIndex armorBuff;
		private BuffIndex stationaryArmorBuff;
		private readonly bool standStill;
		protected Transform muzzleTransform;
		private float oldMass;

		public override void OnEnter()
		{
			base.OnEnter();
			this.armorBuff = Modules.Buffs.armorBuff.buffIndex;
			this.stationaryArmorBuff = Modules.Buffs.stationaryArmorBuff.buffIndex;
			this.oldMass = base.characterMotor.mass;
			this.muzzleTransform = base.FindModelChild(MinigunState.muzzleName);
			bool flag = NetworkServer.active && base.characterBody;
			if (flag)
			{
				base.characterBody.AddBuff(TemplarMinigunState.slowBuff);
				base.characterBody.AddBuff(this.armorBuff);
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			base.StartAimMode(2.5f, false);
			base.characterBody.isSprinting = false;
			bool isStationary = base.characterMotor.velocity.x == 0f && base.characterMotor.velocity.z == 0f && base.characterMotor.isGrounded;
			bool isGrounded = base.characterMotor.isGrounded;
			bool flag2 = isStationary && isGrounded;
			if (flag2)
			{
				bool flag3 = !base.characterBody.HasBuff(this.stationaryArmorBuff);
				if (flag3)
				{
					bool active = NetworkServer.active;
					if (active)
					{
						base.characterBody.RemoveBuff(this.armorBuff);
						base.characterBody.AddBuff(this.stationaryArmorBuff);
					}
					base.characterMotor.mass = 10000f;
				}
			}
			else
			{
				bool flag4 = base.characterBody.HasBuff(this.stationaryArmorBuff);
				if (flag4)
				{
					bool active2 = NetworkServer.active;
					if (active2)
					{
						base.characterBody.RemoveBuff(this.stationaryArmorBuff);
						base.characterBody.AddBuff(this.armorBuff);
					}
					base.characterMotor.mass = this.oldMass;
				}
			}
		}

		public override void OnExit()
		{
			bool flag = NetworkServer.active && base.characterBody;
			if (flag)
			{
				base.characterBody.RemoveBuff(TemplarMinigunState.slowBuff);
				bool flag2 = base.HasBuff(this.armorBuff);
				if (flag2)
				{
					base.characterBody.RemoveBuff(this.armorBuff);
				}
				bool flag3 = base.HasBuff(this.stationaryArmorBuff);
				if (flag3)
				{
					base.characterBody.RemoveBuff(this.stationaryArmorBuff);
				}
			}
			base.characterMotor.mass = this.oldMass;
			base.OnExit();
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
			return InterruptPriority.Skill;
		}
	}
}
