using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class ShuffleCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 5;
			item.crit = 4;
			item.knockBack = 3f;
			item.useAnimation = 25;
			item.useTime = 25;
			item.shootSpeed = 10f;
			this.cardRequirement = 0;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Shuffle");
		    Tooltip.SetDefault("Randomly shoots a selection of clovers, spades, diamonds and hearts"
							+  "\nEach projectile has its own properties and bahaviour"
							+  "\nDamage increases with the number of cards in your deck");
		}
	}
}
