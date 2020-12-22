using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Alchemist.Buffs.Debuffs
{
    public class ReactionCooldown : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Experimental drawback");
			Description.SetDefault("You cannot use alchemist hidden reactions");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
        }
    }
}