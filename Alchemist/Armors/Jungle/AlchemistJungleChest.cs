using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Armors.Jungle
{
	[AutoloadEquip(EquipType.Body)]
	public class AlchemistJungleChest : OrchidModAlchemistEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 90, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 7;
		}

		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Lily Tunic");
			Tooltip.SetDefault("Maximum potency increased by 3"
							+ "\nIncreases alchemic main projectile velocity by 20%");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			modPlayer.alchemistPotencyMax += 3;
			modPlayer.alchemistVelocity += 0.2f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Content.Items.Materials.JungleLilyBloomed>(), 1);
			recipe.AddIngredient(ItemID.Vine, 1);
			recipe.AddIngredient(ItemID.JungleSpores, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
