using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Misc.Scrolls
{
	public class ScrollTier3 : OrchidModAlchemistScroll
	{
		public override void SafeSetDefaults()
		{
			item.rare = 3;
			this.hintLevel = 3;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alchemist Recipe Scroll");
		    Tooltip.SetDefault("Contains the recipe for an unknown alchemist hidden reaction");
		}
	}
}
