﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Runes
{
	public class GoblinRune : OrchidModGuardianRune
	{

		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 1, 20, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item103;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.knockBack = 3f;
			Item.damage = 53;
			Item.shoot = ModContent.ProjectileType<Projectiles.Runes.GoblinRuneProj>();
			RuneCost = 2;
			RuneNumber = 3;
		}

		public override void Activate(Player player, OrchidGuardian guardian, int type, int damage, float knockback, int critChance, int duration, float distance, int number)
		{
			NewRuneProjectiles(player, guardian, duration, type, damage, knockback, critChance, distance, GetAmount(guardian, 2));
		}
	}
}
