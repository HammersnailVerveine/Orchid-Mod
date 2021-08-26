using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.Corruption
{
	[AutoloadEquip(EquipType.Body)]
	public class DarkShamanChest : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			item.width = 30;
			item.height = 16;
			item.value = Item.sellPrice(0, 0, 60, 0);
			item.rare = 2;
			item.defense = 5;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Shaman Tunic");
			Tooltip.SetDefault("10% increased shamanic critical chance");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanCrit += 10;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DemoniteBar, 25);
			recipe.AddIngredient(ItemID.ShadowScale, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
