using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Chips
{
	public class HellChip : OrchidModGamblerChipItem
	{

		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.width = 26;
			item.height = 26;
			item.useStyle = 1;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 80;
			item.useTime = 40;
			item.knockBack = 8f;
			item.damage = 48;
			item.crit = 4;
			item.rare = 3;
			item.shootSpeed = 15f;
			item.shoot = ProjectileType<Gambler.Projectiles.Chips.HellChipProj>();
			item.autoReuse = true;
			this.chipCost = 1;
			this.consumeChance = 100;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hellfire Chip");
			Tooltip.SetDefault("Throws explosive chips at your foes");
		}
		
		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack, OrchidModPlayer modPlayer, float speed) {
			Vector2 velocity = new Vector2(0f, speed).RotatedBy(MathHelper.ToRadians(modPlayer.gamblerChipSpin));
			Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, item.shoot, damage, knockBack, player.whoAmI);
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.HellstoneBar, 15);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
