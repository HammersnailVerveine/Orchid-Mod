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
			Item.width = 30;
			Item.height = 16;
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 5;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Dark Shaman Tunic");
			// Tooltip.SetDefault("10% increased shamanic critical chance");
		}

		public override void UpdateEquip(Player player)
		{
			player.GetCritChance<ShamanDamageClass>() += 10;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DemoniteBar, 25);
			recipe.AddIngredient(ItemID.ShadowScale, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
