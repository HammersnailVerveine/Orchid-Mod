using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Water
{
	public class DungeonFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 25;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 30, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.WATER;
			this.rightClickDust = 29;
			this.colorR = 6;
			this.colorG = 13;
			this.colorB = 144;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirited Water");
		    Tooltip.SetDefault("Releases lingering water flames"
							+ "\nHas a chance to release a catalytic spirited bubble");
		}
	}
}
