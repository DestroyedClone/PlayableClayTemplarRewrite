using System;
using EntityStates.Treebot.Weapon;
using RoR2;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	// Token: 0x02000012 RID: 18
	public class TemplarFireSonicBoom : FireSonicBoom
	{
		// Token: 0x0600005E RID: 94 RVA: 0x00005018 File Offset: 0x00003218
		public override void OnEnter()
		{
			base.OnEnter();
			base.GetModelAnimator().SetBool("WeaponIsReady", true);
			this.baseDuration = 0.5f;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000503F File Offset: 0x0000323F
		public override void AddDebuff(CharacterBody body)
		{
			body.AddTimedBuff((BuffIndex)21, this.slowDuration * 3f);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00005058 File Offset: 0x00003258
		public override void OnExit()
		{
			bool flag = !this.outer.destroying;
			if (flag)
			{
				base.GetModelAnimator().SetBool("WeaponIsReady", false);
			}
			base.OnExit();
		}
	}
}
