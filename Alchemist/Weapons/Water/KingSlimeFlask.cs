using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Water
{
	public class KingSlimeFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 16;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.WATER;
			this.rightClickDust = 56;
			this.colorR = 21;
			this.colorG = 83;
			this.colorB = 214;
			this.secondaryDamage = 5;
			this.secondaryScaling = 4f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slimy Flask");
			
		    Tooltip.SetDefault("Makes hit enemies bouncy and susceptible to fall damage"
							+ "\nHas a chance to release a catalytic slime bubble");
		}
	}
}
