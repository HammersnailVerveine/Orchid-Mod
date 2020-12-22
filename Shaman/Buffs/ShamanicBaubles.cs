using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Buffs
{
    public class ShamanicBaubles : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Shamanic Baubles");
			Description.SetDefault("Your next hit will give you a shamanic orb for free");
        }
    }
}