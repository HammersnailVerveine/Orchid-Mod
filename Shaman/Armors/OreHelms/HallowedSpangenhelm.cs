using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.OreHelms
{
	[AutoloadEquip(EquipType.Head)]
	public class HallowedSpangenhelm : OrchidModShamanEquipable
	{


		public override void SafeSetDefaults()
		{
			Item.width = 20;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.defense = 12;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallowed Spangenhelm");
			Tooltip.SetDefault("12% increased shamanic damage and critical chance");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanCrit += 12;
			modPlayer.shamanDamage += 0.12f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == 551 && legs.type == 552;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Your shamanic bonds will last 5 seconds longer";
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanBuffTimer += 5;
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

		public static void ArmorSetShadows(Player player, ref bool longTrail, ref bool smallPulse, ref bool largePulse, ref bool shortTrail)
		{
			smallPulse = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HallowedBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
			recipe.Register();
		}
	}
}
