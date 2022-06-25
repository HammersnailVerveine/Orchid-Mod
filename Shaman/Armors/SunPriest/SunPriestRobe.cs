using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.SunPriest
{
	[AutoloadEquip(EquipType.Body)]
	public class SunPriestRobe : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 6, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 18;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sun Priest Tunic");
			Tooltip.SetDefault("8% increased shamanic damage"
							 + "\n4% increased shamanic critical stike chance");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			player.GetCritChance<ShamanDamageClass>() += 4;
			player.GetDamage<ShamanDamageClass>() += 0.08f;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(null, "LihzahrdSilk", 5);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 24);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
