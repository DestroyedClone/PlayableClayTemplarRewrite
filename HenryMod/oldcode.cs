using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using KinematicCharacterController;
using On.RoR2;
using R2API;
using R2API.Utils;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

namespace PlayableTemplar
{
	public class PlayableTemplar : BaseUnityPlugin
	{
		// Token: 0x060000AF RID: 175 RVA: 0x00007A08 File Offset: 0x00005C08
		public void Awake()
		{
			this.PopulateDisplays();
			this.RegisterTemplar();
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00008CC8 File Offset: 0x00006EC8
		private void RegisterTemplar()
		{
			this.myCharacter = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/ClayBruiserBody"), "TemplarBody", true, "C:\\Users\\rseid\\Documents\\ror2mods\\PlayableTemplar\\PlayableTemplar\\PlayableTemplar.cs", "RegisterTemplar", 468);
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
			ProjectileCatalog.getAdditionalEntries += delegate (List<GameObject> list)
			{
				list.Add(PlayableTemplar.templarGrenade);
				list.Add(PlayableTemplar.templarRocket);
			};
			this.SkinSetup();
			gameObject5.SetActive(false);
			LanguageAPI.Add("TEMPLAR_NAME", "Clay Templar");
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


		// Token: 0x040000E4 RID: 228
		public GameObject myCharacter;

		// Token: 0x040000E5 RID: 229
		public GameObject characterDisplay;

		// Token: 0x040000E6 RID: 230
		public GameObject doppelganger;



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
