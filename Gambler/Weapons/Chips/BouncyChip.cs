using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Chips
{
	public class BouncyChip : OrchidModGamblerChipItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 5, 20);
			item.width = 26;
			item.height = 26;
			item.useStyle = 1;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 30;
			item.useTime = 30;
			item.knockBack = 3f;
			item.damage = 20;
			item.crit = 4;
			item.rare = 1;
			item.shootSpeed = 10f;
			item.shoot = ProjectileType<Gambler.Projectiles.Chips.BouncyChipProj>();
			item.autoReuse = true;
			this.chipCost = 1;
			this.consumeChance = 100;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bouncy Chip");
			Tooltip.SetDefault("Throws bouncy gambling chips at your foes");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ModContent.ItemType<Gambler.Weapons.Chips.GamblingChip>(), 1);
			recipe.AddIngredient(3111, 25); // Pink Gel
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
