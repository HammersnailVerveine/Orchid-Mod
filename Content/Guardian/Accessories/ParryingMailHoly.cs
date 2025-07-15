using OrchidMod.Content.Gambler.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Accessories
{
	public class ParryingMailHoly : OrchidModGuardianItem
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 4, 50, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			modPlayer.GuardianBlockDuration += 0.25f;
			modPlayer.GuardianParryDuration += 0.25f;
			player.longInvince = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient<SturdySlab>();
			recipe.AddIngredient(ItemID.CrossNecklace);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
}