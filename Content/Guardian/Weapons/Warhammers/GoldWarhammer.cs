﻿using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Warhammers
{
	public class GoldWarhammer : OrchidModGuardianHammer
	{

		public override void SafeSetDefaults()
		{
			Item.width = 38;
			Item.height = 38;
			Item.value = Item.sellPrice(0, 0, 17, 50);
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.knockBack = 8f;
			Item.shootSpeed = 9f;
			Item.damage = 61;
			range = 25;
			blockStacks = 1;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.GoldBar, 8);
			recipe.Register();
		}
	}
}
