using System;
using EntityStates.ClayBruiser.Weapon;
using RoR2;
using UnityEngine;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	public class TemplarRifleSpinDown : TemplarRifleState
	{
		private float duration;

		public override void OnEnter()
		{
			base.OnEnter();
			base.characterBody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/SimpleDotCrosshair");
			this.duration = MinigunSpinDown.baseDuration * 0.1f / this.attackSpeedStat;
			Util.PlayAttackSpeedSound(MinigunSpinDown.sound, base.gameObject, this.attackSpeedStat);
			base.GetModelAnimator().SetBool("WeaponIsReady", false);
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			bool flag = base.fixedAge >= this.duration && base.isAuthority;
			if (flag)
			{
				this.outer.SetNextStateToMain();
			}
		}
	}
}
