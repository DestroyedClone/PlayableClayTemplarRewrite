using System;
using EntityStates.ClayBoss;
using PlayableTemplar;
using RoR2;
using UnityEngine;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	public class TemplarOverdrive : BaseSkillState
	{
		public static float pushForce = 2500f;
		private Transform modelTransform;

		public override void OnEnter()
		{
			base.OnEnter();
			bool flag = base.characterBody;
			if (flag)
			{
				base.characterBody.AddTimedBuff(Modules.Buffs.overdriveBuff, 3f);
			}
			bool flag2 = FireTarball.effectPrefab;
			if (flag2)
			{
				Transform exists = base.GetModelTransform();
				bool flag3 = exists;
				if (flag3)
				{
					EffectManager.SimpleMuzzleFlash(FireTarball.effectPrefab, base.gameObject, "Root", false);
				}
			}
			BlastAttack blastAttack = new BlastAttack
			{
				attacker = base.gameObject,
				inflictor = base.gameObject,
				teamIndex = base.teamComponent.teamIndex,
				baseForce = TemplarOverdrive.pushForce,
				bonusForce = Vector3.zero,
				position = base.transform.position,
				radius = 12f,
				falloffModel = BlastAttack.FalloffModel.None,
				crit = false,
				baseDamage = 0f,
				procCoefficient = 0f,
				damageType = DamageType.ClayGoo
			};
			blastAttack.Fire();
			this.modelTransform = base.GetModelTransform();
			bool flag4 = this.modelTransform;
			if (flag4)
			{
				TemporaryOverlay temporaryOverlay = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
				temporaryOverlay.duration = 8f;
				temporaryOverlay.animateShaderAlpha = true;
				temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
				temporaryOverlay.destroyComponentOnEnd = true;
				temporaryOverlay.originalMaterial = Resources.Load<Material>("Materials/matClayGooDebuff");
				temporaryOverlay.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
			}
			Util.PlayAttackSpeedSound(FireTarball.attackSoundString, base.gameObject, 0.75f);
			this.outer.SetNextStateToMain();
		}

		public override void OnExit()
		{
			base.OnExit();
		}
	}
}
