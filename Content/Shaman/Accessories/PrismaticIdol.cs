using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Accessories
{
	[AutoloadEquip(EquipType.Waist)]
	public class PrismaticIdol : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 5, 50, 0);
			Item.rare = ItemRarityID.Pink;
			Item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Prismatic Idol");
			/* Tooltip.SetDefault("Your different shamanic bonds increases various stats while active"
							+ "\nYour shamanic earth bonds will increase your maximum life by 50"
							+ "\nIncreases the duration of your shamanic bonds by 3 seconds"
							+ "\n10% increased shamanic damage"); */
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int BuffsCount = modPlayer.CountShamanicBonds();

			// buff timer  3;
			/*
			modPlayer.shamanAmber = true;
			modPlayer.shamanAmethyst = true;
			modPlayer.shamanTopaz = true;
			modPlayer.shamanSapphire = true;
			modPlayer.shamanEmerald = true;
			modPlayer.shamanRuby = true;
			*/
			player.GetDamage<ShamanDamageClass>() += 0.10f;
		}
		
		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<AmberIdol>(), 1);
			recipe.AddIngredient(ModContent.ItemType<TopazIdol>(), 1);
			recipe.AddIngredient(ModContent.ItemType<AmethystIdol>(), 1);
			recipe.AddIngredient(ModContent.ItemType<SapphireIdol>(), 1);
			recipe.AddIngredient(ModContent.ItemType<EmeraldIdol>(), 1);
			recipe.AddIngredient(ModContent.ItemType<RubyIdol>(), 1);
			recipe.AddIngredient(ModContent.ItemType<DiamondIdol>(), 1);
			recipe.AddIngredient(ModContent.ItemType<ShamanEmblem>(), 1);
			recipe.AddIngredient(ItemID.HallowedBar, 10);
			recipe.AddIngredient(ItemID.SoulofFright, 5);
			recipe.AddIngredient(ItemID.SoulofMight, 5);
			recipe.AddIngredient(ItemID.SoulofSight, 5);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
}