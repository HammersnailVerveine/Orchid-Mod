using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.Downpour
{
	[AutoloadEquip(EquipType.Body)]
	public class DownpourTunic : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 34;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.rare = 5;
			Item.defense = 16;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Downpour Tunic");
			Tooltip.SetDefault("15% increased shamanic critical strike chance");
		}

		public override void UpdateEquip(Player player)
		{
			Main.player[Main.myPlayer].GetModPlayer<OrchidModPlayer>().shamanCrit += 10;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.AdamantiteBar, 20);
			recipe.AddIngredient(null, "DownpourCrystal", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
			recipe.AddRecipe();

			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.TitaniumBar, 20);
			recipe.AddIngredient(null, "DownpourCrystal", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
			recipe.AddRecipe();
		}
	}
}
