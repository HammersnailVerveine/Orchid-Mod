﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public class MechanicalGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 32;
			Item.knockBack = 5f;
			Item.damage = 45;
			Item.value = Item.sellPrice(0, 0, 8, 40);
			Item.rare = ItemRarityID.White;
			Item.useTime = 35;
			strikeVelocity = 15f;
			parryDuration = 60;
		}

		public override Color GetColor(bool offHand)
		{
			return new Color(223, 51, 51);
		}

		public override void OnHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, NPC.HitInfo hit, bool charged)
		{
			if (charged) guardian.AddGuard(1);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.SilverBar, 8);
			recipe.Register();
		}
	}
}
