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
			item.value = Item.sellPrice(0, 0, 0, 72);
			item.width = 26;
			item.height = 26;
			item.useStyle = 1;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 36;
			item.useTime = 36;
			item.knockBack = 5f;
			item.damage = 16;
			item.crit = 4;
			item.rare = 0;
			item.shootSpeed = 8f;
			item.shoot = ProjectileType<Gambler.Projectiles.Chips.CopperChipProj>();
			item.autoReuse = true;
			this.chipCost = 1;
			this.consumeChance = 100;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Copper Chip");
			Tooltip.SetDefault("Throws gambling chips at your foes");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(20, 6);
			recipe.AddIngredient(ItemID.Wood, 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
