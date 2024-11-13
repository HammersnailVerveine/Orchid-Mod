using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Quarterstaves
{
	public class ShardQuarterstaff : OrchidModGuardianQuarterstaff
	{
		public override void SafeSetDefaults()
		{
			Item.width = 48;
			Item.height = 48;
			Item.value = Item.sellPrice(0, 2, 88, 0);
			Item.rare = ItemRarityID.Pink;
			Item.useTime = 30;
			ParryDuration = 90;
			Item.knockBack = 8f;
			Item.damage = 282;
			GuardStacks = 2;
		}
	}
}
