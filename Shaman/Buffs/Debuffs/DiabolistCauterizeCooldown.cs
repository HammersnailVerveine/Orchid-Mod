using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Buffs.Debuffs
{
    public class DiabolistCauterizeCooldown : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Cauterized");
			Description.SetDefault("Cannot trigger a diabolist cauterization again");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
        }
    }
}