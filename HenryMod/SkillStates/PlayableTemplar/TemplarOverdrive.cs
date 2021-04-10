using System;
using EntityStates.ClayBoss;
using PlayableTemplar;
using RoR2;
using UnityEngine;
using EntityStates;

namespace PlayableTemplar.SkillStates
{
	// Token: 0x02000011 RID: 17
	public class TemplarOverdrive : BaseSkillState
	{
		// Token: 0x0600005A RID: 90 RVA: 0x00004E34 File Offset: 0x00003034
		public override void OnEnter()
		{
			base.OnEnter();
			bool flag = base.characterBody;
			if (flag)
			{
				base.characterBody.AddTimedBuff(PlayableTemplar.instance.overdriveBuff, 3f);
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
			Util.PlayScaledSound(FireTarball.attackSoundString, base.gameObject, 0.75f);
			this.outer.SetNextStateToMain();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00004FF9 File Offset: 0x000031F9
		public override void OnExit()
		{
			base.OnExit();
		}

		// Token: 0x04000067 RID: 103
		public static float pushForce = 2500f;

		// Token: 0x04000068 RID: 104
		private Transform modelTransform;
	}
}
