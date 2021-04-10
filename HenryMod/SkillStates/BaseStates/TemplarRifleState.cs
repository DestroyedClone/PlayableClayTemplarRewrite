using System;
using EntityStates.ClayBruiser.Weapon;
using RoR2;
using UnityEngine;

namespace EntityStates.Templar
{
	// Token: 0x02000010 RID: 16
	public class TemplarRifleState : BaseState
	{
		// Token: 0x06000054 RID: 84 RVA: 0x00004DBB File Offset: 0x00002FBB
		public override void OnEnter()
		{
			base.OnEnter();
			this.muzzleTransform = base.FindModelChild(MinigunState.muzzleName);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00004DD6 File Offset: 0x00002FD6
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			base.StartAimMode(0.5f, false);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004DED File Offset: 0x00002FED
		public override void OnExit()
		{
			base.OnExit();
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00004DF8 File Offset: 0x00002FF8
		protected ref InputBankTest.ButtonState skillButtonState
		{
			get
			{
				return ref base.inputBank.skill1;
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004E18 File Offset: 0x00003018
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Skill;
		}

		// Token: 0x04000065 RID: 101
		private static readonly BuffIndex slowBuff;

		// Token: 0x04000066 RID: 102
		protected Transform muzzleTransform;
	}
}
