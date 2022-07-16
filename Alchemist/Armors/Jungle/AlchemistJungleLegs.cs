using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Armors.Jungle
{
	[AutoloadEquip(EquipType.Legs)]
	public class AlchemistJungleLegs : OrchidModAlchemistEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 7;
		}

		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Lily Leggings");
			Tooltip.SetDefault("10% increased chemical damage");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			player.GetDamage<AlchemistDamageClass>() += 0.1f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Content.Items.Materials.JungleLilyBloomed>(), 1);
			recipe.AddIngredient(ItemID.Vine, 2);
			recipe.AddIngredient(ItemID.JungleSpores, 3);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
