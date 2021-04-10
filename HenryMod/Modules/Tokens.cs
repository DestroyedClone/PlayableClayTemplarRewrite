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
            bool value3 = PlayableTemplar.enableRocketJump.Value;
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
            LanguageAPI.Add(prefix + "PRIMARY_MINIGUN_DESCRIPTION", Helpers.rapidFirePrefix + $"Rev up and fire your <style=cIsUtility>minigun</style>, dealing <style=cIsDamage>" + PlayableTemplar.minigunDamageCoefficient.Value * 100f + "% damage</style> per bullet. <style=cIsUtility>Slow</style> your movement while shooting, but gain <style=cIsHealing>bonus armor</style>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Handgun");
            LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Helpers.agilePrefix + $"Fire a handgun for <style=cIsDamage>{100f * StaticValues.gunDamageCoefficient}% damage</style>.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_ROLL_NAME", "Roll");
            LanguageAPI.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Roll a short distance, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_NAME", "Bomb");
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * StaticValues.bombDamageCoefficient}% damage</style>.");
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