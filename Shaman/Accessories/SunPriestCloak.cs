using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	[AutoloadEquip(EquipType.Back)]
	public class SunPriestCloak : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 4, 75, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 2;
			Item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sun Priest Cloak");
			Tooltip.SetDefault("Having all five shamanic bonds active at once will allow them to fade out 50% slower");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod) == 5 && modPlayer.timer120 % 2 == 0)
			{
				modPlayer.shamanFireTimer++;
				modPlayer.shamanWaterTimer++;
				modPlayer.shamanAirTimer++;
				modPlayer.shamanEarthTimer++;
				modPlayer.shamanSpiritTimer++;
			}
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(null, "LihzahrdSilk", 2);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
			recipe.Register();
		}
	}
}