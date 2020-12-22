using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Air
{
	public class CouldInAVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 18;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 1, 0, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.AIR;
			this.rightClickDust = 16;
			this.colorR = 121;
			this.colorG = 152;
			this.colorB = 239;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cloud in a Flask");
		    Tooltip.SetDefault("Launches hit enemy in the air"
							+ "\nIncreases the likelihood of spawning catalytic bubbles");
		}
	}
}
