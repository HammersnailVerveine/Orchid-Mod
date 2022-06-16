using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.GemTiaras
{
	[AutoloadEquip(EquipType.Head)]
	public class DiamondTiara : OrchidModShamanEquipable
	{

		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 12;
			Item.value = Item.sellPrice(0, 0, 25, 0);
			Item.rare = 1;
			Item.defense = 2;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diamond Circlet");
			Tooltip.SetDefault("5% increased shamanic critical stike chance"
							  + "\nYour shamanic bonds will last 3 seconds longer"
							  + "\n10% increased shamanic damage");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanDamage += 0.1f;
			modPlayer.shamanCrit += 5;
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
			recipe.AddIngredient(ItemID.Diamond, 1);
			recipe.AddIngredient(null, "EmptyTiara", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
			recipe.Register();
		}
	}
}
