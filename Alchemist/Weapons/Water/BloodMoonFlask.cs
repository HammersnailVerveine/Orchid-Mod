using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Water
{
	public class BloodMoonFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 15;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 50, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.WATER;
			this.rightClickDust = 5;
			this.colorR = 349;
			this.colorG = 98;
			this.colorB = 64;
			this.secondaryDamage = 0;
			this.secondaryScaling = 4f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Mist Flask");
		    Tooltip.SetDefault("Creates a lingering cloud of damaging mist"
							+  "\nThe mist knockback heavily scales with the number of elements used");
		}
	}
}
