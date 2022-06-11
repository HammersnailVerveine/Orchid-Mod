using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.OreHelms
{
	[AutoloadEquip(EquipType.Head)]
	public class PalladiumSpangenhelm : OrchidModShamanEquipable
	{


		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.rare = 4;
			Item.defense = 8;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Palladium Spangenhelm");
			Tooltip.SetDefault("Your shamanic bonds will last 3 seconds longer"
							  + "\n7% increased shamanic damage and critical strike chance");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanBuffTimer += 3;
			modPlayer.shamanCrit += 7;
			modPlayer.shamanDamage += 0.07f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == 1208 && legs.type == 1209;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Greatly increases life regeneration after striking an enemy";
			player.armorEffectDrawShadow = true;
			player.onHitRegen = true;
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
			shortTrail = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.PalladiumBar, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
			recipe.AddRecipe();
		}
	}
}
