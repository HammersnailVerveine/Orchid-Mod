using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Dark
{
	public class EmoVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 5;
			item.width = 24;
			item.height = 24;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.DARK;
			this.rightClickDust = 27;
			this.colorR = 182;
			this.colorG = 27;
			this.colorB = 248;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Emo Vial");
			
		    Tooltip.SetDefault("Briefly shadowburns your target");
		}
	}
}
