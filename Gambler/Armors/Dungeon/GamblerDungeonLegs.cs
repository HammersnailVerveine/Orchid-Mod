using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Gambler.Armors.Dungeon
{
	[AutoloadEquip(EquipType.Legs)]
	public class GamblerDungeonLegs : OrchidModGamblerEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 35, 0);
			Item.rare = 2;
			Item.defense = 6;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tyche Leggings");
			Tooltip.SetDefault("10% increased gambling damage");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.gamblerDamage += 0.1f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(null, "TiamatRelic", 1);
			recipe.AddIngredient(ItemID.Bone, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
			recipe.Register();
		}
	}
}
