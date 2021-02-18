using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Air
{
	public class ShadowChestFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 25;
			item.width = 30;
			item.height = 30;
			item.rare = 3;
			item.value = Item.sellPrice(0, 2, 50, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.AIR;
			this.rightClickDust = 21;
			this.colorR = 139;
			this.colorG = 42;
			this.colorB = 156;
			this.secondaryDamage = 10;
			this.secondaryScaling = 20f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Demon Breath");
		    Tooltip.SetDefault("Releases returning demon flames"
							+ "\nCoats hit enemies in alchemical air");
		}
	}
}
