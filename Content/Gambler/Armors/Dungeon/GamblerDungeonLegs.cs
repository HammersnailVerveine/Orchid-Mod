using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Gambler.Armors.Dungeon
{
	[AutoloadEquip(EquipType.Legs)]
	public class GamblerDungeonLegs : OrchidModGamblerEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 35, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 6;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Tyche Leggings");
			// Tooltip.SetDefault("10% increased gambling damage");
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage<GamblerDamageClass>() += 0.1f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(null, "TiamatRelic", 1);
			recipe.AddIngredient(ItemID.Bone, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
