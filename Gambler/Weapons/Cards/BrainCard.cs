using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class BrainCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 35;
			item.crit = 4;
			item.knockBack = 1f;
			item.useAnimation = 30;
			item.useTime = 30;
			this.cardRequirement = 4;
			this.gamblerCardSets.Add("Boss");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : The Hivemind");
		    Tooltip.SetDefault("Summons 3 brains around you, one of them following your cursor"
							+  "\nOnly one of them is real, and deals contact damage."
							+  "\nHitting randomly changes the true brain, and increases damage a lot"
							+  "\nBrains cannot deal damage if they are too close to you");
		}
	}
}
