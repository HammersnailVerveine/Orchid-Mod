using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.OreHelms
{
	[AutoloadEquip(EquipType.Head)]
	public class AdamantiteSpangenhelm : OrchidModShamanEquipable
	{

		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.defense = 11;
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
		}

		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Adamantite Spangenhelm");
			Tooltip.SetDefault("11% increased shamanic damage and critical chance");
		}

		public override void UpdateEquip(Player player)
		{
			player.GetCritChance<ShamanDamageClass>() += 11;
			player.GetDamage<ShamanDamageClass>() += 0.11f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == 403 && legs.type == 404;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Your shamanic bonds will last 5 seconds longer";
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			modPlayer.shamanBuffTimer += 5;
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawOutlines = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.AdamantiteBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
