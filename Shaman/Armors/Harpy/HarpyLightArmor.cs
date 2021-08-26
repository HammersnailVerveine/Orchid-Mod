using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.Harpy
{
	[AutoloadEquip(EquipType.Body)]
	public class HarpyLightArmor : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			item.width = 30;
			item.height = 18;
			item.value = Item.sellPrice(0, 0, 25, 50);
			item.rare = 2;
			item.defense = 5;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harpy Tunic");
			Tooltip.SetDefault("8% increased shamanic critical strike chance");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanCrit += 8;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "HarpyTalon", 3);
			recipe.AddIngredient(ItemID.Feather, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
