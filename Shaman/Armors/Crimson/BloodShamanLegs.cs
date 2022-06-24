using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.Crimson
{
	[AutoloadEquip(EquipType.Legs)]
	public class BloodShamanLegs : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 14;
			Item.value = Item.sellPrice(0, 0, 45, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 5;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Shaman Kilt");
			Tooltip.SetDefault("5% increased shamanic damage");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidModPlayerShaman modPlayer = player.GetModPlayer<OrchidModPlayerShaman>();
			player.GetDamage<ShamanDamageClass>() += 0.05f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrimtaneBar, 20);
			recipe.AddIngredient(ItemID.TissueSample, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
