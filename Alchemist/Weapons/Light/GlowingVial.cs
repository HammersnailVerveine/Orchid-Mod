using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Light
{
	public class GlowingVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 5;
			item.width = 24;
			item.height = 24;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.LIGHT;
			this.rightClickDust = 57;
			this.colorR = 253;
			this.colorG = 194;
			this.colorB = 18;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glowing Vial");
			
		    Tooltip.SetDefault("Confuses your target briefly");
		}
	}
}
