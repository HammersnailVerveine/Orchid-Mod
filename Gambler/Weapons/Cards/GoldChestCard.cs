using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class GoldChestCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 16;
			item.crit = 4;
			item.knockBack = 1f;
			item.useAnimation = 10;
			item.useTime = 10;
			item.shootSpeed = 10f;
			this.cardRequirement = 3;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Enchantment");
		    Tooltip.SetDefault("Releases damaging sparkles");
		}
	}
}
