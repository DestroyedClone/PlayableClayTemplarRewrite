using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace PlayableTemplar.Modules
{
    public static class Buffs
    {
        internal static BuffDef armorBuff;
        internal static BuffDef stationaryArmorBuff;
        internal static BuffDef overdriveBuff;
        internal static BuffDef igniteDebuff;

        internal static List<BuffDef> buffDefs = new List<BuffDef>();

        internal static void RegisterBuffs()
        {
            // fix the buff catalog to actually register our buffs
            IL.RoR2.BuffCatalog.Init += FixBuffCatalog; // remove this hook after next ror2 update as it will have been fixed

            armorBuff = AddNewBuff("PlayableTemplarArmorBuff", Resources.Load<Sprite>("Textures/BuffIcons/texBuffGenericShield"), StaticValues.BUFF1_COLOR, false, false);
            stationaryArmorBuff = AddNewBuff("PlayableTemplarStationaryArmorBuff", Resources.Load<Sprite>("Textures/BuffIcons/texBuffGenericShield"), StaticValues.BUFF2_COLOR, false, false);
            igniteDebuff = AddNewBuff("PlayableTemplarIgniteBuff", Resources.Load<Sprite>("Textures/BuffIcons/texBuffGenericShield"), StaticValues.BUFF3_COLOR, false, true);
            overdriveBuff = AddNewBuff("PlayableTemplarOverdriveBuff", Resources.Load<Sprite>("Textures/BuffIcons/texBuffGenericShield"), StaticValues.BUFF4_COLOR, false, false);
        }

        internal static void FixBuffCatalog(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (!c.Next.MatchLdsfld(typeof(RoR2Content.Buffs), nameof(RoR2Content.Buffs.buffDefs)))
            {
                return;
            }

            c.Remove();
            c.Emit(OpCodes.Ldsfld, typeof(ContentManager).GetField(nameof(ContentManager.buffDefs)));
        }

        // simple helper method
        internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;

            buffDefs.Add(buffDef);

            return buffDef;
        }
    }
}