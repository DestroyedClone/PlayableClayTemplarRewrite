using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using EntityStates;
using EntityStates.ClayBoss;
using EntityStates.ClayBruiserMonster;
using EntityStates.ClayMan;
using EntityStates.ClaymanMonster;
using EntityStates.Templar;
using KinematicCharacterController;
using On.RoR2;
using PlayableTemplar.Achievements;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Navigation;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

namespace PlayableTemplar
{
	// Token: 0x0200001F RID: 31
	[BepInDependency("com.bepis.r2api", 1)]
	[BepInPlugin("com.rob.PlayableTemplar", "Playable Templar", "2.1.1")]
	[R2APISubmoduleDependency(new string[]
	{
		"DirectorAPI",
		"PrefabAPI",
		"SurvivorAPI",
		"LoadoutAPI",
		"LanguageAPI",
		"BuffAPI",
		"EffectAPI",
		"UnlockablesAPI"
	})]
	public class PlayableTemplar : BaseUnityPlugin
	{
		// Token: 0x060000AF RID: 175 RVA: 0x00007A08 File Offset: 0x00005C08
		public void Awake()
		{
			PlayableTemplar.instance = this;
			this.ReadConfig();
			Assets.PopulateAssets();
			this.PopulateDisplays();
			this.RegisterTemplar();
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00008CC8 File Offset: 0x00006EC8
		private void RegisterTemplar()
		{
			this.myCharacter = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/ClayBruiserBody"), "TemplarBody", true, "C:\\Users\\rseid\\Documents\\ror2mods\\PlayableTemplar\\PlayableTemplar\\PlayableTemplar.cs", "RegisterTemplar", 468);
			this.myCharacter.GetComponent<SetStateOnHurt>().canBeHitStunned = false;
			this.myCharacter.tag = "Player";
			CharacterBody component = this.myCharacter.GetComponent<CharacterBody>();
			component.bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;
			component.portraitIcon = Assets.templarIconOld;
			bool value = PlayableTemplar.originalSize.Value;
			if (value)
			{
				this.myCharacter.GetComponent<ModelLocator>().modelBaseTransform.gameObject.transform.localScale = Vector3.one * 1.45f;
				this.myCharacter.GetComponent<ModelLocator>().modelBaseTransform.gameObject.transform.Translate(new Vector3(0f, 1f, 0f));
				component.aimOriginTransform.Translate(new Vector3(0f, 1.5f, 0f));
				foreach (KinematicCharacterMotor kinematicCharacterMotor in this.myCharacter.GetComponentsInChildren<KinematicCharacterMotor>())
				{
					kinematicCharacterMotor.SetCapsuleDimensions(kinematicCharacterMotor.Capsule.radius * 0.8f, kinematicCharacterMotor.Capsule.height * 0.7f, 0f);
				}
			}
			else
			{
				this.myCharacter.GetComponent<ModelLocator>().modelBaseTransform.gameObject.transform.localScale = Vector3.one * 0.9f;
				this.myCharacter.GetComponent<ModelLocator>().modelBaseTransform.gameObject.transform.Translate(new Vector3(0f, 1.6f, 0f));
				component.aimOriginTransform.Translate(new Vector3(0f, 0f, 0f));
				component.baseJumpPower = Resources.Load<GameObject>("Prefabs/CharacterBodies/LoaderBody").GetComponent<CharacterBody>().baseJumpPower;
				component.baseMoveSpeed = Resources.Load<GameObject>("Prefabs/CharacterBodies/LoaderBody").GetComponent<CharacterBody>().baseMoveSpeed;
				component.levelMoveSpeed = Resources.Load<GameObject>("Prefabs/CharacterBodies/LoaderBody").GetComponent<CharacterBody>().levelMoveSpeed;
				component.sprintingSpeedMultiplier = Resources.Load<GameObject>("Prefabs/CharacterBodies/LoaderBody").GetComponent<CharacterBody>().sprintingSpeedMultiplier;
				this.myCharacter.GetComponentInChildren<CharacterMotor>().mass = Resources.Load<GameObject>("Prefabs/CharacterBodies/LoaderBody").GetComponentInChildren<CharacterMotor>().mass;
				this.myCharacter.GetComponent<CameraTargetParams>().cameraParams = Resources.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody").GetComponent<CameraTargetParams>().cameraParams;
				foreach (KinematicCharacterMotor kinematicCharacterMotor2 in this.myCharacter.GetComponentsInChildren<KinematicCharacterMotor>())
				{
					kinematicCharacterMotor2.SetCapsuleDimensions(kinematicCharacterMotor2.Capsule.radius * 0.5f, kinematicCharacterMotor2.Capsule.height * 0.5f, 0f);
				}
			}
			component.SetSpreadBloom(0f, false);
			component.spreadBloomCurve = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<CharacterBody>().spreadBloomCurve;
			component.spreadBloomDecayTime = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<CharacterBody>().spreadBloomDecayTime;


			GameObject gameObject4 = Resources.Load<GameObject>("Prefabs/CharacterBodies/Pot2Body");
			MeshRenderer componentInChildren = gameObject4.GetComponentInChildren<MeshRenderer>();
			GameObject gameObject5 = PrefabAPI.InstantiateClone(componentInChildren.gameObject, "VagabondHead", false, "C:\\Users\\rseid\\Documents\\ror2mods\\PlayableTemplar\\PlayableTemplar\\PlayableTemplar.cs", "RegisterTemplar", 679);
			Object.Destroy(gameObject5.GetComponent<HurtBoxGroup>());
			Object.Destroy(gameObject5.transform.GetComponentInChildren<HurtBox>());
			Object.Destroy(gameObject5.transform.GetChild(0).gameObject);
			gameObject5.transform.parent = this.myCharacter.GetComponentInChildren<ChildLocator>().FindChild("Head");
			gameObject5.transform.localScale = new Vector3(1f, 1f, 1.25f);
			gameObject5.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
			gameObject5.transform.localPosition = new Vector3(0f, -0.24f, 0f);
			MeshRenderer componentInChildren2 = gameObject5.GetComponentInChildren<MeshRenderer>();
			CharacterModel.RendererInfo[] baseRendererInfos = this.myCharacter.GetComponentInChildren<CharacterModel>().baseRendererInfos;
			CharacterModel.RendererInfo[] baseRendererInfos2 = new CharacterModel.RendererInfo[]
			{
				baseRendererInfos[0],
				baseRendererInfos[1],
				baseRendererInfos[2],
				baseRendererInfos[3],
				baseRendererInfos[4],
				baseRendererInfos[5],
				new CharacterModel.RendererInfo
				{
					defaultMaterial = componentInChildren.material,
					renderer = componentInChildren2,
					defaultShadowCastingMode = ShadowCastingMode.On,
					ignoreOverlays = false
				}
			};
			this.myCharacter.GetComponentInChildren<CharacterModel>().baseRendererInfos = baseRendererInfos2;
			this.characterDisplay = PrefabAPI.InstantiateClone(this.myCharacter.GetComponent<ModelLocator>().modelBaseTransform.gameObject, "TemplarDisplay", true, "C:\\Users\\rseid\\Documents\\ror2mods\\PlayableTemplar\\PlayableTemplar\\PlayableTemplar.cs", "RegisterTemplar", 712);
			this.characterDisplay.transform.localScale = Vector3.one * 0.8f;
			this.characterDisplay.AddComponent<PlayableTemplar.TemplarMenuAnim>();
			this.characterDisplay.AddComponent<NetworkIdentity>();
			bool flag3 = gameObject;
			if (flag3)
			{
				PrefabAPI.RegisterNetworkPrefab(gameObject, "C:\\Users\\rseid\\Documents\\ror2mods\\PlayableTemplar\\PlayableTemplar\\PlayableTemplar.cs", "RegisterTemplar", 719);
			}
			bool flag4 = gameObject2;
			if (flag4)
			{
				PrefabAPI.RegisterNetworkPrefab(gameObject2, "C:\\Users\\rseid\\Documents\\ror2mods\\PlayableTemplar\\PlayableTemplar\\PlayableTemplar.cs", "RegisterTemplar", 720);
			}
			bool flag5 = PlayableTemplar.templarGrenade;
			if (flag5)
			{
				PrefabAPI.RegisterNetworkPrefab(PlayableTemplar.templarGrenade, "C:\\Users\\rseid\\Documents\\ror2mods\\PlayableTemplar\\PlayableTemplar\\PlayableTemplar.cs", "RegisterTemplar", 721);
			}
			bool flag6 = PlayableTemplar.templarRocket;
			if (flag6)
			{
				PrefabAPI.RegisterNetworkPrefab(PlayableTemplar.templarRocket, "C:\\Users\\rseid\\Documents\\ror2mods\\PlayableTemplar\\PlayableTemplar\\PlayableTemplar.cs", "RegisterTemplar", 722);
			}
			bool flag7 = this.templarRocketEffect;
			if (flag7)
			{
				PrefabAPI.RegisterNetworkPrefab(this.templarRocketEffect, "C:\\Users\\rseid\\Documents\\ror2mods\\PlayableTemplar\\PlayableTemplar\\PlayableTemplar.cs", "RegisterTemplar", 723);
			}
			ProjectileCatalog.getAdditionalEntries += delegate (List<GameObject> list)
			{
				list.Add(PlayableTemplar.templarGrenade);
				list.Add(PlayableTemplar.templarRocket);
			};
			this.SkinSetup();
			gameObject5.SetActive(false);
			string text = "The Clay Templar is a slow, tanky bruiser who uses the many weapons in his arsenal to mow down his opposition with ease.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
			text = text + "< ! > Minigun takes time to rev up, but inflicts heavy damage at a high rate of fire." + Environment.NewLine + Environment.NewLine;
			text = text + "< ! > Let Blunderbuss reload all 4 shots and unload them all at once for a big burst of damage." + Environment.NewLine + Environment.NewLine;
			text = text + "< ! > Use Bazooka after applying tar to deal massive AoE damage with chain explosions." + Environment.NewLine + Environment.NewLine;
			bool value3 = PlayableTemplar.enableRocketJump.Value;
			if (value3)
			{
				text = text + "< ! > The explosion force from Bazooka can be used to launch yourself high up with good timing.</color>" + Environment.NewLine;
			}
			else
			{
				text = text + "< ! > Tar Rifle is an all around good weapon in most cases, but lacks the high damage of Minigun.</color>" + Environment.NewLine;
			}
			LanguageAPI.Add("TEMPLAR_NAME", "Clay Templar");
			LanguageAPI.Add("TEMPLAR_DESCRIPTION", text);
			LanguageAPI.Add("TEMPLAR_SUBTITLE", "Lost Soldier of Aphelia");
			LanguageAPI.Add("TEMPLAR_LORE", "\n''le minigunne man :-DDD\n\n");
			LanguageAPI.Add("TEMPLAR_OUTRO_FLAVOR", "..and so it left, reveling in its triumph.");
			component.name = "TemplarBody";
			component.baseNameToken = "TEMPLAR_NAME";
			component.subtitleNameToken = "TEMPLAR_SUBTITLE";
			component.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/SimpleDotCrosshair");
			component.baseMaxHealth = PlayableTemplar.baseHealth.Value;
			component.levelMaxHealth = PlayableTemplar.healthGrowth.Value;
			component.baseRegen = PlayableTemplar.baseRegen.Value;
			component.levelRegen = PlayableTemplar.regenGrowth.Value;
			component.baseDamage = PlayableTemplar.baseDamage.Value;
			component.levelDamage = PlayableTemplar.damageGrowth.Value;
			component.baseArmor = PlayableTemplar.baseArmor.Value;
			component.baseCrit = 1f;
			SfxLocator componentInChildren3 = this.myCharacter.GetComponentInChildren<SfxLocator>();
			componentInChildren3.fallDamageSound = "Play_char_land_fall_damage";
			this.myCharacter.GetComponent<DeathRewards>().logUnlockableName = "";
			SurvivorDef survivorDef = new SurvivorDef
			{
				name = "TEMPLAR_NAME",
				unlockableName = "",
				descriptionToken = "TEMPLAR_DESCRIPTION",
				primaryColor = PlayableTemplar.CHAR_COLOR,
				bodyPrefab = this.myCharacter,
				displayPrefab = this.characterDisplay,
				outroFlavorToken = "TEMPLAR_OUTRO_FLAVOR"
			};
			SurvivorAPI.AddSurvivor(survivorDef);
			this.SkillSetup();
			BodyCatalog.getAdditionalEntries += delegate (List<GameObject> list)
			{
				list.Add(this.myCharacter);
			};
			this.CreateMaster();
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000995C File Offset: 0x00007B5C
		private void SkinSetup()
		{
			GameObject gameObject = this.myCharacter.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
			CharacterModel component = gameObject.GetComponent<CharacterModel>();
			bool flag = gameObject.GetComponent<ModelSkinController>();
			ModelSkinController modelSkinController;
			if (flag)
			{
				modelSkinController = gameObject.AddComponent<ModelSkinController>();
			}
			else
			{
				modelSkinController = gameObject.AddComponent<ModelSkinController>();
			}
			ChildLocator component2 = gameObject.GetComponent<ChildLocator>();
			bool flag2 = Reflection.GetFieldValue<SkinnedMeshRenderer>(component, "mainSkinnedMeshRenderer") == null;
			if (flag2)
			{
				for (int i = 0; i < component.baseRendererInfos.Length; i++)
				{
					bool flag3 = component.baseRendererInfos[i].renderer.gameObject.GetComponent<SkinnedMeshRenderer>();
					if (flag3)
					{
						Reflection.SetFieldValue<SkinnedMeshRenderer>(component, "mainSkinnedMeshRenderer", component.baseRendererInfos[i].renderer.gameObject.GetComponent<SkinnedMeshRenderer>());
					}
				}
			}
			SkinnedMeshRenderer fieldValue = Reflection.GetFieldValue<SkinnedMeshRenderer>(component, "mainSkinnedMeshRenderer");
			GameObject gameObject2 = null;
			GameObject gameObject3 = null;
			GameObject gameObject4 = null;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.myCharacter.GetComponentsInChildren<SkinnedMeshRenderer>())
			{
				bool flag4 = skinnedMeshRenderer && skinnedMeshRenderer.gameObject.name == "ClayBruiserChestArmorMesh";
				if (flag4)
				{
					gameObject2 = skinnedMeshRenderer.gameObject;
				}
				bool flag5 = skinnedMeshRenderer && skinnedMeshRenderer.gameObject.name == "ClayBruiserHeadMesh";
				if (flag5)
				{
					gameObject3 = skinnedMeshRenderer.gameObject;
				}
			}
			foreach (MeshRenderer meshRenderer in this.myCharacter.GetComponentsInChildren<MeshRenderer>())
			{
				bool flag6 = meshRenderer && meshRenderer.gameObject.name == "VagabondHead";
				if (flag6)
				{
					gameObject4 = meshRenderer.gameObject;
				}
			}
			LanguageAPI.Add("TEMPLARBODY_DEFAULT_SKIN_NAME", "Default");
			LanguageAPI.Add("TEMPLARBODY_ALT_SKIN_NAME", "Vagabond");
			LoadoutAPI.SkinDefInfo skinDefInfo = default(LoadoutAPI.SkinDefInfo);
			skinDefInfo.BaseSkins = Array.Empty<SkinDef>();
			skinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
			skinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
			skinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[]
			{
				new SkinDef.GameObjectActivation
				{
					gameObject = gameObject2,
					shouldActivate = true
				},
				new SkinDef.GameObjectActivation
				{
					gameObject = gameObject3,
					shouldActivate = true
				},
				new SkinDef.GameObjectActivation
				{
					gameObject = gameObject4,
					shouldActivate = false
				}
			};
			skinDefInfo.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.61f, 0.36f, 0.23f), new Color(0f, 0f, 0f), new Color(0.89f, 0.7f, 0.17f), new Color(0.52f, 0.21f, 0.21f));
			skinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
			{
				new SkinDef.MeshReplacement
				{
					renderer = fieldValue,
					mesh = fieldValue.sharedMesh
				}
			};
			skinDefInfo.Name = "TEMPLARBODY_DEFAULT_SKIN_NAME";
			skinDefInfo.NameToken = "TEMPLARBODY_DEFAULT_SKIN_NAME";
			skinDefInfo.RendererInfos = component.baseRendererInfos;
			skinDefInfo.RootObject = gameObject;
			skinDefInfo.UnlockableName = "";
			SkinDef skinDef = LoadoutAPI.CreateNewSkinDef(skinDefInfo);
			LoadoutAPI.SkinDefInfo skinDefInfo2 = default(LoadoutAPI.SkinDefInfo);
			skinDefInfo2.BaseSkins = Array.Empty<SkinDef>();
			skinDefInfo2.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
			skinDefInfo2.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];
			skinDefInfo2.GameObjectActivations = new SkinDef.GameObjectActivation[]
			{
				new SkinDef.GameObjectActivation
				{
					gameObject = gameObject2,
					shouldActivate = false
				},
				new SkinDef.GameObjectActivation
				{
					gameObject = gameObject3,
					shouldActivate = false
				},
				new SkinDef.GameObjectActivation
				{
					gameObject = gameObject4,
					shouldActivate = true
				}
			};
			skinDefInfo2.Icon = LoadoutAPI.CreateSkinIcon(new Color(0.66f, 0.41f, 0.29f), new Color(0f, 0f, 0f), new Color(0.46f, 0.25f, 0.02f), new Color(0.47f, 0.16f, 0.16f));
			skinDefInfo2.MeshReplacements = new SkinDef.MeshReplacement[]
			{
				new SkinDef.MeshReplacement
				{
					renderer = fieldValue,
					mesh = fieldValue.sharedMesh
				}
			};
			skinDefInfo2.Name = "TEMPLARBODY_ALT_SKIN_NAME";
			skinDefInfo2.NameToken = "TEMPLARBODY_ALT_SKIN_NAME";
			skinDefInfo2.RendererInfos = component.baseRendererInfos;
			skinDefInfo2.RootObject = gameObject;
			skinDefInfo2.UnlockableName = "TEMPLAR_MONSOONUNLOCKABLE_REWARD_ID";
			SkinDef skinDef2 = LoadoutAPI.CreateNewSkinDef(skinDefInfo2);
			modelSkinController.skins = new SkinDef[]
			{
				skinDef,
				skinDef2
			};
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000D8A0 File Offset: 0x0000BAA0
		private GameObject LoadDisplay(string name)
		{
			bool flag = PlayableTemplar.itemDisplayPrefabs.ContainsKey(name.ToLower());
			GameObject result;
			if (flag)
			{
				result = PlayableTemplar.itemDisplayPrefabs[name.ToLower()];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000D8DC File Offset: 0x0000BADC
		private void PopulateDisplays()
		{
			ItemDisplayRuleSet itemDisplayRuleSet = Resources.Load<GameObject>("Prefabs/CharacterBodies/MageBody").GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet;
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			ItemDisplayRuleSet.NamedRuleGroup[] array = typeof(ItemDisplayRuleSet).GetField("namedItemRuleGroups", bindingAttr).GetValue(itemDisplayRuleSet) as ItemDisplayRuleSet.NamedRuleGroup[];
			ItemDisplayRuleSet.NamedRuleGroup[] array2 = typeof(ItemDisplayRuleSet).GetField("namedEquipmentRuleGroups", bindingAttr).GetValue(itemDisplayRuleSet) as ItemDisplayRuleSet.NamedRuleGroup[];
			ItemDisplayRuleSet.NamedRuleGroup[] array3 = array;
			for (int i = 0; i < array3.Length; i++)
			{
				ItemDisplayRule[] rules = array3[i].displayRuleGroup.rules;
				for (int j = 0; j < rules.Length; j++)
				{
					GameObject followerPrefab = rules[j].followerPrefab;
					bool flag = !(followerPrefab == null);
					if (flag)
					{
						string name = followerPrefab.name;
						string key = (name != null) ? name.ToLower() : null;
						bool flag2 = !PlayableTemplar.itemDisplayPrefabs.ContainsKey(key);
						if (flag2)
						{
							PlayableTemplar.itemDisplayPrefabs[key] = followerPrefab;
						}
					}
				}
			}
			array3 = array2;
			for (int k = 0; k < array3.Length; k++)
			{
				ItemDisplayRule[] rules2 = array3[k].displayRuleGroup.rules;
				for (int l = 0; l < rules2.Length; l++)
				{
					GameObject followerPrefab2 = rules2[l].followerPrefab;
					bool flag3 = !(followerPrefab2 == null);
					if (flag3)
					{
						string name2 = followerPrefab2.name;
						string key2 = (name2 != null) ? name2.ToLower() : null;
						bool flag4 = !PlayableTemplar.itemDisplayPrefabs.ContainsKey(key2);
						if (flag4)
						{
							PlayableTemplar.itemDisplayPrefabs[key2] = followerPrefab2;
						}
					}
				}
			}
		}


		// Token: 0x060000BF RID: 191 RVA: 0x0000DC5C File Offset: 0x0000BE5C
		private void SkillSetup()
		{
			foreach (GenericSkill obj in this.myCharacter.GetComponentsInChildren<GenericSkill>())
			{
				Object.DestroyImmediate(obj);
			}
			this.PassiveSetup();
			this.PrimarySetup();
			this.SecondarySetup();
			this.UtilitySetup();
			this.SpecialSetup();
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000DCB8 File Offset: 0x0000BEB8
		private void PassiveSetup()
		{
			SkillLocator component = this.myCharacter.GetComponent<SkillLocator>();
			LanguageAPI.Add("TEMPLAR_PASSIVE_NAME", "Volatile Tar");
			LanguageAPI.Add("TEMPLAR_PASSIVE_DESCRIPTION", "Certain attacks cover enemies in <style=cIsDamage>tar</style>, <style=cIsUtility>slowing</style> them. <style=cIsDamage>Ignite</style> the <style=cIsDamage>tar</style> to create an <style=cIsDamage>explosion</style> that leaves enemies <style=cIsDamage>Scorched</style>, <style=cIsHealth>reducing their armor</style>.");
			component.passiveSkill.enabled = true;
			component.passiveSkill.skillNameToken = "TEMPLAR_PASSIVE_NAME";
			component.passiveSkill.skillDescriptionToken = "TEMPLAR_PASSIVE_DESCRIPTION";
			component.passiveSkill.icon = Assets.iconP;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000DD30 File Offset: 0x0000BF30
		private void PrimarySetup()
		{
			SkillLocator component = this.myCharacter.GetComponent<SkillLocator>();
			LanguageAPI.Add("KEYWORD_RAPIDFIRE", "<style=cKeywordName>Rapidfire</style><style=cSub><style=cIsDamage>Rate of fire</style> increases the longer the skill is held.</style></style>");
			LanguageAPI.Add("KEYWORD_EXPLOSIVE", "<style=cKeywordName>Explosive</style><style=cSub><style=cIsDamage>Ignite</style> <style=cIsUtility>tarred enemies</style>, creating an <style=cIsDamage>explosion</style> for <style=cIsDamage>20% of the damage dealt</style> and <style=cIsHealth>reducing their armor</style>.</style></style>");
			string text = "<style=cIsDamage>Rapidfire</style>. Rev up and fire your <style=cIsUtility>minigun</style>, dealing <style=cIsDamage>" + PlayableTemplar.minigunDamageCoefficient.Value * 100f + "% damage</style> per bullet. <style=cIsUtility>Slow</style> your movement while shooting, but gain <style=cIsHealing>bonus armor</style>.";
			LanguageAPI.Add("TEMPLAR_PRIMARY_MINIGUN_NAME", "Minigun");
			LanguageAPI.Add("TEMPLAR_PRIMARY_MINIGUN_DESCRIPTION", text);
			SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();
			skillDef.activationState = new SerializableEntityStateType(typeof(TemplarMinigunSpinUp));
			skillDef.activationStateMachineName = "Weapon";
			skillDef.baseMaxStock = 1;
			skillDef.baseRechargeInterval = 0f;
			skillDef.beginSkillCooldownOnSkillEnd = true;
			skillDef.canceledFromSprinting = false;
			skillDef.fullRestockOnAssign = true;
			skillDef.interruptPriority = InterruptPriority.Any;
			skillDef.isBullets = false;
			skillDef.isCombatSkill = true;
			skillDef.mustKeyPress = false;
			skillDef.noSprint = true;
			skillDef.rechargeStock = 1;
			skillDef.requiredStock = 1;
			skillDef.shootDelay = 0f;
			skillDef.stockToConsume = 1;
			skillDef.icon = Assets.icon1;
			skillDef.skillDescriptionToken = "TEMPLAR_PRIMARY_MINIGUN_DESCRIPTION";
			skillDef.skillName = "TEMPLAR_PRIMARY_MINIGUN_NAME";
			skillDef.skillNameToken = "TEMPLAR_PRIMARY_MINIGUN_NAME";
			skillDef.keywordTokens = new string[]
			{
				"KEYWORD_RAPIDFIRE"
			};
			LoadoutAPI.AddSkillDef(skillDef);
			component.primary = this.myCharacter.AddComponent<GenericSkill>();
			SkillFamily skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
			skillFamily.variants = new SkillFamily.Variant[1];
			LoadoutAPI.AddSkillFamily(skillFamily);
			Reflection.SetFieldValue<SkillFamily>(component.primary, "_skillFamily", skillFamily);
			SkillFamily skillFamily2 = component.primary.skillFamily;
			skillFamily2.variants[0] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				unlockableName = "",
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
			text = "Fire a <style=cIsUtility>tar rifle</style>, dealing <style=cIsDamage>" + PlayableTemplar.rifleDamageCoefficient.Value * 100f + "% damage</style> per bullet and applying <style=cIsUtility>tar</style> with high accuracy.";
			LanguageAPI.Add("TEMPLAR_PRIMARY_PRECISEMINIGUN_NAME", "Tar Rifle");
			LanguageAPI.Add("TEMPLAR_PRIMARY_PRECISEMINIGUN_DESCRIPTION", text);
			skillDef = ScriptableObject.CreateInstance<SkillDef>();
			skillDef.activationState = new SerializableEntityStateType(typeof(TemplarRifleFire));
			skillDef.activationStateMachineName = "Weapon";
			skillDef.baseMaxStock = 1;
			skillDef.baseRechargeInterval = 0f;
			skillDef.beginSkillCooldownOnSkillEnd = true;
			skillDef.canceledFromSprinting = true;
			skillDef.fullRestockOnAssign = true;
			skillDef.interruptPriority = InterruptPriority.Any;
			skillDef.isBullets = true;
			skillDef.isCombatSkill = true;
			skillDef.mustKeyPress = false;
			skillDef.noSprint = true;
			skillDef.rechargeStock = 1;
			skillDef.requiredStock = 1;
			skillDef.shootDelay = 0f;
			skillDef.stockToConsume = 1;
			skillDef.icon = Assets.icon1b;
			skillDef.skillDescriptionToken = "TEMPLAR_PRIMARY_PRECISEMINIGUN_DESCRIPTION";
			skillDef.skillName = "TEMPLAR_PRIMARY_PRECISEMINIGUN_NAME";
			skillDef.skillNameToken = "TEMPLAR_PRIMARY_PRECISEMINIGUN_NAME";
			LoadoutAPI.AddSkillDef(skillDef);
			Array.Resize<SkillFamily.Variant>(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
			skillFamily2.variants[skillFamily2.variants.Length - 1] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				unlockableName = "",
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
			bool value = PlayableTemplar.bazookaGoBoom.Value;
			if (value)
			{
				text = "<style=cIsDamage>Explosive</style>. Fire a <style=cIsUtility>rocket</style>, dealing <style=cIsDamage>" + PlayableTemplar.miniBazookaDamageCoefficient.Value * 100f + "% damage</style>.";
				LanguageAPI.Add("TEMPLAR_PRIMARY_BAZOOKA_NAME", "Bazooka Mk. 2");
				LanguageAPI.Add("TEMPLAR_PRIMARY_BAZOOKA_DESCRIPTION", text);
				skillDef = ScriptableObject.CreateInstance<SkillDef>();
				skillDef.activationState = new SerializableEntityStateType(typeof(TemplarChargeMiniRocket));
				skillDef.activationStateMachineName = "Weapon";
				skillDef.baseMaxStock = 1;
				skillDef.baseRechargeInterval = 0.1f;
				skillDef.beginSkillCooldownOnSkillEnd = true;
				skillDef.canceledFromSprinting = false;
				skillDef.fullRestockOnAssign = true;
				skillDef.interruptPriority = InterruptPriority.Any;
				skillDef.isBullets = true;
				skillDef.isCombatSkill = true;
				skillDef.mustKeyPress = false;
				skillDef.noSprint = true;
				skillDef.rechargeStock = 1;
				skillDef.requiredStock = 1;
				skillDef.shootDelay = 0f;
				skillDef.stockToConsume = 1;
				skillDef.icon = Assets.icon4;
				skillDef.skillDescriptionToken = "TEMPLAR_PRIMARY_BAZOOKA_DESCRIPTION";
				skillDef.skillName = "TEMPLAR_PRIMARY_BAZOOKA_NAME";
				skillDef.skillNameToken = "TEMPLAR_PRIMARY_BAZOOKA_NAME";
				skillDef.keywordTokens = new string[]
				{
					"KEYWORD_EXPLOSIVE"
				};
				LoadoutAPI.AddSkillDef(skillDef);
				Array.Resize<SkillFamily.Variant>(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
				skillFamily2.variants[skillFamily2.variants.Length - 1] = new SkillFamily.Variant
				{
					skillDef = skillDef,
					unlockableName = "",
					viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
				};
			}
			else
			{
				LanguageAPI.Add("TEMPLAR_PRIMARY_RAILGUN_NAME", "Railgun");
				LanguageAPI.Add("TEMPLAR_PRIMARY_RAILGUN_DESCRIPTION", "<style=cIsDamage>Explosive</style>. Charge up and fire a <style=cIsUtility>piercing bullet</style>, dealing <style=cIsDamage>800% damage</style>.");
				skillDef = ScriptableObject.CreateInstance<SkillDef>();
				skillDef.activationState = new SerializableEntityStateType(typeof(TemplarChargeBeam));
				skillDef.activationStateMachineName = "Weapon";
				skillDef.baseMaxStock = 1;
				skillDef.baseRechargeInterval = 0f;
				skillDef.beginSkillCooldownOnSkillEnd = true;
				skillDef.canceledFromSprinting = false;
				skillDef.fullRestockOnAssign = true;
				skillDef.interruptPriority = InterruptPriority.Any;
				skillDef.isBullets = true;
				skillDef.isCombatSkill = true;
				skillDef.mustKeyPress = false;
				skillDef.noSprint = true;
				skillDef.rechargeStock = 1;
				skillDef.requiredStock = 1;
				skillDef.shootDelay = 0f;
				skillDef.stockToConsume = 1;
				skillDef.icon = Assets.icon1c;
				skillDef.skillDescriptionToken = "TEMPLAR_PRIMARY_RAILGUN_DESCRIPTION";
				skillDef.skillName = "TEMPLAR_PRIMARY_RAILGUN_NAME";
				skillDef.skillNameToken = "TEMPLAR_PRIMARY_RAILGUN_NAME";
				skillDef.keywordTokens = new string[]
				{
					"KEYWORD_EXPLOSIVE"
				};
				LoadoutAPI.AddSkillDef(skillDef);
				Array.Resize<SkillFamily.Variant>(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
				skillFamily2.variants[skillFamily2.variants.Length - 1] = new SkillFamily.Variant
				{
					skillDef = skillDef,
					unlockableName = "",
					viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
				};
			}
			LanguageAPI.Add("TEMPLAR_PRIMARY_FLAMETHROWER_NAME", "Flamethrower");
			LanguageAPI.Add("TEMPLAR_PRIMARY_FLAMETHROWER_DESCRIPTION", "<style=cIsDamage>Explosive</style>. Fire a <style=cIsUtility>continuous stream of flames</style>, dealing <style=cIsDamage>1250% damage per second</style>.");
			skillDef = ScriptableObject.CreateInstance<SkillDef>();
			skillDef.activationState = new SerializableEntityStateType(typeof(TemplarFlamethrower));
			skillDef.activationStateMachineName = "Weapon";
			skillDef.baseMaxStock = 1;
			skillDef.baseRechargeInterval = 0f;
			skillDef.beginSkillCooldownOnSkillEnd = true;
			skillDef.canceledFromSprinting = false;
			skillDef.fullRestockOnAssign = true;
			skillDef.interruptPriority = InterruptPriority.Any;
			skillDef.isBullets = true;
			skillDef.isCombatSkill = true;
			skillDef.mustKeyPress = false;
			skillDef.noSprint = true;
			skillDef.rechargeStock = 1;
			skillDef.requiredStock = 1;
			skillDef.shootDelay = 0f;
			skillDef.stockToConsume = 1;
			skillDef.icon = Assets.icon1d;
			skillDef.skillDescriptionToken = "TEMPLAR_PRIMARY_FLAMETHROWER_DESCRIPTION";
			skillDef.skillName = "TEMPLAR_PRIMARY_FLAMETHROWER_NAME";
			skillDef.skillNameToken = "TEMPLAR_PRIMARY_FLAMETHROWER_NAME";
			skillDef.keywordTokens = new string[]
			{
				"KEYWORD_EXPLOSIVE"
			};
			LoadoutAPI.AddSkillDef(skillDef);
			Array.Resize<SkillFamily.Variant>(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
			skillFamily2.variants[skillFamily2.variants.Length - 1] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				unlockableName = "",
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000E4BC File Offset: 0x0000C6BC
		private void SecondarySetup()
		{
			SkillLocator component = this.myCharacter.GetComponent<SkillLocator>();
			string text = "Throw a <style=cIsUtility>clay bomb</style> that explodes for <style=cIsDamage>" + PlayableTemplar.clayGrenadeDamageCoefficient.Value * 100f + "% damage</style> and inflicts <style=cIsUtility>tar</style>.";
			bool flag = PlayableTemplar.clayGrenadeStock.Value > 1;
			if (flag)
			{
				text = string.Concat(new object[]
				{
					text,
					" Hold up to <style=cIsDamage>",
					PlayableTemplar.clayGrenadeStock.Value,
					"</style> bombs."
				});
			}
			LanguageAPI.Add("TEMPLAR_SECONDARY_GRENADE_NAME", "Clay Bomb");
			LanguageAPI.Add("TEMPLAR_SECONDARY_GRENADE_DESCRIPTION", text);
			SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();
			skillDef.activationState = new SerializableEntityStateType(typeof(TemplarThrowClaybomb));
			skillDef.activationStateMachineName = "Weapon";
			skillDef.baseMaxStock = PlayableTemplar.clayGrenadeStock.Value;
			skillDef.baseRechargeInterval = PlayableTemplar.clayGrenadeCooldown.Value;
			skillDef.beginSkillCooldownOnSkillEnd = false;
			skillDef.canceledFromSprinting = false;
			skillDef.fullRestockOnAssign = true;
			skillDef.interruptPriority = InterruptPriority.Skill;
			skillDef.isBullets = false;
			skillDef.isCombatSkill = true;
			skillDef.mustKeyPress = false;
			skillDef.noSprint = true;
			skillDef.rechargeStock = 1;
			skillDef.requiredStock = 1;
			skillDef.shootDelay = 0f;
			skillDef.stockToConsume = 1;
			skillDef.icon = Assets.icon2;
			skillDef.skillDescriptionToken = "TEMPLAR_SECONDARY_GRENADE_DESCRIPTION";
			skillDef.skillName = "TEMPLAR_SECONDARY_GRENADE_NAME";
			skillDef.skillNameToken = "TEMPLAR_SECONDARY_GRENADE_NAME";
			LoadoutAPI.AddSkillDef(skillDef);
			component.secondary = this.myCharacter.AddComponent<GenericSkill>();
			SkillFamily skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
			skillFamily.variants = new SkillFamily.Variant[1];
			LoadoutAPI.AddSkillFamily(skillFamily);
			Reflection.SetFieldValue<SkillFamily>(component.secondary, "_skillFamily", skillFamily);
			SkillFamily skillFamily2 = component.secondary.skillFamily;
			skillFamily2.variants[0] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				unlockableName = "",
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
			text = string.Concat(new object[]
			{
				"Fire a burst of <style=cIsUtility>pellets</style>, dealing <style=cIsDamage>",
				PlayableTemplar.blunderbussPelletCount.Value,
				"x",
				PlayableTemplar.blunderbussDamageCoefficient.Value * 100f,
				"% damage</style>."
			});
			bool flag2 = PlayableTemplar.blunderbussStock.Value > 1;
			if (flag2)
			{
				text = string.Concat(new object[]
				{
					text,
					" Store up to ",
					PlayableTemplar.blunderbussStock.Value,
					" shots."
				});
			}
			LanguageAPI.Add("TEMPLAR_SECONDARY_SHOTGUN_NAME", "Blunderbuss");
			LanguageAPI.Add("TEMPLAR_SECONDARY_SHOTGUN_DESCRIPTION", text);
			skillDef = ScriptableObject.CreateInstance<SkillDef>();
			skillDef.activationState = new SerializableEntityStateType(typeof(TemplarShotgun));
			skillDef.activationStateMachineName = "Weapon";
			skillDef.baseMaxStock = PlayableTemplar.blunderbussStock.Value;
			skillDef.baseRechargeInterval = PlayableTemplar.blunderbussCooldown.Value;
			skillDef.beginSkillCooldownOnSkillEnd = false;
			skillDef.canceledFromSprinting = false;
			skillDef.fullRestockOnAssign = true;
			skillDef.interruptPriority = InterruptPriority.Skill;
			skillDef.isBullets = false;
			skillDef.isCombatSkill = true;
			skillDef.mustKeyPress = false;
			skillDef.noSprint = true;
			skillDef.rechargeStock = 1;
			skillDef.requiredStock = 1;
			skillDef.shootDelay = 0f;
			skillDef.stockToConsume = 1;
			skillDef.icon = Assets.icon2b;
			skillDef.skillDescriptionToken = "TEMPLAR_SECONDARY_SHOTGUN_DESCRIPTION";
			skillDef.skillName = "TEMPLAR_SECONDARY_SHOTGUN_NAME";
			skillDef.skillNameToken = "TEMPLAR_SECONDARY_SHOTGUN_NAME";
			LoadoutAPI.AddSkillDef(skillDef);
			Array.Resize<SkillFamily.Variant>(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
			skillFamily2.variants[skillFamily2.variants.Length - 1] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				unlockableName = "",
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000E898 File Offset: 0x0000CA98
		private void UtilitySetup()
		{
			SkillLocator component = this.myCharacter.GetComponent<SkillLocator>();
			LanguageAPI.Add("TEMPLAR_UTILITY_OVERDRIVE_NAME", "Tar Overdrive");
			LanguageAPI.Add("TEMPLAR_UTILITY_OVERDRIVE_DESCRIPTION", "Shift into <style=cIsUtility>maximum overdrive</style>, knocking enemies back and applying <style=cIsUtility>tar</style>. <style=cIsUtility>Gain bonus attack speed and health regen for 3 seconds.</style>");
			SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();
			skillDef.activationState = new SerializableEntityStateType(typeof(TemplarOverdrive));
			skillDef.activationStateMachineName = "Body";
			skillDef.baseMaxStock = PlayableTemplar.overdriveStock.Value;
			skillDef.baseRechargeInterval = PlayableTemplar.overdriveCooldown.Value;
			skillDef.beginSkillCooldownOnSkillEnd = false;
			skillDef.canceledFromSprinting = false;
			skillDef.fullRestockOnAssign = true;
			skillDef.interruptPriority = InterruptPriority.PrioritySkill;
			skillDef.isBullets = false;
			skillDef.isCombatSkill = true;
			skillDef.mustKeyPress = true;
			skillDef.noSprint = false;
			skillDef.rechargeStock = 1;
			skillDef.requiredStock = 1;
			skillDef.shootDelay = 0f;
			skillDef.stockToConsume = 1;
			skillDef.icon = Assets.icon3;
			skillDef.skillDescriptionToken = "TEMPLAR_UTILITY_OVERDRIVE_DESCRIPTION";
			skillDef.skillName = "TEMPLAR_UTILITY_OVERDRIVE_NAME";
			skillDef.skillNameToken = "TEMPLAR_UTILITY_OVERDRIVE_NAME";
			LoadoutAPI.AddSkillDef(skillDef);
			component.utility = this.myCharacter.AddComponent<GenericSkill>();
			SkillFamily skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
			skillFamily.variants = new SkillFamily.Variant[1];
			LoadoutAPI.AddSkillFamily(skillFamily);
			Reflection.SetFieldValue<SkillFamily>(component.utility, "_skillFamily", skillFamily);
			SkillFamily skillFamily2 = component.utility.skillFamily;
			skillFamily2.variants[0] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				unlockableName = "",
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
			string text = "<style=cIsUtility>Dash</style> a short distance. Can dash while shooting.";
			bool flag = PlayableTemplar.dashStock.Value > 1;
			if (flag)
			{
				text = string.Concat(new object[]
				{
					text,
					" Store up to ",
					PlayableTemplar.dashStock.Value,
					" charges."
				});
			}
			LanguageAPI.Add("TEMPLAR_UTILITY_DODGE_NAME", "Sidestep");
			LanguageAPI.Add("TEMPLAR_UTILITY_DODGE_DESCRIPTION", text);
			SkillDef skillDef2 = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<SkillLocator>().utility.skillFamily.variants[0].skillDef;
			skillDef = ScriptableObject.CreateInstance<SkillDef>();
			skillDef.activationState = new SerializableEntityStateType(typeof(TemplarSidestep));
			skillDef.activationStateMachineName = "Body";
			skillDef.baseRechargeInterval = PlayableTemplar.dashCooldown.Value;
			skillDef.baseMaxStock = PlayableTemplar.dashStock.Value;
			skillDef.beginSkillCooldownOnSkillEnd = skillDef2.beginSkillCooldownOnSkillEnd;
			skillDef.canceledFromSprinting = false;
			skillDef.fullRestockOnAssign = skillDef2.fullRestockOnAssign;
			skillDef.interruptPriority = skillDef2.interruptPriority;
			skillDef.isBullets = skillDef2.isBullets;
			skillDef.isCombatSkill = true;
			skillDef.mustKeyPress = skillDef2.mustKeyPress;
			skillDef.noSprint = false;
			skillDef.rechargeStock = skillDef2.rechargeStock;
			skillDef.requiredStock = skillDef2.requiredStock;
			skillDef.shootDelay = skillDef2.shootDelay;
			skillDef.stockToConsume = skillDef2.stockToConsume;
			bool value = PlayableTemplar.oldIcons.Value;
			if (value)
			{
				skillDef.icon = skillDef2.icon;
			}
			else
			{
				skillDef.icon = Assets.icon3b;
			}
			skillDef.skillDescriptionToken = "TEMPLAR_UTILITY_DODGE_DESCRIPTION";
			skillDef.skillName = "TEMPLAR_UTILITY_DODGE_NAME";
			skillDef.skillNameToken = "TEMPLAR_UTILITY_DODGE_NAME";
			LoadoutAPI.AddSkillDef(skillDef);
			Array.Resize<SkillFamily.Variant>(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
			skillFamily2.variants[skillFamily2.variants.Length - 1] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				unlockableName = "",
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000EC40 File Offset: 0x0000CE40
		private void SpecialSetup()
		{
			SkillLocator component = this.myCharacter.GetComponent<SkillLocator>();
			string text = "<style=cIsDamage>Explosive</style>. Fire a <style=cIsUtility>rocket</style>, dealing <style=cIsDamage>" + PlayableTemplar.bazookaDamageCoefficient.Value * 100f + "% damage</style>.";
			LanguageAPI.Add("TEMPLAR_SPECIAL_FIRE_NAME", "Bazooka");
			LanguageAPI.Add("TEMPLAR_SPECIAL_FIRE_DESCRIPTION", text);
			SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();
			skillDef.activationState = new SerializableEntityStateType(typeof(TemplarChargeRocket));
			skillDef.activationStateMachineName = "Weapon";
			skillDef.baseMaxStock = PlayableTemplar.bazookaStock.Value;
			skillDef.baseRechargeInterval = PlayableTemplar.bazookaCooldown.Value;
			skillDef.beginSkillCooldownOnSkillEnd = true;
			skillDef.canceledFromSprinting = false;
			skillDef.fullRestockOnAssign = true;
			skillDef.interruptPriority = InterruptPriority.PrioritySkill;
			skillDef.isBullets = true;
			skillDef.isCombatSkill = true;
			skillDef.mustKeyPress = true;
			skillDef.noSprint = true;
			skillDef.rechargeStock = 1;
			skillDef.requiredStock = 1;
			skillDef.shootDelay = 0f;
			skillDef.stockToConsume = 1;
			skillDef.icon = Assets.icon4;
			skillDef.skillDescriptionToken = "TEMPLAR_SPECIAL_FIRE_DESCRIPTION";
			skillDef.skillName = "TEMPLAR_SPECIAL_FIRE_NAME";
			skillDef.skillNameToken = "TEMPLAR_SPECIAL_FIRE_NAME";
			skillDef.keywordTokens = new string[]
			{
				"KEYWORD_EXPLOSIVE"
			};
			LoadoutAPI.AddSkillDef(skillDef);
			component.special = this.myCharacter.AddComponent<GenericSkill>();
			SkillFamily skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
			skillFamily.variants = new SkillFamily.Variant[1];
			LoadoutAPI.AddSkillFamily(skillFamily);
			Reflection.SetFieldValue<SkillFamily>(component.special, "_skillFamily", skillFamily);
			SkillFamily skillFamily2 = component.special.skillFamily;
			skillFamily2.variants[0] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				unlockableName = "",
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
			bool value = PlayableTemplar.disableWeaponSwap.Value;
			if (!value)
			{
				LanguageAPI.Add("TEMPLAR_SPECIAL_SWAP_NAME", "Weapon Swap");
				LanguageAPI.Add("TEMPLAR_SPECIAL_SWAP_DESCRIPTION", "<style=cIsUtility>Swap</style> your <style=cIsDamage>primary</style> weapon type.");
				skillDef = ScriptableObject.CreateInstance<SkillDef>();
				skillDef.activationState = new SerializableEntityStateType(typeof(TemplarSwapWeapon));
				skillDef.activationStateMachineName = "Weapon";
				skillDef.baseMaxStock = 1;
				skillDef.baseRechargeInterval = 0.1f;
				skillDef.beginSkillCooldownOnSkillEnd = true;
				skillDef.canceledFromSprinting = false;
				skillDef.fullRestockOnAssign = true;
				skillDef.interruptPriority = InterruptPriority.PrioritySkill;
				skillDef.isBullets = false;
				skillDef.isCombatSkill = false;
				skillDef.mustKeyPress = false;
				skillDef.noSprint = true;
				skillDef.rechargeStock = 1;
				skillDef.requiredStock = 1;
				skillDef.shootDelay = 0f;
				skillDef.stockToConsume = 1;
				skillDef.icon = Assets.icon4b;
				skillDef.skillDescriptionToken = "TEMPLAR_SPECIAL_SWAP_DESCRIPTION";
				skillDef.skillName = "TEMPLAR_SPECIAL_SWAP_NAME";
				skillDef.skillNameToken = "TEMPLAR_SPECIAL_SWAP_NAME";
				LoadoutAPI.AddSkillDef(skillDef);
				Array.Resize<SkillFamily.Variant>(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
				skillFamily2.variants[skillFamily2.variants.Length - 1] = new SkillFamily.Variant
				{
					skillDef = skillDef,
					unlockableName = "",
					viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
				};
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000EF68 File Offset: 0x0000D168
		private void CreateMaster()
		{
			this.doppelganger = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterMasters/CommandoMonsterMaster"), "TemplarMonsterMaster", true, "C:\\Users\\rseid\\Documents\\ror2mods\\PlayableTemplar\\PlayableTemplar\\PlayableTemplar.cs", "CreateMaster", 3280);
			this.doppelganger.GetComponent<CharacterMaster>().bodyPrefab = this.myCharacter;
			MasterCatalog.getAdditionalEntries += delegate (List<GameObject> list)
			{
				list.Add(this.doppelganger);
			};
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000EFC8 File Offset: 0x0000D1C8
		private void Masochism()
		{
			GameObject gameObject = Resources.Load<GameObject>("Prefabs/CharacterBodies/ClayBruiserBody");
			foreach (GenericSkill obj in gameObject.GetComponentsInChildren<GenericSkill>())
			{
				Object.DestroyImmediate(obj);
			}
			SkillLocator component = gameObject.GetComponent<SkillLocator>();
			SkillDef skillDef = this.myCharacter.GetComponentInChildren<SkillLocator>().primary.skillFamily.variants[0].skillDef;
			component.primary = gameObject.AddComponent<GenericSkill>();
			SkillFamily skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
			skillFamily.variants = new SkillFamily.Variant[1];
			LoadoutAPI.AddSkillFamily(skillFamily);
			Reflection.SetFieldValue<SkillFamily>(component.primary, "_skillFamily", skillFamily);
			SkillFamily skillFamily2 = component.primary.skillFamily;
			skillFamily2.variants[0] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				unlockableName = "",
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
			skillDef = this.myCharacter.GetComponentInChildren<SkillLocator>().special.skillFamily.variants[0].skillDef;
			component.secondary = gameObject.AddComponent<GenericSkill>();
			skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
			skillFamily.variants = new SkillFamily.Variant[1];
			LoadoutAPI.AddSkillFamily(skillFamily);
			Reflection.SetFieldValue<SkillFamily>(component.secondary, "_skillFamily", skillFamily);
			skillFamily2 = component.secondary.skillFamily;
			skillFamily2.variants[0] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				unlockableName = "",
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
			skillDef = this.myCharacter.GetComponentInChildren<SkillLocator>().secondary.skillFamily.variants[0].skillDef;
			component.utility = gameObject.AddComponent<GenericSkill>();
			skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
			skillFamily.variants = new SkillFamily.Variant[1];
			LoadoutAPI.AddSkillFamily(skillFamily);
			Reflection.SetFieldValue<SkillFamily>(component.utility, "_skillFamily", skillFamily);
			skillFamily2 = component.utility.skillFamily;
			skillFamily2.variants[0] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				unlockableName = "",
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
			skillDef = this.myCharacter.GetComponentInChildren<SkillLocator>().special.skillFamily.variants[0].skillDef;
			component.special = gameObject.AddComponent<GenericSkill>();
			skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
			skillFamily.variants = new SkillFamily.Variant[1];
			LoadoutAPI.AddSkillFamily(skillFamily);
			Reflection.SetFieldValue<SkillFamily>(component.special, "_skillFamily", skillFamily);
			skillFamily2 = component.special.skillFamily;
			skillFamily2.variants[0] = new SkillFamily.Variant
			{
				skillDef = skillDef,
				unlockableName = "",
				viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
			};
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000F2BD File Offset: 0x0000D4BD
		public PlayableTemplar()
		{
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000F2C8 File Offset: 0x0000D4C8
		// Note: this type is marked as 'beforefieldinit'.
		static PlayableTemplar()
		{
		}

		// Token: 0x040000DD RID: 221
		public const string MODUID = "com.rob.PlayableTemplar";

		// Token: 0x040000DE RID: 222
		internal static PlayableTemplar instance;

		// Token: 0x040000DF RID: 223
		public const string characterName = "Clay Templar";

		// Token: 0x040000E0 RID: 224
		public const string characterSubtitle = "Lost Soldier of Aphelia";

		// Token: 0x040000E1 RID: 225
		public const string characterOutro = "..and so it left, reveling in its triumph.";

		// Token: 0x040000E2 RID: 226
		public const string altOutro = "..and so it left, with a nerfed ass rifle.";

		// Token: 0x040000E3 RID: 227
		public const string characterLore = "\n''le minigunne man :-DDD\n\n";

		// Token: 0x040000E4 RID: 228
		public GameObject myCharacter;

		// Token: 0x040000E5 RID: 229
		public GameObject characterDisplay;

		// Token: 0x040000E6 RID: 230
		public GameObject doppelganger;

		// Token: 0x040000E7 RID: 231
		public static GameObject templarGrenade;

		// Token: 0x040000E8 RID: 232
		public static GameObject templarRocket;

		// Token: 0x040000E9 RID: 233
		public GameObject templarRocketEffect;


		// Token: 0x040000F3 RID: 243
		private static Dictionary<string, GameObject> itemDisplayPrefabs = new Dictionary<string, GameObject>();



		// Token: 0x02000025 RID: 37
		public class TemplarMenuAnim : MonoBehaviour
		{
			// Token: 0x060000EB RID: 235 RVA: 0x00010789 File Offset: 0x0000E989
			internal void OnEnable()
			{
				base.StartCoroutine(this.SpawnAnim());
			}

			// Token: 0x060000EC RID: 236 RVA: 0x0001079C File Offset: 0x0000E99C
			internal void OnDisable()
			{
				bool flag = this.playID > 0U;
				if (flag)
				{
					AkSoundEngine.StopPlayingID(this.playID);
				}
			}

			// Token: 0x060000ED RID: 237 RVA: 0x000107C3 File Offset: 0x0000E9C3
			private IEnumerator SpawnAnim()
			{
				Animator animator = base.GetComponentInChildren<Animator>();
				EffectManager.SpawnEffect(EntityStates.ClayBruiserMonster.SpawnState.spawnEffectPrefab, new EffectData
				{
					origin = base.gameObject.transform.position
				}, false);
				this.playID = Util.PlayScaledSound(EntityStates.ClayBruiserMonster.SpawnState.spawnSoundString, base.gameObject, 1.25f);
				this.PlayAnimation("Body", "Spawn", "Spawn.playbackRate", 1f, animator);
				animator.SetBool("WeaponIsReady", false);
				yield return 60f;
				animator.SetBool("WeaponIsReady", true);
				yield break;
			}

			// Token: 0x060000EE RID: 238 RVA: 0x000107D4 File Offset: 0x0000E9D4
			private void PlayAnimation(string layerName, string animationStateName, string playbackRateParam, float duration, Animator animator)
			{
				int layerIndex = animator.GetLayerIndex(layerName);
				animator.SetFloat(playbackRateParam, 1f);
				animator.PlayInFixedTime(animationStateName, layerIndex, 0f);
				animator.Update(0f);
				float length = animator.GetCurrentAnimatorStateInfo(layerIndex).length;
				animator.SetFloat(playbackRateParam, length / duration);
			}

			// Token: 0x060000EF RID: 239 RVA: 0x00010833 File Offset: 0x0000EA33
			public TemplarMenuAnim()
			{
			}

			// Token: 0x0400014E RID: 334
			private uint playID;
		}


	}
}
