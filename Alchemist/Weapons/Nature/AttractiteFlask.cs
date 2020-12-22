using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Nature
{
	public class AttractiteFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 10;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 20, 0);
			this.potencyCost = 3;
			this.element = AlchemistElement.NATURE;
			this.rightClickDust = 60;
			this.colorR = 155;
			this.colorG = 21;
			this.colorB = 18;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Attractite Flask");
		    Tooltip.SetDefault("Hit target will attract most nearby alchemical lingering projectiles"
							+  "\nThe attractivity buff will jump to the nearest target on miss");
		}
	}
}
