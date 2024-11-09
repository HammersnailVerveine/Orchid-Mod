using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Quarterstaves
{
	public class Quarterstaff : OrchidModGuardianQuarterstaff
	{
		public override void SafeSetDefaults()
		{
			Item.width = 42;
			Item.height = 42;
			Item.value = Item.sellPrice(0, 0, 3, 45);
			Item.rare = ItemRarityID.White;
			Item.useTime = 35;
			Item.UseSound = SoundID.DD2_BetsyWindAttack;
			ParryDuration = 60;
			Item.knockBack = 5f;
			Item.damage = 20;
			GuardStacks = 1;
		}
	}
}
