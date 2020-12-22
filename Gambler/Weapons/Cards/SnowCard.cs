using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class SnowCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 10;
			item.crit = 4;
			item.knockBack = 2f;
			item.useAnimation = 40;
			item.useTime = 40;
			item.shootSpeed = 5f;
			this.cardRequirement = 1;
			this.gamblerCardSets.Add("Biome");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Snow");
		    Tooltip.SetDefault("Throws returning snowflakes backwards, gaining in damage over time"
							+  "\nThe snowflakes cannot be thrown diagonally"
							+  "\nChances to summon a pine cone, replicating the attack");
		}
	}
}
