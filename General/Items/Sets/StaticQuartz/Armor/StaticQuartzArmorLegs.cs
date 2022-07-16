using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.General.Items.Sets.StaticQuartz.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class StaticQuartzArmorLegs : OrchidModItem
	{
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 14;
			Item.value = Item.sellPrice(0, 0, 7, 50);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 2;
		}

		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Static Quartz Leggings");
			Tooltip.SetDefault("5% increased movement speed");
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.05f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemType<General.Items.Sets.StaticQuartz.StaticQuartz>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
