using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.Corruption
{
	[AutoloadEquip(EquipType.Legs)]
	public class DarkShamanLegs : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 14;
			Item.value = Item.sellPrice(0, 0, 45, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 4;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Shaman Kilt");
			Tooltip.SetDefault("6% increased shamanic damage");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			player.GetDamage<ShamanDamageClass>() += 0.06f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DemoniteBar, 20);
			recipe.AddIngredient(ItemID.ShadowScale, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
