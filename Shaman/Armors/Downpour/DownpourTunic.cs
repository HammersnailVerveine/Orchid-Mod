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
			item.width = 34;
			item.height = 20;
			item.value = Item.sellPrice(0, 4, 0, 0);
			item.rare = 5;
			item.defense = 16;
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.AdamantiteBar, 20);
			recipe.AddIngredient(null, "DownpourCrystal", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.TitaniumBar, 20);
			recipe.AddIngredient(null, "DownpourCrystal", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
