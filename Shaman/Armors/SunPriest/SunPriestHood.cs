using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.SunPriest
{
	[AutoloadEquip(EquipType.Head)]
	public class SunPriestHood : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 7, 50, 0);
			Item.rare = 8;
			Item.defense = 20;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sun Priest Hood");
			Tooltip.SetDefault("10% increased shamanic damage"
							  + "\nYour shamanic bonds will last 5 seconds longer");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanDamage += 0.1f;
			modPlayer.shamanBuffTimer += 5;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == Mod.Find<ModItem>("SunPriestRobe").Type && legs.type == Mod.Find<ModItem>("SunPriestPants").Type;
		}

		public override void UpdateArmorSet(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			player.setBonus = "Releases homing bursts of light while under the effect of a shamanic fire bond";
			modPlayer.shamanSmite = true;
		}

		public override bool DrawHead()
		{
			return false;
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			drawHair = false;
			drawAltHair = false;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(null, "LihzahrdSilk", 3);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
			recipe.AddRecipe();
		}
	}
}
