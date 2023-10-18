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
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hallowed Spangenhelm");
			/* Tooltip.SetDefault("12% increased shamanic damage and critical chance"
							+  "\nIncreases the duration of your shamanic bonds by 5 seconds"); */
		}

		public override void UpdateEquip(Player player)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			player.GetCritChance<ShamanDamageClass>() += 12;
			player.GetDamage<ShamanDamageClass>() += 0.12f;
			modPlayer.shamanBuffTimer += 5;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (body.type == ItemID.HallowedPlateMail || body.type == ItemID.AncientHallowedPlateMail) 
				&& (legs.type == ItemID.HallowedGreaves || legs.type == ItemID.AncientHallowedGreaves);
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Become immune after striking an enemy";
			player.armorEffectDrawShadow = true;
			player.onHitDodge = true;
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
		}
	}
}
