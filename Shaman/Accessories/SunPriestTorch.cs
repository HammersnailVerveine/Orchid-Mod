using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace OrchidMod.Shaman.Accessories
{
	[AutoloadEquip(EquipType.Balloon)]
	public class SunPriestTorch : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			item.width = 22;
			item.height = 20;
            item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = 0;
			item.accessory = true;
			item.vanity = true;
		}
		
		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Sun Priest Torch");
		}
	}
}