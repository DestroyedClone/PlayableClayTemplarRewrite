using BepInEx.Configuration;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PlayableTemplar.Modules
{
    public static class Configg
	{
        #region variables
        public static ConfigEntry<bool> originalSize;

		public static ConfigEntry<bool> oldIcons;

		private static ConfigEntry<bool> bazookaGoBoom;

		private static ConfigEntry<bool> disableWeaponSwap;

		private static ConfigEntry<float> baseHealth;

		private static ConfigEntry<float> healthGrowth;

		private static ConfigEntry<float> baseArmor;

		private static ConfigEntry<float> baseDamage;

		private static ConfigEntry<float> damageGrowth;

		private static ConfigEntry<float> baseRegen;

		private static ConfigEntry<float> regenGrowth;

		public static ConfigEntry<float> minigunDamageCoefficient;

		public static ConfigEntry<float> minigunProcCoefficient;

		public static ConfigEntry<float> minigunForce;

		private static ConfigEntry<float> minigunArmorBoost;

		private static ConfigEntry<float> minigunStationaryArmorBoost;

		public static ConfigEntry<float> minigunMinFireRate;

		public static ConfigEntry<float> minigunMaxFireRate;

		public static ConfigEntry<float> minigunFireRateGrowth;

		public static ConfigEntry<float> rifleDamageCoefficient;

		public static ConfigEntry<float> rifleProcCoefficient;

		public static ConfigEntry<float> rifleForce;

		public static ConfigEntry<float> rifleFireRate;

		private static ConfigEntry<int> clayGrenadeStock;

		private static ConfigEntry<float> clayGrenadeCooldown;

		private static ConfigEntry<float> clayGrenadeDamageCoefficient;

		private static ConfigEntry<float> clayGrenadeProcCoefficient;

		private static ConfigEntry<float> clayGrenadeRadius;

		private static ConfigEntry<float> clayGrenadeDetonationTime;

		private static ConfigEntry<int> blunderbussStock;

		private static ConfigEntry<float> blunderbussCooldown;

		public static ConfigEntry<int> blunderbussPelletCount;

		public static ConfigEntry<float> blunderbussDamageCoefficient;

		public static ConfigEntry<float> blunderbussProcCoefficient;

		public static ConfigEntry<float> blunderbussSpread;

		private static ConfigEntry<int> tarStock;

		private static ConfigEntry<float> tarCooldown;

		private static ConfigEntry<int> overdriveStock;

		private static ConfigEntry<float> overdriveCooldown;

		private static ConfigEntry<int> dashStock;

		private static ConfigEntry<float> dashCooldown;

		private static ConfigEntry<int> bazookaStock;

		private static ConfigEntry<float> bazookaCooldown;

		public static ConfigEntry<float> bazookaDamageCoefficient;

		private static ConfigEntry<float> bazookaProcCoefficient;

		private static ConfigEntry<float> bazookaBlastRadius;

		public static ConfigEntry<float> miniBazookaDamageCoefficient;

		public static ConfigEntry<bool> jellyfishEvent;

		private static ConfigEntry<bool> enableDunestrider;

		private static ConfigEntry<bool> enableRocketJump;

		private static ConfigEntry<bool> masochism;
		#endregion
		public static void ReadConfig()
        {//base.config.bind
			originalSize = PlayableTemplarPlugin.instance.Config.Bind<bool>(new ConfigDefinition("01 - General Settings", "Original Size"), false, new ConfigDescription("Makes the Templar similar in size to the enemy Templars", null, Array.Empty<object>()));
			oldIcons = PlayableTemplarPlugin.instance.Config.Bind<bool>(new ConfigDefinition("01 - General Settings", "Old Skill Icons"), false, new ConfigDescription("Uses the old skill icons", null, Array.Empty<object>()));
			bazookaGoBoom = PlayableTemplarPlugin.instance.Config.Bind<bool>(new ConfigDefinition("01 - General Settings", "Bazooka go KABOOM"), false, new ConfigDescription("Enables Bazooka Mk.2", null, Array.Empty<object>()));
			disableWeaponSwap = PlayableTemplarPlugin.instance.Config.Bind<bool>(new ConfigDefinition("01 - General Settings", "Disable Weapon Swap"), false, new ConfigDescription("Disables this skill, for those who find it overpowered", null, Array.Empty<object>()));
			baseHealth = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - General Settings", "Health"), 200f, new ConfigDescription("Base health", null, Array.Empty<object>()));
			healthGrowth = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - General Settings", "Health growth"), 48f, new ConfigDescription("Health per level", null, Array.Empty<object>()));
			baseArmor = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - General Settings", "Armor"), 10f, new ConfigDescription("Base armor", null, Array.Empty<object>()));
			baseDamage = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - General Settings", "Damage"), 12f, new ConfigDescription("Base damage", null, Array.Empty<object>()));
			damageGrowth = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - General Settings", "Damage growth"), 2.4f, new ConfigDescription("Damage per level", null, Array.Empty<object>()));
			baseRegen = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - General Settings", "Regen"), 0.5f, new ConfigDescription("Base HP regen", null, Array.Empty<object>()));
			regenGrowth = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - General Settings", "Regen growth"), 0.5f, new ConfigDescription("HP regen per level", null, Array.Empty<object>()));
			minigunDamageCoefficient = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("02 - Minigun", "Damage"), 0.4f, new ConfigDescription("Minigun damage per bullet", null, Array.Empty<object>()));
			minigunProcCoefficient = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("02 - Minigun", "Proc Coefficient"), 0.4f, new ConfigDescription("Minigun proc coefficient per bullet", null, Array.Empty<object>()));
			minigunForce = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("02 - Minigun", "Force"), 3f, new ConfigDescription("Minigun bullet force", null, Array.Empty<object>()));
			minigunArmorBoost = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("02 - Minigun", "Armor Bonus"), 50f, new ConfigDescription("Bonus armor while firing", null, Array.Empty<object>()));
			minigunStationaryArmorBoost = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("02 - Minigun", "Stationary Armor Bonus"), 100f, new ConfigDescription("Bonus armor while standing still and firing", null, Array.Empty<object>()));
			minigunMinFireRate = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("02 - Minigun", "Minimum Fire Rate"), 0.75f, new ConfigDescription("Starting fire rate", null, Array.Empty<object>()));
			minigunMaxFireRate = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("02 - Minigun", "Maximum Fire Rate"), 1.35f, new ConfigDescription("Max fire rate", null, Array.Empty<object>()));
			minigunFireRateGrowth = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("02 - Minigun", "Fire Rate Growth"), 0.01f, new ConfigDescription("Amount the fire rate increases per shot", null, Array.Empty<object>()));
			rifleDamageCoefficient = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("03 - Tar Rifle", "Damage"), 0.5f, new ConfigDescription("Rifle damage per bullet", null, Array.Empty<object>()));
			rifleProcCoefficient = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("03 - Tar Rifle", "Proc Coefficient"), 0.7f, new ConfigDescription("Rifle proc coefficient per bullet", null, Array.Empty<object>()));
			rifleForce = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("03 - Tar Rifle", "Force"), 5f, new ConfigDescription("Rifle bullet force", null, Array.Empty<object>()));
			rifleFireRate = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("03 - Tar Rifle", "Fire Rate"), 1.6f, new ConfigDescription("Higher value = lower fire rate, due to spaghetti code", null, Array.Empty<object>()));
			clayGrenadeStock = PlayableTemplarPlugin.instance.Config.Bind<int>(new ConfigDefinition("04 - Clay Bomb", "Stock"), 2, new ConfigDescription("Maximum stock", null, Array.Empty<object>()));
			clayGrenadeCooldown = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("04 - Clay Bomb", "Cooldown"), 5f, new ConfigDescription("Bomb cooldown", null, Array.Empty<object>()));
			clayGrenadeDamageCoefficient = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("04 - Clay Bomb", "Damage"), 4f, new ConfigDescription("Explosion damage", null, Array.Empty<object>()));
			clayGrenadeProcCoefficient = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("04 - Clay Bomb", "Proc Coefficient"), 0.8f, new ConfigDescription("Explosion proc coefficient", null, Array.Empty<object>()));
			clayGrenadeRadius = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("04 - Clay Bomb", "Radius"), 12f, new ConfigDescription("Explosion radius", null, Array.Empty<object>()));
			clayGrenadeDetonationTime = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("04 - Clay Bomb", "Detonation Time"), 0.15f, new ConfigDescription("The time it takes to explode after hitting something", null, Array.Empty<object>()));
			blunderbussStock = PlayableTemplarPlugin.instance.Config.Bind<int>(new ConfigDefinition("05 - Blunderbuss", "Stock"), 4, new ConfigDescription("Maximum stock", null, Array.Empty<object>()));
			blunderbussCooldown = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Blunderbuss", "Cooldown"), 2.5f, new ConfigDescription("Cooldown per shot", null, Array.Empty<object>()));
			blunderbussPelletCount = PlayableTemplarPlugin.instance.Config.Bind<int>(new ConfigDefinition("05 - Blunderbuss", "Pellet count"), 6, new ConfigDescription("Pellets fired per shot", null, Array.Empty<object>()));
			blunderbussDamageCoefficient = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Blunderbuss", "Damage"), 1f, new ConfigDescription("Pellet damage", null, Array.Empty<object>()));
			blunderbussProcCoefficient = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Blunderbuss", "Proc Coefficient"), 1f, new ConfigDescription("Pellet proc coefficient", null, Array.Empty<object>()));
			blunderbussSpread = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Blunderbuss", "Pellet spread"), 10f, new ConfigDescription("Pellet spread when fired", null, Array.Empty<object>()));
			tarStock = PlayableTemplarPlugin.instance.Config.Bind<int>(new ConfigDefinition("06 - Tar Blast", "Stock"), 1, new ConfigDescription("Maximum stock", null, Array.Empty<object>()));
			tarCooldown = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("06 - Tar Blast", "Cooldown"), 1f, new ConfigDescription("Cooldown", null, Array.Empty<object>()));
			overdriveStock = PlayableTemplarPlugin.instance.Config.Bind<int>(new ConfigDefinition("06 - Tar Overdrive", "Stock"), 1, new ConfigDescription("Maximum stock", null, Array.Empty<object>()));
			overdriveCooldown = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("06 - Tar Overdrive", "Cooldown"), 12f, new ConfigDescription("Cooldown", null, Array.Empty<object>()));
			dashStock = PlayableTemplarPlugin.instance.Config.Bind<int>(new ConfigDefinition("07 - Sidestep", "Stock"), 2, new ConfigDescription("Maximum stock", null, Array.Empty<object>()));
			dashCooldown = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("07 - Sidestep", "Cooldown"), 7f, new ConfigDescription("Cooldown", null, Array.Empty<object>()));
			bazookaStock = PlayableTemplarPlugin.instance.Config.Bind<int>(new ConfigDefinition("08 - Bazooka", "Stock"), 1, new ConfigDescription("Maximum stock", null, Array.Empty<object>()));
			bazookaCooldown = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("08 - Bazooka", "Cooldown"), 14f, new ConfigDescription("Bazooka cooldown", null, Array.Empty<object>()));
			bazookaDamageCoefficient = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("08 - Bazooka", "Damage"), 12f, new ConfigDescription("Bazooka damage", null, Array.Empty<object>()));
			bazookaProcCoefficient = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("08 - Bazooka", "Proc Coefficient"), 0.6f, new ConfigDescription("Bazooka proc coefficient", null, Array.Empty<object>()));
			bazookaBlastRadius = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("08 - Bazooka", "Blast Radius"), 16f, new ConfigDescription("Bazooka blast radius", null, Array.Empty<object>()));
			miniBazookaDamageCoefficient = PlayableTemplarPlugin.instance.Config.Bind<float>(new ConfigDefinition("08 - Bazooka Mk.2", "Damage"), 5f, new ConfigDescription("Bazooka Mk.2 damage", null, Array.Empty<object>()));
			enableRocketJump = PlayableTemplarPlugin.instance.Config.Bind<bool>(new ConfigDefinition("10 - Misc", "Enable Rocket Jump"), true, new ConfigDescription("Disable if you just don't like the mechanic", null, Array.Empty<object>()));
			jellyfishEvent = PlayableTemplarPlugin.instance.Config.Bind<bool>(new ConfigDefinition("10 - Misc", "Jellyfish Event"), false, new ConfigDescription("Be prepared for the Random Jellyfish Event", null, Array.Empty<object>()));
			masochism = PlayableTemplarPlugin.instance.Config.Bind<bool>(new ConfigDefinition("10 - Misc", "Masochism"), false, new ConfigDescription("Gives Bazooka to enemy Clay Templars", null, Array.Empty<object>()));
		}

        // this helper automatically makes config entries for disabling survivors
        internal static ConfigEntry<bool> CharacterEnableConfig(string characterName)
        {
            return PlayableTemplarPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this character"));
        }

        internal static ConfigEntry<bool> EnemyEnableConfig(string characterName)
        {
            return PlayableTemplarPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this enemy"));
        }
    }
}