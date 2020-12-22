using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Nature
{
	public class LivingSapVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 11;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 15, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.NATURE;
			this.rightClickDust = 153;
			this.colorR = 255;
			this.colorG = 148;
			this.colorB = 0;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Living Sap Flask");
		    Tooltip.SetDefault("Creates a healing living sap bubble"
							+ "\nHas a chance to release a bigger, catalytic sap bubble"
							+ "\nOn reaction, heals players and coats enemies in alchemical nature");
		}
	}
}
