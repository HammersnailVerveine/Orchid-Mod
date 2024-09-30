using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Accessories
{
	[AutoloadEquip(EquipType.Waist)]
	public class AmberIdol : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidShaman shaman = player.GetModPlayer<OrchidShaman>();
			if (shaman.IsShamanicBondReleased(ShamanElement.EARTH)) player.lifeRegen += 5;
		}

		/*
		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Amber, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
		*/
	}
}