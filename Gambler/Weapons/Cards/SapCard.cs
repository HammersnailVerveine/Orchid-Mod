using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class SapCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 8;
			item.crit = 4;
			item.knockBack = 0.5f;
			item.useAnimation = 30;
			item.useTime = 30;
			item.shootSpeed = 3f;
			this.cardRequirement = 0;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Living Sap");
		    Tooltip.SetDefault("Releases a slow-moving sap bubble, following the cursor"
							+  "\nUpon releasing the mouse click, the bubble will explode"
							+  "\nThe longer the bubble exists, the more explosion damage");
		}
	}
}
