using System;
using EntityStates.ClayBruiser.Weapon;
using RoR2;
using UnityEngine;

namespace EntityStates.Templar
{
	// Token: 0x0200000F RID: 15
	public class TemplarRifleSpinDown : TemplarRifleState
	{
		// Token: 0x06000051 RID: 81 RVA: 0x00004D08 File Offset: 0x00002F08
		public override void OnEnter()
		{
			base.OnEnter();
			base.characterBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");
			this.duration = MinigunSpinDown.baseDuration * 0.1f / this.attackSpeedStat;
			Util.PlayScaledSound(MinigunSpinDown.sound, base.gameObject, this.attackSpeedStat);
			base.GetModelAnimator().SetBool("WeaponIsReady", false);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00004D74 File Offset: 0x00002F74
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			bool flag = base.fixedAge >= this.duration && base.isAuthority;
			if (flag)
			{
				this.outer.SetNextStateToMain();
			}
		}

		// Token: 0x04000064 RID: 100
		private float duration;
	}
}
