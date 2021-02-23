using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Misc.Scrolls
{
	public class ScrollTier4 : OrchidModAlchemistScroll
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 4;
			this.hintLevel = 4;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alchemist Recipe Scroll");
		    Tooltip.SetDefault("Contains the recipe for an unknown alchemist hidden reaction");
		}
	}
}
