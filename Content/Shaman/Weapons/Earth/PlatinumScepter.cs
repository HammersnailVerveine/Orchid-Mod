using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shaman.Projectiles;
using OrchidMod.Content.Shaman.Projectiles.Earth;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Earth
{
	public class PlatinumScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 32;
			Item.width = 36;
			Item.height = 38;
			Item.useTime = 46;
			Item.useAnimation = 46;
			Item.knockBack = 5.5f;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.UseSound = SoundID.Item45;
			Item.autoReuse = true;
			Item.shootSpeed = 9.5f;
			Item.shoot = ModContent.ProjectileType<GemScepterProjectilePlatinum>();
			this.Element = ShamanElement.EARTH;
			CatalystMovement = ShamanSummonMovement.FLOATABOVE;
		}
		/*
		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.Diamond, 8)
			.AddIngredient(ItemID.PlatinumBar, 10)
			.AddTile(TileID.Anvils)
			.Register();
		*/
	}
}