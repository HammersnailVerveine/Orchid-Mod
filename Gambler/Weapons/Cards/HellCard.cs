using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class HellCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 40;
			item.crit = 4;
			item.knockBack = 3f;
			item.useAnimation = 30;
			item.useTime = 30;
			item.shootSpeed = 13f;
			this.cardRequirement = 3;
			this.gamblerCardSets.Add("Biome");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Hell");
		    Tooltip.SetDefault("Launches fiery mortar"
							+  "\nChances to summon a pepper, replicating the attack");
		}
	}
}
