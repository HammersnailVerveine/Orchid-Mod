using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Accessories
{
	[AutoloadEquip(EquipType.Waist)]
	public class SapphireIdol : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}

		public override void OnReleaseShamanicBond(Player player, OrchidShaman shaman, ShamanElement element, Projectile catalyst)
		{
			if (element == ShamanElement.WATER) catalyst.damage = (int)(catalyst.damage * 1.2f);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Sapphire, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
