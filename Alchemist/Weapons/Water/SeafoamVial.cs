using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Water
{
	public class SeafoamVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 11;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 20, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.WATER;
			this.rightClickDust = 29;
			this.colorR = 1;
			this.colorG = 139;
			this.colorB = 252;
			this.secondaryDamage = 10;
			this.secondaryScaling = 5f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seafoam Flask");
		    Tooltip.SetDefault("Creates a lingering, damaging water bubble"
							+ "\nHas a chance to release a catalytic seafoam bubble");
		}
	}
}
