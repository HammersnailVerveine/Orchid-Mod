using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Air
{
	public class SunplateFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 15;
			item.width = 30;
			item.height = 30;
			item.rare = 2;
			item.value = Item.sellPrice(0, 1, 0, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.AIR;
			this.rightClickDust = 16;
			this.colorR = 190;
			this.colorG = 18;
			this.colorB = 148;
			this.secondaryDamage = 8;
			this.secondaryScaling = 4f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Talk");
		    Tooltip.SetDefault("Creates orbiting stars on impact");
		}
	}
}
