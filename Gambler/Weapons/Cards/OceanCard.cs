using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class OceanCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 13;
			item.crit = 4;
			item.knockBack = 5f;
			item.useAnimation = 50;
			item.useTime = 50;
			item.shootSpeed = 5f;
			this.cardRequirement = 1;
			this.gamblerCardSets.Add("Biome");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Ocean");
		    Tooltip.SetDefault("Throws rolling coconuts"
							+  "\nChances to summon a seed, replicating the attack");
		}
	}
}
