using System;
using EntityStates.ClayBruiser.Weapon;
using RoR2;
using UnityEngine;

namespace EntityStates.Templar
{
	// Token: 0x0200000B RID: 11
	public class TemplarMinigunSpinDown : TemplarMinigunState
	{
		// Token: 0x0600003C RID: 60 RVA: 0x00004370 File Offset: 0x00002570
		public override void OnEnter()
		{
			base.OnEnter();
			base.characterBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");
			this.duration = MinigunSpinDown.baseDuration * 0.25f / this.attackSpeedStat;
			Util.PlayScaledSound(MinigunSpinDown.sound, base.gameObject, this.attackSpeedStat);
			base.GetModelAnimator().SetBool("WeaponIsReady", false);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000043DC File Offset: 0x000025DC
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			bool flag = base.fixedAge >= this.duration && base.isAuthority;
			if (flag)
			{
				this.outer.SetNextStateToMain();
			}
		}

		// Token: 0x04000050 RID: 80
		private float duration;
	}
}
