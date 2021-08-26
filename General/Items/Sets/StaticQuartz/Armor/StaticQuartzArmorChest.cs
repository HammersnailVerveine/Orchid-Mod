using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.General.Items.Sets.StaticQuartz.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class StaticQuartzArmorChest : OrchidModItem
	{
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 18;
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.defense = 2;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Static Quartz Chestplate");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemType<General.Items.Sets.StaticQuartz.StaticQuartz>(), 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
