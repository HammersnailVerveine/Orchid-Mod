using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Fire
{
	public class EmberVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 7;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.FIRE;
			this.rightClickDust = 6;
			this.colorR = 253;
			this.colorG = 62;
			this.colorB = 3;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ember Flask");
		    Tooltip.SetDefault("Burns your target briefly"
							+ "\nCoats your target in alchemical fire"
							+ "\nReleases lingering embers");
		}
	}
}
