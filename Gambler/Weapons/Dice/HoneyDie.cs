using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Weapons.Dice
{
	public class HoneyDie : OrchidModGamblerDie
	{
		public override void SafeSetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.rare = 2;
			this.diceID = 2;
			this.diceCost = 3;
			this.diceDuration = 20;
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wax Die");
		    Tooltip.SetDefault("Recovers 1 - 6 health on gambling critical strike");
		}
	}
}
