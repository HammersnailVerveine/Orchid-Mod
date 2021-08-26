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
			item.width = 22;
			item.height = 14;
			item.value = Item.sellPrice(0, 0, 45, 0);
			item.rare = 2;
			item.defense = 5;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Shaman Kilt");
			Tooltip.SetDefault("5% increased shamanic damage");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanDamage += 0.05f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CrimtaneBar, 20);
			recipe.AddIngredient(ItemID.TissueSample, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
