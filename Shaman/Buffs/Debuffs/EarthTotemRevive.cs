using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Buffs.Debuffs
{
    public class EarthTotemRevive : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Earth Totem Revival");
			Description.SetDefault("Cannot be revived by a shaman totem");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
        }
    }
}