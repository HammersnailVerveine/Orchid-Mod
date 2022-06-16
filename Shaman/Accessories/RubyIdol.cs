using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	[AutoloadEquip(EquipType.Waist)]
	public class RubyIdol : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ruby Idol");
			Tooltip.SetDefault("Having an active fire bond increases life regeneration");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanRuby = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Ruby, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
			recipe.Register();
		}
	}
}
