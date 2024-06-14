using OrchidMod.Content.Gambler.Misc;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Accessories
{
	public class MechanicalSpike : OrchidModGuardianItem
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 4, 50, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			modPlayer.GuardianSpikeGoblin = true;
			modPlayer.GuardianSpikeMechanical = true;
			player.GetDamage<GuardianDamageClass>() += 0.12f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient<DungeonSpike>();
			recipe.AddIngredient(ItemID.AvengerEmblem);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
}