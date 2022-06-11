using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	[AutoloadEquip(EquipType.Waist)]
	public class PrismaticIdol : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 5, 50, 0);
			Item.rare = 5;
			Item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismatic Idol");
			Tooltip.SetDefault("Your different shamanic bonds increases various stats while active"
							+ "\nYour shamanic earth bonds will increase your maximum life by 50"
							+ "\nIncreases the duration of your shamanic bonds by 3 seconds"
							+ "\n10% increased shamanic damage");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int BuffsCount = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod);

			modPlayer.shamanBuffTimer += 3;
			modPlayer.shamanAmber = true;
			modPlayer.shamanAmethyst = true;
			modPlayer.shamanTopaz = true;
			modPlayer.shamanSapphire = true;
			modPlayer.shamanEmerald = true;
			modPlayer.shamanRuby = true;
			modPlayer.shamanDamage += 0.10f;
		}
		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(null, "AmberIdol", 1);
			recipe.AddIngredient(null, "TopazIdol", 1);
			recipe.AddIngredient(null, "AmethystIdol", 1);
			recipe.AddIngredient(null, "SapphireIdol", 1);
			recipe.AddIngredient(null, "EmeraldIdol", 1);
			recipe.AddIngredient(null, "RubyIdol", 1);
			recipe.AddIngredient(null, "DiamondIdol", 1);
			recipe.AddIngredient(null, "ShamanEmblem", 1);
			recipe.AddIngredient(1225, 10);
			recipe.AddIngredient(547, 5);
			recipe.AddIngredient(548, 5);
			recipe.AddIngredient(549, 5);
			recipe.AddTile(114);
			recipe.Register();
			recipe.AddRecipe();
		}
	}
}