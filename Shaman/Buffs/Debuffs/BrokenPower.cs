using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Shaman.Buffs.Debuffs
{
    public class BrokenPower : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Broken Present");
			Description.SetDefault("Reduces the effectiveness of all your shamanic bonds"
								 + "\nNullifies the positive effects of the fragile and sinister present accessories");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
        }
		
        public override void Update(Player player, ref int buffIndex)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanFireBonus --;
			modPlayer.shamanWaterBonus --;
			modPlayer.shamanAirBonus --;
			modPlayer.shamanEarthBonus --;
			modPlayer.shamanSpiritBonus --;
		}
    }
}