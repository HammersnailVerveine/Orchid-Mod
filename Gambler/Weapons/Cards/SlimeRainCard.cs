using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class SlimeRainCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 8;
			item.crit = 4;
			item.knockBack = 1f;
			item.useAnimation = 30;
			item.useTime = 30;
			this.cardRequirement = 3;
			this.gamblerCardSets.Add("Slime");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Jelly Clouds");
		    Tooltip.SetDefault("Summons a slime rain cloud, following your cursor"
							+  "\nSlimes will actively chase enemies after landing"
							+  "\nThe longer the slimes falls, the more damage they do");
		}
	}
}
