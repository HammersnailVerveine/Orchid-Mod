using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class IceChestCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 22;
			item.crit = 4;
			item.knockBack = 4f;
			item.useAnimation = 25;
			item.useTime = 25;
			item.shootSpeed = 12.5f;
			this.cardRequirement = 2;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Icicle");
		    Tooltip.SetDefault("Summons icicles, falling from the ceiling above your cursor");
		}
	}
}
