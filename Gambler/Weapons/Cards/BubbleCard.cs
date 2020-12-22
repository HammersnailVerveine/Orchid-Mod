using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class BubbleCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 8;
			item.crit = 4;
			item.knockBack = 1f;
			item.useAnimation = 20;
			item.useTime = 20;
			item.shootSpeed = 5f;
			this.cardRequirement = 2;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Bubbles");
		    Tooltip.SetDefault("Summons bubbles, floating upwards"
							+  "\nReleasing your left click causes existing bubbles to dash"
							+  "\nBubbles gain in damage over time");
		}
	}
}
