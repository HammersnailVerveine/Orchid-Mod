using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Alchemist.Weapons.Nature
{
	public class PoisonVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 15;
			item.width = 30;
			item.height = 30;
			item.rare = 2;
			item.value = Item.sellPrice(0, 0, 30, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.NATURE;
			this.rightClickDust = 44;
			this.colorR = 130;
			this.colorG = 151;
			this.colorB = 31;
			this.secondaryDamage = 14;
			this.secondaryScaling = 7f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Poison Flask");
		    Tooltip.SetDefault("Releases lingering poison bubbles"
							+ "\nHas a chance to release a catalytic poison bubble"
							+ "\nCan contaminate other bubbly weapons effects");
		}
	}
}
