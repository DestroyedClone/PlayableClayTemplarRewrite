using BepInEx.Configuration;
using UnityEngine;

namespace PlayableTemplar.Modules
{
    public static class Config
	{
		private static ConfigEntry<bool> originalSize;

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
		public static void ReadConfig()
        {

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