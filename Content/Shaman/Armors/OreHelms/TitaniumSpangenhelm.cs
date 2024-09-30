using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Armors.OreHelms
{
	[AutoloadEquip(EquipType.Head)]
	public class TitaniumSpangenhelm : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.defense = 11;
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Titanium Spangenhelm");
			/* Tooltip.SetDefault("Your shamanic bonds will last 4 seconds longer"
							  + "\n16% increased shamanic damage and 7% increased shamanic critical strike chance"); */
		}

		public override void UpdateEquip(Player player)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			// buff timer  4;
			player.GetCritChance<ShamanDamageClass>() += 7;
			player.GetDamage<ShamanDamageClass>() += 0.16f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == 1218 && legs.type == 1219;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Attacking generates a defensive barrier of titanium shards";
			player.armorEffectDrawShadow = true;
			player.onHitTitaniumStorm = true;
		}

		public static void ArmorSetShadows(Player player, ref bool longTrail, ref bool smallPulse, ref bool largePulse, ref bool shortTrail)
		{
			shortTrail = true;
		}

		/*
		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.TitaniumBar, 13);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
		*/
	}
}
