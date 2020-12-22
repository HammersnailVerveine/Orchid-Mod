using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class SlimeCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 8;
			item.crit = 4;
			item.knockBack = 0.5f;
			item.shootSpeed = 10f;
			item.useAnimation = 30;
			item.useTime = 30;
			this.cardRequirement = 0;
			this.gamblerCardSets.Add("Slime");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Bouncy Beginnings");
		    Tooltip.SetDefault("Summons a bouncy slime, following your cursor"
							+  "\nEach successful hit increases damage, touching the ground resets it");
		}
	}
}
