using System;
using EntityStates.Treebot.Weapon;
using RoR2;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	public class TemplarFireSonicBoom : FireSonicBoom
	{
		public override void OnEnter()
		{
			base.OnEnter();
			base.GetModelAnimator().SetBool("WeaponIsReady", true);
			this.baseDuration = 0.5f;
		}

		public override void AddDebuff(CharacterBody body)
		{
			body.AddTimedBuff((BuffIndex)21, this.slowDuration * 3f);
		}

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
