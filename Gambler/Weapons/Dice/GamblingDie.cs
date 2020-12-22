using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Weapons.Dice
{
	public class GamblingDie : OrchidModGamblerDie
	{
		public override void SafeSetDefaults()
		{
			item.width = 32;
			item.height = 34;
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			this.diceID = 0;
			this.diceCost = 2;
			this.diceDuration = 15;
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gambling Die");
		    Tooltip.SetDefault("Increases gambling damage by 2 - 12%");
		}
	}
}
