using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Armors.Dungeon
{
	[AutoloadEquip(EquipType.Body)]
	public class GamblerDungeonBody : OrchidModGamblerEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 34;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = 2;
			Item.defense = 7;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tyche Chestplate");
			Tooltip.SetDefault("10% increased gambling critical strike chance"
							+ "\nMaximum redraws increased by 1");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.gamblerCrit += 10;
			modPlayer.gamblerRedrawsMax += 1;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(null, "TiamatRelic", 1);
			recipe.AddIngredient(ItemID.Bone, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
			recipe.Register();
		}
	}
}
