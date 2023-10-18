using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Gambler.Weapons.Chips
{
	public class GoldChip : OrchidModGamblerChipItem
	{

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.width = 26;
			Item.height = 26;
			Item.useStyle = 1;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 50;
			Item.useTime = 25;
			Item.knockBack = 6f;
			Item.damage = 30;
			Item.rare = ItemRarityID.Blue;
			Item.shootSpeed = 10f;
			Item.shoot = ProjectileType<Content.Gambler.Projectiles.Chips.GoldChipProj>();
			Item.autoReuse = true;
			this.chipCost = 1;
			this.consumeChance = 100;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Gold Chip");
			// Tooltip.SetDefault("Throws gambling chips at your foes");
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockBack, OrchidGambler modPlayer, float speed)
		{
			velocity = new Vector2(0f, speed).RotatedBy(MathHelper.ToRadians(modPlayer.gamblerChipSpin));
			Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, Item.shoot, damage, knockBack, player.whoAmI);
			return false;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.Ruby, 8);
			recipe.AddIngredient(ItemID.GoldBar, 10);
			recipe.Register();
		}
	}
}
