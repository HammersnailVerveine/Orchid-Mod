using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Air
{
	public class QueenBeeFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 18;
			item.width = 30;
			item.height = 30;
			item.rare = 3;
			item.value = Item.sellPrice(0, 1, 50, 0);
			this.potencyCost = 3;
			this.element = AlchemistElement.AIR;
			this.rightClickDust = 153;
			this.colorR = 255;
			this.colorG = 156;
			this.colorB = 12;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Royal Jelly");
		    Tooltip.SetDefault("If no fire element is used, summons bees on impact"
							+ "\nHas a chance to release a catalytic beehive");
		}
	}
}
