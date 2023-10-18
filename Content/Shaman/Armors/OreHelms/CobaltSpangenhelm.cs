using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Armors.OreHelms
{
	[AutoloadEquip(EquipType.Head)]
	public class CobaltSpangenhelm : OrchidModShamanEquipable
	{


		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.defense = 7;
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Cobalt Spangenhelm");
			// Tooltip.SetDefault("9% increased shamanic critical strike chance");
		}

		public override void UpdateEquip(Player player)
		{
			player.GetCritChance<ShamanDamageClass>() += 9;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == 374 && legs.type == 375;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Your shamanic bonds will last 4 seconds longer";
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			modPlayer.shamanBuffTimer += 4;
			player.armorEffectDrawShadow = true;
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CobaltBar, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
