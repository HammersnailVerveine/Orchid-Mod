using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.Thorium.Viscount
{
	[AutoloadEquip(EquipType.Legs)]
	public class VampireSarong : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 12, 50);
			Item.rare = ItemRarityID.Green;
			Item.defense = 6;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vampire Sarong");
			Tooltip.SetDefault("8% increased shamanic damage");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanDamage += 0.08f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(null, "ViscountMaterial", 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
			recipe.Register();
		}
	}
}
