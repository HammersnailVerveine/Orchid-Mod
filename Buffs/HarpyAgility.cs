using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace OrchidMod.Buffs
{
    public class HarpyAgility : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Harpy Agility");
			Description.SetDefault("Ability to double jump, your fist bonus jump will release damaging feathers around you");
        }
        public override void Update(Player player, ref int buffIndex)
		{
			Main.player[Main.myPlayer].GetModPlayer<OrchidModPlayer>().doubleJumpHarpy = true;
		}
    }
}