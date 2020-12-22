using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class KingSlimeCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 16;
			item.crit = 4;
			item.knockBack = 1f;
			item.shootSpeed = 10f;
			item.useAnimation = 30;
			item.useTime = 30;
			this.cardRequirement = 3;
			this.gamblerCardSets.Add("Boss");
			this.gamblerCardSets.Add("Slime");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Blue King");
		    Tooltip.SetDefault("Summons a bouncy slime, following your cursor, and jumping up to it"
							+  "\nGains in damage with fall distance and enemy hits, touching on the ground resets it");
		}
	}
}
