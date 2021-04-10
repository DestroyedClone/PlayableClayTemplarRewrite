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
			PlayableTemplar.originalSize = base.Config.Bind<bool>(new ConfigDefinition("01 - General Settings", "Original Size"), false, new ConfigDescription("Makes the Templar similar in size to the enemy Templars", null, Array.Empty<object>()));
			PlayableTemplar.oldIcons = base.Config.Bind<bool>(new ConfigDefinition("01 - General Settings", "Old Skill Icons"), false, new ConfigDescription("Uses the old skill icons", null, Array.Empty<object>()));
			PlayableTemplar.bazookaGoBoom = base.Config.Bind<bool>(new ConfigDefinition("01 - General Settings", "Bazooka go KABOOM"), false, new ConfigDescription("Enables Bazooka Mk.2", null, Array.Empty<object>()));
			PlayableTemplar.disableWeaponSwap = base.Config.Bind<bool>(new ConfigDefinition("01 - General Settings", "Disable Weapon Swap"), false, new ConfigDescription("Disables this skill, for those who find it overpowered", null, Array.Empty<object>()));
			PlayableTemplar.baseHealth = base.Config.Bind<float>(new ConfigDefinition("01 - General Settings", "Health"), 200f, new ConfigDescription("Base health", null, Array.Empty<object>()));
			PlayableTemplar.healthGrowth = base.Config.Bind<float>(new ConfigDefinition("01 - General Settings", "Health growth"), 48f, new ConfigDescription("Health per level", null, Array.Empty<object>()));
			PlayableTemplar.baseArmor = base.Config.Bind<float>(new ConfigDefinition("01 - General Settings", "Armor"), 10f, new ConfigDescription("Base armor", null, Array.Empty<object>()));
			PlayableTemplar.baseDamage = base.Config.Bind<float>(new ConfigDefinition("01 - General Settings", "Damage"), 12f, new ConfigDescription("Base damage", null, Array.Empty<object>()));
			PlayableTemplar.damageGrowth = base.Config.Bind<float>(new ConfigDefinition("01 - General Settings", "Damage growth"), 2.4f, new ConfigDescription("Damage per level", null, Array.Empty<object>()));
			PlayableTemplar.baseRegen = base.Config.Bind<float>(new ConfigDefinition("01 - General Settings", "Regen"), 0.5f, new ConfigDescription("Base HP regen", null, Array.Empty<object>()));
			PlayableTemplar.regenGrowth = base.Config.Bind<float>(new ConfigDefinition("01 - General Settings", "Regen growth"), 0.5f, new ConfigDescription("HP regen per level", null, Array.Empty<object>()));
			PlayableTemplar.minigunDamageCoefficient = base.Config.Bind<float>(new ConfigDefinition("02 - Minigun", "Damage"), 0.4f, new ConfigDescription("Minigun damage per bullet", null, Array.Empty<object>()));
			PlayableTemplar.minigunProcCoefficient = base.Config.Bind<float>(new ConfigDefinition("02 - Minigun", "Proc Coefficient"), 0.4f, new ConfigDescription("Minigun proc coefficient per bullet", null, Array.Empty<object>()));
			PlayableTemplar.minigunForce = base.Config.Bind<float>(new ConfigDefinition("02 - Minigun", "Force"), 3f, new ConfigDescription("Minigun bullet force", null, Array.Empty<object>()));
			PlayableTemplar.minigunArmorBoost = base.Config.Bind<float>(new ConfigDefinition("02 - Minigun", "Armor Bonus"), 50f, new ConfigDescription("Bonus armor while firing", null, Array.Empty<object>()));
			PlayableTemplar.minigunStationaryArmorBoost = base.Config.Bind<float>(new ConfigDefinition("02 - Minigun", "Stationary Armor Bonus"), 100f, new ConfigDescription("Bonus armor while standing still and firing", null, Array.Empty<object>()));
			PlayableTemplar.minigunMinFireRate = base.Config.Bind<float>(new ConfigDefinition("02 - Minigun", "Minimum Fire Rate"), 0.75f, new ConfigDescription("Starting fire rate", null, Array.Empty<object>()));
			PlayableTemplar.minigunMaxFireRate = base.Config.Bind<float>(new ConfigDefinition("02 - Minigun", "Maximum Fire Rate"), 1.35f, new ConfigDescription("Max fire rate", null, Array.Empty<object>()));
			PlayableTemplar.minigunFireRateGrowth = base.Config.Bind<float>(new ConfigDefinition("02 - Minigun", "Fire Rate Growth"), 0.01f, new ConfigDescription("Amount the fire rate increases per shot", null, Array.Empty<object>()));
			PlayableTemplar.rifleDamageCoefficient = base.Config.Bind<float>(new ConfigDefinition("03 - Tar Rifle", "Damage"), 0.5f, new ConfigDescription("Rifle damage per bullet", null, Array.Empty<object>()));
			PlayableTemplar.rifleProcCoefficient = base.Config.Bind<float>(new ConfigDefinition("03 - Tar Rifle", "Proc Coefficient"), 0.7f, new ConfigDescription("Rifle proc coefficient per bullet", null, Array.Empty<object>()));
			PlayableTemplar.rifleForce = base.Config.Bind<float>(new ConfigDefinition("03 - Tar Rifle", "Force"), 5f, new ConfigDescription("Rifle bullet force", null, Array.Empty<object>()));
			PlayableTemplar.rifleFireRate = base.Config.Bind<float>(new ConfigDefinition("03 - Tar Rifle", "Fire Rate"), 1.6f, new ConfigDescription("Higher value = lower fire rate, due to spaghetti code", null, Array.Empty<object>()));
			PlayableTemplar.clayGrenadeStock = base.Config.Bind<int>(new ConfigDefinition("04 - Clay Bomb", "Stock"), 2, new ConfigDescription("Maximum stock", null, Array.Empty<object>()));
			PlayableTemplar.clayGrenadeCooldown = base.Config.Bind<float>(new ConfigDefinition("04 - Clay Bomb", "Cooldown"), 5f, new ConfigDescription("Bomb cooldown", null, Array.Empty<object>()));
			PlayableTemplar.clayGrenadeDamageCoefficient = base.Config.Bind<float>(new ConfigDefinition("04 - Clay Bomb", "Damage"), 4f, new ConfigDescription("Explosion damage", null, Array.Empty<object>()));
			PlayableTemplar.clayGrenadeProcCoefficient = base.Config.Bind<float>(new ConfigDefinition("04 - Clay Bomb", "Proc Coefficient"), 0.8f, new ConfigDescription("Explosion proc coefficient", null, Array.Empty<object>()));
			PlayableTemplar.clayGrenadeRadius = base.Config.Bind<float>(new ConfigDefinition("04 - Clay Bomb", "Radius"), 12f, new ConfigDescription("Explosion radius", null, Array.Empty<object>()));
			PlayableTemplar.clayGrenadeDetonationTime = base.Config.Bind<float>(new ConfigDefinition("04 - Clay Bomb", "Detonation Time"), 0.15f, new ConfigDescription("The time it takes to explode after hitting something", null, Array.Empty<object>()));
			PlayableTemplar.blunderbussStock = base.Config.Bind<int>(new ConfigDefinition("05 - Blunderbuss", "Stock"), 4, new ConfigDescription("Maximum stock", null, Array.Empty<object>()));
			PlayableTemplar.blunderbussCooldown = base.Config.Bind<float>(new ConfigDefinition("05 - Blunderbuss", "Cooldown"), 2.5f, new ConfigDescription("Cooldown per shot", null, Array.Empty<object>()));
			PlayableTemplar.blunderbussPelletCount = base.Config.Bind<int>(new ConfigDefinition("05 - Blunderbuss", "Pellet count"), 6, new ConfigDescription("Pellets fired per shot", null, Array.Empty<object>()));
			PlayableTemplar.blunderbussDamageCoefficient = base.Config.Bind<float>(new ConfigDefinition("05 - Blunderbuss", "Damage"), 1f, new ConfigDescription("Pellet damage", null, Array.Empty<object>()));
			PlayableTemplar.blunderbussProcCoefficient = base.Config.Bind<float>(new ConfigDefinition("05 - Blunderbuss", "Proc Coefficient"), 1f, new ConfigDescription("Pellet proc coefficient", null, Array.Empty<object>()));
			PlayableTemplar.blunderbussSpread = base.Config.Bind<float>(new ConfigDefinition("05 - Blunderbuss", "Pellet spread"), 10f, new ConfigDescription("Pellet spread when fired", null, Array.Empty<object>()));
			PlayableTemplar.tarStock = base.Config.Bind<int>(new ConfigDefinition("06 - Tar Blast", "Stock"), 1, new ConfigDescription("Maximum stock", null, Array.Empty<object>()));
			PlayableTemplar.tarCooldown = base.Config.Bind<float>(new ConfigDefinition("06 - Tar Blast", "Cooldown"), 1f, new ConfigDescription("Cooldown", null, Array.Empty<object>()));
			PlayableTemplar.overdriveStock = base.Config.Bind<int>(new ConfigDefinition("06 - Tar Overdrive", "Stock"), 1, new ConfigDescription("Maximum stock", null, Array.Empty<object>()));
			PlayableTemplar.overdriveCooldown = base.Config.Bind<float>(new ConfigDefinition("06 - Tar Overdrive", "Cooldown"), 12f, new ConfigDescription("Cooldown", null, Array.Empty<object>()));
			PlayableTemplar.dashStock = base.Config.Bind<int>(new ConfigDefinition("07 - Sidestep", "Stock"), 2, new ConfigDescription("Maximum stock", null, Array.Empty<object>()));
			PlayableTemplar.dashCooldown = base.Config.Bind<float>(new ConfigDefinition("07 - Sidestep", "Cooldown"), 7f, new ConfigDescription("Cooldown", null, Array.Empty<object>()));
			PlayableTemplar.bazookaStock = base.Config.Bind<int>(new ConfigDefinition("08 - Bazooka", "Stock"), 1, new ConfigDescription("Maximum stock", null, Array.Empty<object>()));
			PlayableTemplar.bazookaCooldown = base.Config.Bind<float>(new ConfigDefinition("08 - Bazooka", "Cooldown"), 14f, new ConfigDescription("Bazooka cooldown", null, Array.Empty<object>()));
			PlayableTemplar.bazookaDamageCoefficient = base.Config.Bind<float>(new ConfigDefinition("08 - Bazooka", "Damage"), 12f, new ConfigDescription("Bazooka damage", null, Array.Empty<object>()));
			PlayableTemplar.bazookaProcCoefficient = base.Config.Bind<float>(new ConfigDefinition("08 - Bazooka", "Proc Coefficient"), 0.6f, new ConfigDescription("Bazooka proc coefficient", null, Array.Empty<object>()));
			PlayableTemplar.bazookaBlastRadius = base.Config.Bind<float>(new ConfigDefinition("08 - Bazooka", "Blast Radius"), 16f, new ConfigDescription("Bazooka blast radius", null, Array.Empty<object>()));
			PlayableTemplar.miniBazookaDamageCoefficient = base.Config.Bind<float>(new ConfigDefinition("08 - Bazooka Mk.2", "Damage"), 5f, new ConfigDescription("Bazooka Mk.2 damage", null, Array.Empty<object>()));
			PlayableTemplar.enableRocketJump = base.Config.Bind<bool>(new ConfigDefinition("10 - Misc", "Enable Rocket Jump"), true, new ConfigDescription("Disable if you just don't like the mechanic", null, Array.Empty<object>()));
			PlayableTemplar.jellyfishEvent = base.Config.Bind<bool>(new ConfigDefinition("10 - Misc", "Jellyfish Event"), false, new ConfigDescription("Be prepared for the Random Jellyfish Event", null, Array.Empty<object>()));
			PlayableTemplar.masochism = base.Config.Bind<bool>(new ConfigDefinition("10 - Misc", "Masochism"), false, new ConfigDescription("Gives Bazooka to enemy Clay Templars", null, Array.Empty<object>()));
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