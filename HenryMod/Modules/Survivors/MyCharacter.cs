using BepInEx.Configuration;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayableTemplar.Modules.Survivors
{
    internal class MyCharacter : SurvivorBase
    {
        internal override string bodyName { get; set; } = "PlayableTemplar";

        internal override GameObject bodyPrefab { get; set; }
        internal override GameObject displayPrefab { get; set; }

        internal override ConfigEntry<bool> characterEnabled { get; set; }

        internal override float survivorSortPosition { get; set; } = 100f;

        internal override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            armor = 20f,
            armorGrowth = 0f,
            bodyName = instance.bodyName + "Body",
            bodyNameToken = PlayableTemplarPlugin.developerPrefix + "_PLAYABLETEMPLAR_BODY_NAME",
            bodyColor = Color.grey,
            characterPortrait = Modules.Assets.LoadCharacterIcon("PlayableTemplar"),
            crosshair = Modules.Assets.LoadCrosshair("Standard"),
            damage = 12f,
            healthGrowth = 33f,
            healthRegen = 1.5f,
            jumpCount = 1,
            maxHealth = 110f,
            subtitleNameToken = PlayableTemplarPlugin.developerPrefix + "_PLAYABLETEMPLAR_BODY_SUBTITLE",
            podPrefab = Resources.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody").GetComponent<CharacterBody>().preferredPodPrefab
        };

        internal static Material henryMat = Modules.Assets.CreateMaterial("matPlayableTemplar");
        internal override int mainRendererIndex { get; set; } = 2;


        internal override Type characterMainState { get; set; } = typeof(EntityStates.GenericCharacterMain);
        internal override Type characterSpawnState { get; set; } = typeof(EntityStates.GenericCharacterSpawnState);

        internal override ItemDisplayRuleSet itemDisplayRuleSet { get; set; }
        internal override List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules { get; set; }

        internal override UnlockableDef characterUnlockableDef { get; set; }
        private static UnlockableDef masterySkinUnlockableDef;

        internal override void InitializeUnlockables()
        {
            masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.MasteryAchievement>(true);
        }

        internal override void InitializeDoppelganger()
        {
            Modules.Prefabs.CreateGenericDoppelganger(bodyPrefab, bodyName + "MonsterMaster", "Commando");
        }

        internal override void InitializeHitboxes()
        {
            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            GameObject model = childLocator.gameObject;

            Transform hitboxTransform = childLocator.FindChild("SwordHitbox");
            Modules.Prefabs.SetupHitbox(model, hitboxTransform, "Sword");
        }

        internal override void InitializeSkills()
        {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);

            string prefix = PlayableTemplarPlugin.developerPrefix;

            #region Primary
            SkillDef minigunSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PLAYABLETEMPLAR_BODY_PRIMARY_MINIGUN_NAME",
                skillNameToken = prefix + "_PLAYABLETEMPLAR_BODY_PRIMARY_MINIGUN_NAME",
                skillDescriptionToken = prefix + "_PLAYABLETEMPLAR_BODY_PRIMARY_MINIGUN_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.TemplarMinigunSpinUp)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Any,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true, //new
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_RAPIDFIRE" }
            });

            SkillDef rifleSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PLAYABLETEMPLAR_BODY_PRIMARY_PRECISEMINIGUN_NAME",
                skillNameToken = prefix + "_PLAYABLETEMPLAR_BODY_PRIMARY_PRECISEMINIGUN_NAME",
                skillDescriptionToken = prefix + "_PLAYABLETEMPLAR_BODY_PRIMARY_PRECISEMINIGUN_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.TemplarRifleFire)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = true,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Any,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true, //new
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { }
            });

            Modules.Skills.AddPrimarySkill(bodyPrefab, minigunSkillDef);
            Modules.Skills.AddPrimarySkill(bodyPrefab, rifleSkillDef);

            if (Modules.Config.bazookaGoBoom.Value)
            {
                SkillDef bazookaSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
                {
                    skillName = prefix + "_PLAYABLETEMPLAR_BODY_PRIMARY_BAZOOKA_NAME",
                    skillNameToken = prefix + "_PLAYABLETEMPLAR_BODY_PRIMARY_BAZOOKA_NAME",
                    skillDescriptionToken = prefix + "_PLAYABLETEMPLAR_BODY_PRIMARY_BAZOOKA_DESCRIPTION",
                    skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                    activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.TemplarChargeMiniRocket)),
                    activationStateMachineName = "Weapon",
                    baseMaxStock = 1,
                    baseRechargeInterval = 0.1f,
                    beginSkillCooldownOnSkillEnd = true,
                    canceledFromSprinting = false,
                    forceSprintDuringState = false,
                    fullRestockOnAssign = true,
                    interruptPriority = EntityStates.InterruptPriority.Any,
                    resetCooldownTimerOnUse = false,
                    isCombatSkill = true,
                    mustKeyPress = false,
                    cancelSprintingOnActivation = true, //new
                    rechargeStock = 1,
                    requiredStock = 1,
                    stockToConsume = 1,
                    keywordTokens = new string[] { "KEYWORD_EXPLOSIVE" }
                });
                Modules.Skills.AddPrimarySkill(bodyPrefab, bazookaSkillDef);
            } else
            {
                SkillDef railgunSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
                {
                    skillName = prefix + "_PLAYABLETEMPLAR_BODY_PRIMARY_RAILGUN_NAME",
                    skillNameToken = prefix + "_PLAYABLETEMPLAR_BODY_PRIMARY_RAILGUN_NAME",
                    skillDescriptionToken = prefix + "_PLAYABLETEMPLAR_BODY_PRIMARY_RAILGUN_DESCRIPTION",
                    skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                    activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.TemplarChargeBeam)),
                    activationStateMachineName = "Weapon",
                    baseMaxStock = 1,
                    baseRechargeInterval = 0f,
                    beginSkillCooldownOnSkillEnd = true,
                    canceledFromSprinting = false,
                    forceSprintDuringState = false,
                    fullRestockOnAssign = true,
                    interruptPriority = EntityStates.InterruptPriority.Any,
                    resetCooldownTimerOnUse = false,
                    isCombatSkill = true,
                    mustKeyPress = false,
                    cancelSprintingOnActivation = true, //new
                    rechargeStock = 1,
                    requiredStock = 1,
                    stockToConsume = 1,
                    keywordTokens = new string[] { "KEYWORD_EXPLOSIVE" }
                });
                Modules.Skills.AddPrimarySkill(bodyPrefab, railgunSkillDef);
            }

            SkillDef flameThrowerSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PLAYABLETEMPLAR_BODY_PRIMARY_FLAMETHROWER_NAME",
                skillNameToken = prefix + "_PLAYABLETEMPLAR_BODY_PRIMARY_FLAMETHROWER_NAME",
                skillDescriptionToken = prefix + "_PLAYABLETEMPLAR_BODY_PRIMARY_FLAMETHROWER_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.TemplarFlamethrower)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Any,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true, //new
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_EXPLOSIVE" }
            });
            Modules.Skills.AddPrimarySkill(bodyPrefab, flameThrowerSkillDef);
            #endregion

            #region Secondary
            SkillDef grenadeSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PLAYABLETEMPLAR_BODY_SECONDARY_GUN_NAME",
                skillNameToken = prefix + "_PLAYABLETEMPLAR_BODY_SECONDARY_GUN_NAME",
                skillDescriptionToken = prefix + "_PLAYABLETEMPLAR_BODY_SECONDARY_GUN_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.TemplarMinigunSpinUp)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { }
            });

            SkillDef shotgunSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PLAYABLETEMPLAR_BODY_SECONDARY_SHOTGUN_NAME",
                skillNameToken = prefix + "_PLAYABLETEMPLAR_BODY_SECONDARY_SHOTGUN_NAME",
                skillDescriptionToken = prefix + "_PLAYABLETEMPLAR_BODY_SECONDARY_SHOTGUN_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.TemplarMinigunSpinUp)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { }
            });

            Modules.Skills.AddSecondarySkill(bodyPrefab, grenadeSkillDef);
            Modules.Skills.AddSecondarySkill(bodyPrefab, shotgunSkillDef);
            #endregion

            #region Utility
            SkillDef overdriveSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PLAYABLETEMPLAR_BODY_UTILITY_OVERDRIVE_NAME",
                skillNameToken = prefix + "_PLAYABLETEMPLAR_BODY_UTILITY_OVERDRIVE_NAME",
                skillDescriptionToken = prefix + "_PLAYABLETEMPLAR_BODY_UTILITY_OVERDRIVE_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texUtilityIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.TemplarOverdrive)),
                activationStateMachineName = "Body",
                baseMaxStock = Modules.Config.overdriveStock.Value,
                baseRechargeInterval = Modules.Config.overdriveCooldown.Value,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = true,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            SkillDef rollSkillDef = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<SkillLocator>().utility.skillFamily.variants[0].skillDef;

            SkillDef dodgeSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PLAYABLETEMPLAR_BODY_UTILITY_DODGE_NAME",
                skillNameToken = prefix + "_PLAYABLETEMPLAR_BODY_UTILITY_DODGE_NAME",
                skillDescriptionToken = prefix + "_PLAYABLETEMPLAR_BODY_UTILITY_DODGE_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texUtilityIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.TemplarSidestep)),
                activationStateMachineName = "Body",
                baseMaxStock = Modules.Config.dashStock.Value,
                baseRechargeInterval = Modules.Config.dashCooldown.Value,
                beginSkillCooldownOnSkillEnd = rollSkillDef.beginSkillCooldownOnSkillEnd,
                canceledFromSprinting = false,
                forceSprintDuringState = true, //new
                fullRestockOnAssign = rollSkillDef.fullRestockOnAssign,
                interruptPriority = rollSkillDef.interruptPriority,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = rollSkillDef.mustKeyPress,
                cancelSprintingOnActivation = false,
                rechargeStock = rollSkillDef.rechargeStock,
                requiredStock = rollSkillDef.requiredStock,
                stockToConsume = rollSkillDef.stockToConsume
            });

            Modules.Skills.AddUtilitySkill(bodyPrefab, overdriveSkillDef);
            Modules.Skills.AddUtilitySkill(bodyPrefab, dodgeSkillDef);
            #endregion

            #region Special
            SkillDef bombSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PLAYABLETEMPLAR_BODY_SPECIAL_BOMB_NAME",
                skillNameToken = prefix + "_PLAYABLETEMPLAR_BODY_SPECIAL_BOMB_NAME",
                skillDescriptionToken = prefix + "_PLAYABLETEMPLAR_BODY_SPECIAL_BOMB_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.TemplarChargeRocket)),
                activationStateMachineName = "Weapon",
                baseMaxStock = Modules.Config.bazookaStock.Value,
                baseRechargeInterval = Modules.Config.bazookaCooldown.Value,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_EXPLOSIVE" }
            });
            SkillDef swapSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_PLAYABLETEMPLAR_BODY_SPECIAL_SWAP_NAME",
                skillNameToken = prefix + "_PLAYABLETEMPLAR_BODY_SPECIAL_SWAP_NAME",
                skillDescriptionToken = prefix + "_PLAYABLETEMPLAR_BODY_SPECIAL_SWAP_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.TemplarSwapWeapon)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 0.1f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });

            Modules.Skills.AddSpecialSkill(bodyPrefab, bombSkillDef);
            Modules.Skills.AddSpecialSkill(bodyPrefab, swapSkillDef);
            #endregion
        }

        internal override void InitializeSkins()
        {
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;

            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(PlayableTemplarPlugin.developerPrefix + "_PLAYABLETEMPLAR_BODY_DEFAULT_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMainSkin"),
                defaultRenderers,
                mainRenderer,
                model);

            defaultSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenrySword"),
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenryGun"),
                    renderer = defaultRenderers[1].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenry"),
                    renderer = defaultRenderers[instance.mainRendererIndex].renderer
                }
            };

            skins.Add(defaultSkin);
            #endregion

            #region MasterySkin
            Material masteryMat = Modules.Assets.CreateMaterial("matHenryAlt");
            CharacterModel.RendererInfo[] masteryRendererInfos = SkinRendererInfos(defaultRenderers, new Material[]
            {
                masteryMat,
                masteryMat
            });

            SkinDef masterySkin = Modules.Skins.CreateSkinDef(PlayableTemplarPlugin.developerPrefix + "_PLAYABLETEMPLAR_BODY_MASTERY_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
                masteryRendererInfos,
                mainRenderer,
                model,
                masterySkinUnlockableDef);

            masterySkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenrySwordAlt"),
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenryAlt"),
                    renderer = defaultRenderers[instance.mainRendererIndex].renderer
                }
            };

            skins.Add(masterySkin);
            #endregion

            skinController.skins = skins.ToArray();
        }

        private static CharacterModel.RendererInfo[] SkinRendererInfos(CharacterModel.RendererInfo[] defaultRenderers, Material[] materials)
        {
            CharacterModel.RendererInfo[] newRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(newRendererInfos, 0);

            newRendererInfos[0].defaultMaterial = materials[0];
            newRendererInfos[instance.mainRendererIndex].defaultMaterial = materials[2];

            return newRendererInfos;
        }

        internal override void InitializeItemDisplays()
        {
            
        }
    }
}