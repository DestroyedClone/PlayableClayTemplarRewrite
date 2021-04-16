using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using KinematicCharacterController;
//using On.RoR2;
using R2API;
using R2API.Utils;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PlayableTemplar
{
	public class PlayableTemplar : BaseUnityPlugin
	{
		public void Awake()
		{
			this.RegisterTemplar();
		}

		private void RegisterTemplar()
		{
			this.myCharacter = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/ClayBruiserBody"), "TemplarBody", true, "C:\\Users\\rseid\\Documents\\ror2mods\\PlayableTemplar\\PlayableTemplar\\PlayableTemplar.cs", "RegisterTemplar", 468);
			CharacterBody component = this.myCharacter.GetComponent<CharacterBody>();
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


			this.characterDisplay = PrefabAPI.InstantiateClone(this.myCharacter.GetComponent<ModelLocator>().modelBaseTransform.gameObject, "TemplarDisplay", true, "C:\\Users\\rseid\\Documents\\ror2mods\\PlayableTemplar\\PlayableTemplar\\PlayableTemplar.cs", "RegisterTemplar", 712);
			this.characterDisplay.transform.localScale = Vector3.one * 0.8f;
			this.characterDisplay.AddComponent<PlayableTemplar.TemplarMenuAnim>();
			this.characterDisplay.AddComponent<NetworkIdentity>();
			component.name = "TemplarBody";
			component.baseNameToken = "TEMPLAR_NAME";
			component.subtitleNameToken = "TEMPLAR_SUBTITLE";
		}


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
		public GameObject myCharacter;
		public GameObject characterDisplay;
		public GameObject doppelganger;
		private static Dictionary<string, GameObject> itemDisplayPrefabs = new Dictionary<string, GameObject>();

		public class TemplarMenuAnim : MonoBehaviour
		{
			private uint playID;

			internal void OnEnable()
			{
				base.StartCoroutine(this.SpawnAnim());
			}

			internal void OnDisable()
			{
				bool flag = this.playID > 0U;
				if (flag)
				{
					AkSoundEngine.StopPlayingID(this.playID);
				}
			}

			private IEnumerator SpawnAnim()
			{
				Animator animator = base.GetComponentInChildren<Animator>();
				EffectManager.SpawnEffect(EntityStates.ClayBruiserMonster.SpawnState.spawnEffectPrefab, new EffectData
				{
					origin = base.gameObject.transform.position
				}, false);
				this.playID = Util.PlayAttackSpeedSound(EntityStates.ClayBruiserMonster.SpawnState.spawnSoundString, base.gameObject, 1.25f);
				this.PlayAnimation("Body", "Spawn", "Spawn.playbackRate", 1f, animator);
				animator.SetBool("WeaponIsReady", false);
				yield return 60f;
				animator.SetBool("WeaponIsReady", true);
				yield break;
			}

			private void PlayAnimation(string layerName, string animationStateName, string playbackRateParam, float duration, Animator animator)
			{
				int layerIndex = animator.GetLayerIndex(layerName);
				animator.SetFloat(playbackRateParam, 1f);
				animator.PlayInFixedTime(animationStateName, layerIndex, 0f);
				animator.Update(0f);
				float length = animator.GetCurrentAnimatorStateInfo(layerIndex).length;
				animator.SetFloat(playbackRateParam, length / duration);
			}
			public TemplarMenuAnim()
			{
			}

		}


	}
}
