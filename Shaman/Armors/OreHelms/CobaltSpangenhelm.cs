using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.OreHelms
{
	[AutoloadEquip(EquipType.Head)]
	public class CobaltSpangenhelm : OrchidModShamanEquipable
	{


		public override void SafeSetDefaults()
		{
			item.width = 22;
			item.height = 20;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = 4;
			item.defense = 7;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Spangenhelm");
			Tooltip.SetDefault("9% increased shamanic critical strike chance");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanCrit += 9;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == 374 && legs.type == 375;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Your shamanic bonds will last 4 seconds longer";
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanBuffTimer += 4;
			player.armorEffectDrawShadow = true;
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			drawHair = drawAltHair = false;
		}

		public override bool DrawHead()
		{
			return true;
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CobaltBar, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
