using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.General.Items.Sets.StaticQuartz
{
	public class StaticQuartzRanger : OrchidModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Static Quartz Blowpipe");
		}

		public override void SetDefaults()
		{
			Item.damage = 10;
			Item.ranged = true;
			Item.width = 44;
			Item.height = 12;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 1f;
			Item.value = Item.sellPrice(0, 0, 5, 0);
			Item.rare = 1;
			Item.UseSound = SoundID.Item64;
			Item.shoot = 10;
			Item.shootSpeed = 10f;
			Item.useAmmo = AmmoID.Dart;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemType<General.Items.Sets.StaticQuartz.StaticQuartz>(), 12);
			recipe.Register();
			recipe.AddRecipe();
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(4, -6);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY - 2f)) * 25f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			return true;
		}
	}
}
