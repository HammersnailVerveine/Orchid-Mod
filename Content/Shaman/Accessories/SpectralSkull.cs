using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Accessories
{
	[AutoloadEquip(EquipType.Neck)]
	public class SpectralSkull : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 5, 30, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
			Item.damage = 80;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Spectral Skull");
			// Tooltip.SetDefault("Active water bonds allows your shamanic critical strikes to release homing lost souls");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			modPlayer.shamanSkull = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SpectreBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
