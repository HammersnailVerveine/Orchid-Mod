using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Weapons.Chips
{
	public class PlatinumChip : OrchidModGamblerChipItem
	{

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.width = 26;
			Item.height = 26;
			Item.useStyle = 1;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 60;
			Item.useTime = 30;
			Item.knockBack = 6f;
			Item.damage = 32;
			Item.crit = 4;
			Item.rare = 1;
			Item.shootSpeed = 10f;
			Item.shoot = Mod.Find<ModProjectile>("PlatinumChipProj").Type;
			Item.autoReuse = true;
			this.chipCost = 1;
			this.consumeChance = 100;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Platinum Chip");
			Tooltip.SetDefault("Throws gambling chips at your foes");
		}
		
		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack, OrchidModPlayer modPlayer, float speed) {
			Vector2 velocity = new Vector2(0f, speed).RotatedBy(MathHelper.ToRadians(modPlayer.gamblerChipSpin));
			Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, Item.shoot, damage, knockBack, player.whoAmI);
			return false;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.Diamond, 8);
			recipe.AddIngredient(ItemID.PlatinumBar, 10);
			recipe.Register();
			recipe.AddRecipe();
		}
	}
}
