using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	[AutoloadEquip(EquipType.Waist)]
	public class EmeraldIdol : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Emerald Idol");
			Tooltip.SetDefault("Having an active air bond increases movement speed by 10%");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanEmerald = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Emerald, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}