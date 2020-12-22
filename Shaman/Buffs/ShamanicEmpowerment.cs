using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Buffs
{
    public class ShamanicEmpowerment : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Shamanic Empowerment");
			Description.SetDefault("Increases the effectiveness of all your shamanic bonds");
        }
        public override void Update(Player player, ref int buffIndex)
		{
			Player modPlayer = Main.player[Main.myPlayer];
			modPlayer.GetModPlayer<OrchidModPlayer>().shamanFireBonus += 1;
			modPlayer.GetModPlayer<OrchidModPlayer>().shamanWaterBonus += 1;
			modPlayer.GetModPlayer<OrchidModPlayer>().shamanAirBonus += 1;
			modPlayer.GetModPlayer<OrchidModPlayer>().shamanEarthBonus += 1;
			modPlayer.GetModPlayer<OrchidModPlayer>().shamanSpiritBonus += 1;
		}
    }
}