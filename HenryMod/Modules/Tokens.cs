using R2API;
using System;

namespace PlayableTemplar.Modules
{
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            #region Henry
            string prefix = PlayableTemplarPlugin.developerPrefix + "_PLAYABLETEMPLAR_BODY_";

            string desc = "The Clay Templar is a slow, tanky bruiser who uses the many weapons in his arsenal to mow down his opposition with ease.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Minigun takes time to rev up, but inflicts heavy damage at a high rate of fire." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Let Blunderbuss reload all 4 shots and unload them all at once for a big burst of damage." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Use Bazooka after applying tar to deal massive AoE damage with chain explosions." + Environment.NewLine + Environment.NewLine;
            bool value3 = Modules.Config.enableRocketJump.Value;
            if (value3)
            {
                desc = desc + "< ! > The explosion force from Bazooka can be used to launch yourself high up with good timing.</color>" + Environment.NewLine;
            }
            else
            {
                desc = desc + "< ! > Tar Rifle is an all around good weapon in most cases, but lacks the high damage of Minigun.</color>" + Environment.NewLine;
            }

            string outro = "..and so it left, reveling in its triumph.";
            string outroFailure = "..and so it vanished, washed up in smoke.";

            LanguageAPI.Add(prefix + "NAME", "Clay Templar");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Lost Soldier of Aphelia");
            LanguageAPI.Add(prefix + "LORE", "Resembling a man, the creature stood around seven to eight feet tall, with skin made of tar and elaborate accessories of clay decorating its body. ");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Vagabond");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Volatile Tar");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Certain attacks cover enemies in <style=cIsDamage>tar</style>, <style=cIsUtility>slowing</style> them. <style=cIsDamage>Ignite</style> the <style=cIsDamage>tar</style> to create an <style=cIsDamage>explosion</style> that leaves enemies <style=cIsDamage>Scorched</style>, <style=cIsHealth>reducing their armor</style>.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_MINIGUN_NAME", "Minigun");
            LanguageAPI.Add(prefix + "PRIMARY_MINIGUN_DESCRIPTION", Helpers.rapidFirePrefix + $"Rev up and fire your <style=cIsUtility>minigun</style>, dealing <style=cIsDamage>" + Modules.Config.minigunDamageCoefficient.Value * 100f + "% damage</style> per bullet. <style=cIsUtility>Slow</style> your movement while shooting, but gain <style=cIsHealing>bonus armor</style>.");
            
            LanguageAPI.Add(prefix + "PRIMARY_PRECISEMINIGUN_NAME", "Tar Rifle");
            LanguageAPI.Add(prefix + "PRIMARY_PRECISEMINIGUN_DESCRIPTION", $"Fire a <style=cIsUtility>tar rifle</style>, dealing <style=cIsDamage>" + Modules.Config.rifleDamageCoefficient.Value * 100f + "% damage</style> per bullet and applying <style=cIsUtility>tar</style> with high accuracy.");
            
            //bazooka swaps with railgun depending on bazookaGoBoom
            LanguageAPI.Add(prefix + "PRIMARY_BAZOOKA_NAME", "Bazooka Mk. 2");
            LanguageAPI.Add(prefix + "PRIMARY_BAZOOKA_DESCRIPTION", Helpers.explosivePrefix + $"Fire a <style=cIsUtility>rocket</style>, dealing <style=cIsDamage>" + Modules.Config.miniBazookaDamageCoefficient.Value * 100f + "% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_RAILGUN_NAME", "Railgun");
            LanguageAPI.Add(prefix + "PRIMARY_RAILGUN_DESCRIPTION", Helpers.explosivePrefix + $"Charge up and fire a <style=cIsUtility>piercing bullet</style>, dealing <style=cIsDamage>800% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_FLAMETHROWER_NAME", "Flamethrower");
            LanguageAPI.Add(prefix + "PRIMARY_FLAMETHROWER_DESCRIPTION", Helpers.explosivePrefix + $"Fire a <style=cIsUtility>continuous stream of flames</style>, dealing <style=cIsDamage>1250% damage per second</style>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_GRENADE_NAME", "Clay Bomb");

            var clayBombText = $"Throw a <style=cIsUtility>clay bomb</style> that explodes for <style=cIsDamage>" + Modules.Config.clayGrenadeDamageCoefficient.Value * 100f + "% damage</style> and inflicts <style=cIsUtility>tar</style>.";
            if (Modules.Config.clayGrenadeStock.Value > 1)
            {
                clayBombText = string.Concat(new object[]
                {
                    clayBombText,
                    " Hold up to <style=cIsDamage>",
                    Modules.Config.clayGrenadeStock.Value,
                    "</style> bombs."
                });
            }
            LanguageAPI.Add(prefix + "SECONDARY_GRENADE_DESCRIPTION", clayBombText);

            var shotgunText = string.Concat(new object[]
            {
                "Fire a burst of <style=cIsUtility>pellets</style>, dealing <style=cIsDamage>",
                Modules.Config.blunderbussPelletCount.Value,
                "x",
                Modules.Config.blunderbussDamageCoefficient.Value * 100f,
                "% damage</style>."
            });
            if (Modules.Config.blunderbussStock.Value > 1)
            {
                shotgunText = string.Concat(new object[]
                {
                    shotgunText,
                    " Store up to ",
                    Modules.Config.blunderbussStock.Value,
                    " shots."
                });
            }

            LanguageAPI.Add(prefix + "SECONDARY_SHOTGUN_NAME", "Blunderbuss");
            LanguageAPI.Add(prefix + "SECONDARY_SHOTGUN_DESCRIPTION", "Roll a short distance, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_OVERDRIVE_NAME", "Tar Overdrive");
            LanguageAPI.Add(prefix + "UTILITY_OVERDRIVE_DESCRIPTION", "Shift into <style=cIsUtility>maximum overdrive</style>, knocking enemies back and applying <style=cIsUtility>tar</style>. <style=cIsUtility>Gain bonus attack speed and health regen for 3 seconds.</style>");
            
            LanguageAPI.Add(prefix + "UTILITY_DODGE_NAME", "Sidestep");

            string dodgeText = "<style=cIsUtility>Dash</style> a short distance. Can dash while shooting.";
            if (Modules.Config.dashStock.Value > 1)
            {
                dodgeText = string.Concat(new object[]
                {
                    dodgeText,
                    " Store up to ",
                    Modules.Config.dashStock.Value,
                    " charges."
                });
            }

            LanguageAPI.Add(prefix + "UTILITY_DODGE_DESCRIPTION", dodgeText);
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_FIRE_NAME", "Bomb");
            LanguageAPI.Add(prefix + "SPECIAL_FIRE_DESCRIPTION", Helpers.explosivePrefix + $"Throw a bomb for <style=cIsDamage>{100f * StaticValues.bombDamageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "SPECIAL_SWAP_NAME", "Weapon Swap");
            LanguageAPI.Add(prefix + "SPECIAL_SWAP_DESCRIPTION", "<style=cIsUtility>Swap</style> your <style=cIsDamage>primary</style> weapon type.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Henry: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Henry, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Henry: Mastery");
            #endregion

            #region Keywords
            LanguageAPI.Add("KEYWORD_RAPIDFIRE", "<style=cKeywordName>Rapidfire</style><style=cSub><style=cIsDamage>Rate of fire</style> increases the longer the skill is held.</style></style>");
            LanguageAPI.Add("KEYWORD_EXPLOSIVE", "<style=cKeywordName>Explosive</style><style=cSub><style=cIsDamage>Ignite</style> <style=cIsUtility>tarred enemies</style>, creating an <style=cIsDamage>explosion</style> for <style=cIsDamage>20% of the damage dealt</style> and <style=cIsHealth>reducing their armor</style>.</style></style>");
            #endregion

            #endregion
        }
    }
}