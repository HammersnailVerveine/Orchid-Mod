using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Buffs.Thorium
{
    public class TitanicBuff : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Titan Energy");
			Description.SetDefault("Massively increased shamanic critical strike damage and chance");
        }
        public override void Update(Player player, ref int buffIndex)
		{
			Player modPlayer = Main.player[Main.myPlayer];
			modPlayer.GetModPlayer<OrchidModPlayer>().shamanCrit += 20;
		}
    }
}