using System;
using EntityStates.Toolbot;
using RoR2;
using UnityEngine;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	// Token: 0x02000017 RID: 23
	public class FireRailgun : GenericBulletBaseState
	{
		// Token: 0x0600007C RID: 124 RVA: 0x000060FC File Offset: 0x000042FC
		public override void ModifyBullet(BulletAttack bulletAttack)
		{
			base.ModifyBullet(bulletAttack);
			bulletAttack.stopperMask = LayerIndex.world.mask;
			bulletAttack.falloffModel = BulletAttack.FalloffModel.None;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000612C File Offset: 0x0000432C
		public override void FireBullet(Ray aimRay)
		{
			base.FireBullet(aimRay);
			base.characterBody.SetSpreadBloom(0.2f, false);
			base.AddRecoil(-0.6f * FireSpear.recoilAmplitude, -0.8f * FireSpear.recoilAmplitude, -0.1f * FireSpear.recoilAmplitude, 0.1f * FireSpear.recoilAmplitude);
			base.GetModelAnimator().SetBool("WeaponIsReady", false);
		}

		// Token: 0x0400009E RID: 158
		public float charge;
	}
}
