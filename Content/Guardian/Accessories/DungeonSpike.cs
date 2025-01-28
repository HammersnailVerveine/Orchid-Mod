using OrchidMod.Content.Gambler.Misc;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Accessories
{
	public class DungeonSpike : OrchidModGuardianItem
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 35, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
			Item.damage = 30; // duplicate modifications in GuardianShieldAnchor for the projectile spawn
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			if (modPlayer.GuardianSpikeDamage < 1.5f)
				modPlayer.GuardianSpikeDamage = 1.5f;
			modPlayer.GuardianSpikeDungeon = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient<GoblinSpike>();
			recipe.AddIngredient<TiamatRelic>();
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}