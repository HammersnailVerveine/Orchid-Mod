using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.GemTiaras
{
	[AutoloadEquip(EquipType.Head)]
	public class EmeraldTiara : OrchidModShamanEquipable
	{

		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 12;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 2;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Emerald Circlet");
			Tooltip.SetDefault("Having an active air bond increases movement speed by 10%"
							  + "\nYour shamanic bonds will last 3 seconds longer");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanEmerald = true;
			modPlayer.shamanBuffTimer += 3;
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			drawHair = true;
			drawAltHair = false;
		}

		public override bool DrawHead()
		{
			return true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Emerald, 1);
			recipe.AddIngredient(null, "EmptyTiara", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
			recipe.Register();
		}
	}
}
