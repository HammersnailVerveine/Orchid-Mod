using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Chips
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
			Item.crit = 4;
			Item.rare = 1;
			Item.shootSpeed = 10f;
			Item.shoot = ProjectileType<Gambler.Projectiles.Chips.GoldChipProj>();
			Item.autoReuse = true;
			this.chipCost = 1;
			this.consumeChance = 100;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Chip");
			Tooltip.SetDefault("Throws gambling chips at your foes");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(Mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.Ruby, 8);
			recipe.AddIngredient(ItemID.GoldBar, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
