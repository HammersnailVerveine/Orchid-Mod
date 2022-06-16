using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Chips
{
	public class CopperChip : OrchidModGamblerChipItem
	{

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 0, 72);
			Item.width = 26;
			Item.height = 26;
			Item.useStyle = 1;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 72;
			Item.useTime = 36;
			Item.knockBack = 5f;
			Item.damage = 21;
			Item.crit = 4;
			Item.rare = 0;
			Item.shootSpeed = 8f;
			Item.shoot = ProjectileType<Gambler.Projectiles.Chips.CopperChipProj>();
			Item.autoReuse = true;
			this.chipCost = 1;
			this.consumeChance = 100;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Copper Chip");
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
			recipe.AddIngredient(20, 6);
			recipe.AddIngredient(ItemID.Wood, 10);
			recipe.Register();
			recipe.Register();
		}
	}
}
