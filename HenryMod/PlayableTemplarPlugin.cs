using BepInEx;
using R2API.Utils;
using RoR2;
using System.Security;
using System.Security.Permissions;
using UnityEngine;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace PlayableTemplar
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "SoundAPI",
    })]

    public class PlayableTemplarPlugin : BaseUnityPlugin
    {
        // if you don't change these you're giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.AmogusLovers.PlayableClayTemplar";
        public const string MODNAME = "Playable Clay Templar";
        public const string MODVERSION = "1.0.0";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string developerPrefix = "SUS";

        public static PlayableTemplarPlugin instance;

        private void Awake()
        {
            instance = this;

            // load assets and read config
            Modules.Assets.Initialize();
            Modules.Config.ReadConfig();
            Modules.States.RegisterStates(); // register states for networking
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            Modules.Tokens.AddTokens(); // register name tokens
            Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

            // create your survivor here
            new Modules.Survivors.MyCharacter().Initialize();

            // now make a content pack and add it- this part will change with the next update
            new Modules.ContentPacks().CreateContentPack();

            Hook();
        }

        private void Start()
        {
            // have to set item displays in start now because they require direct object references..
            Modules.Survivors.MyCharacter.instance.SetItemDisplays();
        }

        private void Hook()
        {
            // run hooks here, disabling one is as simple as commenting out the line
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo di)
        {
            orig(self, di);
			if (di.inflictor)
			{
				if (self)
				{
					var selfCharacterBody = self.GetComponent<CharacterBody>();
					if (selfCharacterBody)
					{
						if (di.attacker)
						{
							var attackerCharacterBody = di.attacker.GetComponent<CharacterBody>();
							if (attackerCharacterBody)
							{
								bool attackerIsTemplar = di.attacker.GetComponent<CharacterBody>().baseNameToken == "TEMPLAR_NAME";
								if (attackerIsTemplar)
								{
									bool validDamageType = di.damageType.HasFlag(DamageType.BypassOneShotProtection);
									if (validDamageType)
									{
										di.damageType = DamageType.AOE;

										//var buff21 = ArrayUtils.GetSafe<int>(characterBody.buffs, 21) > 0;
										//TODO: https://discordapp.com/channels/562704639141740588/562704639569428506/832713043158892596
										bool flag15 = selfCharacterBody.HasBuff((BuffIndex)21) && !selfCharacterBody.HasBuff(Modules.Buffs.igniteDebuff);
										if (flag15)
										{
											selfCharacterBody.AddTimedBuff(Modules.Buffs.igniteDebuff, 16f);

											var modelLocator = self.GetComponent<CharacterBody>().modelLocator;
											if (modelLocator)
											{
												Transform modelTransform = modelLocator.modelTransform;
												var characterModel = modelTransform.GetComponent<CharacterModel>();
												if (characterModel)
												{
													TemporaryOverlay temporaryOverlay = modelTransform.gameObject.AddComponent<TemporaryOverlay>();
													temporaryOverlay.duration = 16f;
													temporaryOverlay.animateShaderAlpha = true;
													temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
													temporaryOverlay.destroyComponentOnEnd = true;
													temporaryOverlay.originalMaterial = Resources.Load<Material>("Materials/matDoppelganger");
													temporaryOverlay.AddToCharacerModel(characterModel);
												}
											}
											BlastAttack blastAttack = new BlastAttack
											{
												attacker = di.inflictor,
												inflictor = di.inflictor,
												teamIndex = TeamIndex.Player,
												baseForce = 0f,
												position = self.transform.position,
												radius = 12f,
												falloffModel = BlastAttack.FalloffModel.None,
												crit = di.crit,
												baseDamage = di.damage * 0.2f,
												procCoefficient = di.procCoefficient
											};
											blastAttack.damageType |= DamageType.Stun1s;
											blastAttack.Fire();
											BlastAttack blastAttack2 = new BlastAttack
											{
												attacker = di.inflictor,
												inflictor = di.inflictor,
												teamIndex = TeamIndex.Player,
												baseForce = 0f,
												position = self.transform.position,
												radius = 16f,
												falloffModel = BlastAttack.FalloffModel.None,
												crit = false,
												baseDamage = 0f,
												procCoefficient = 0f,
												damageType = DamageType.BypassOneShotProtection
											};
											blastAttack2.Fire();
											EffectManager.SpawnEffect(Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/MagmaOrbExplosion"), new EffectData
											{
												origin = self.transform.position,
												scale = 16f
											}, true);
										}
									}
									else
									{
										bool flag19 = di.damageType.HasFlag(DamageType.ClayGoo);
										bool flag20 = flag19;
										if (flag20)
										{
											di.damageType = DamageType.Generic;
											self.GetComponent<CharacterBody>().AddTimedBuff((BuffIndex)21, 4f);
										}
										bool flag21 = di.procCoefficient == PlayableTemplar.rifleProcCoefficient.Value;
										bool flag22 = flag21;
										if (flag22)
										{
											self.GetComponent<CharacterBody>().AddTimedBuff((BuffIndex)21, 4f);
										}
									}
								}
							}
						}
					}
				}
			}
		}

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            // a simple stat hook, adds armor after stats are recalculated
            if (self)
            {
                if (self.HasBuff(Modules.Buffs.armorBuff))
                {
                    self.armor += 300f;
                }
            }

			bool flag = self && self.HasBuff(Modules.Buffs.armorBuff);
			if (flag)
			{
				self.armor + Modules.Config.minigunArmorBoost.Value;
			}
			bool flag2 = self && self.HasBuff(Modules.Buffs.stationaryArmorBuff);
			if (flag2)
			{
				self.armor + PlayableTemplar.minigunStationaryArmorBoost.Value;
			}
			bool flag3 = self && self.HasBuff(Modules.Buffs.overdriveBuff);
			if (flag3)
			{
				self.regen *= 12f;
				self.attackSpeed *= 1.5f;
			}
			bool flag4 = self && self.HasBuff(Modules.Buffs.igniteDebuff);
			if (flag4)
			{
				self.armor -= 45f;
				self.moveSpeed *= 0.8f;
			}
		}
    }
}