using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class DesertCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 7;
			item.crit = 4;
			item.knockBack = 0.5f;
			item.useAnimation = 10;
			item.useTime = 10;
			item.shootSpeed = 8f;
			this.cardRequirement = 1;
			this.gamblerCardSets.Add("Biome");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Desert");
		    Tooltip.SetDefault("Rapidly fires thorns"
							+  "\nChances to summon a cacti, replicating the attack");
		}
	}
}
