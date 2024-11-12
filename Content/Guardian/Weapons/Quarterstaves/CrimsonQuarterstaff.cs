using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Quarterstaves
{
	public class CrimsonQuarterstaff : OrchidModGuardianQuarterstaff
	{
		public override void SafeSetDefaults()
		{
			Item.width = 48;
			Item.height = 48;
			Item.value = Item.sellPrice(0, 1, 40, 00);
			Item.rare = ItemRarityID.Blue;
			Item.useTime = 30;
			ParryDuration = 60;
			Item.knockBack = 8f;
			Item.damage = 58;
			GuardStacks = 1;
			JabSpeed = 1.25f;
			SingleSwing = true;
			SwingSpeed = 1.25f;
			SwingDamage = 3f;
			SwingKnockback = 2f;
		}
	}
}
