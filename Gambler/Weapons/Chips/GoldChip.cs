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
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.width = 26;
			item.height = 26;
			item.useStyle = 1;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 25;
			item.useTime = 25;
			item.knockBack = 6f;
			item.damage = 25;
			item.crit = 4;
			item.rare = 1;
			item.shootSpeed = 10f;
			item.shoot = ProjectileType<Gambler.Projectiles.Chips.GoldChipProj>();
			item.autoReuse = true;
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.Ruby, 8);
			recipe.AddIngredient(ItemID.GoldBar, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
