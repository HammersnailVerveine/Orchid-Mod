using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Water
{
	public class GoblinArmyFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 10;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 30, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.WATER;
			this.rightClickDust = 184;
			this.colorR = 22;
			this.colorG = 22;
			this.colorB = 22;
			this.secondaryDamage = 50;
			this.secondaryScaling = 15f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goblin Oil");
		    Tooltip.SetDefault("Coats the target in alchemical water and oil"
							+  "\nUsing a fire element in the same attack will drastically increase its damage"
							+  "\nThis will also spread alchemical fire to all nearby oil coated enemies"
							+  "\nHas a chance to release a catalytic oil bubble, coating nearby enemies on reaction");
		}
	}
}
