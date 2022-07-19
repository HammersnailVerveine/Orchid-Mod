using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.Corruption
{
	[AutoloadEquip(EquipType.Head)]
	public class DarkShamanTiara : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 14;
			Item.value = Item.sellPrice(0, 0, 75, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 4;
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Shaman Tiara");
			Tooltip.SetDefault("8% increased shamanic damage");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			player.GetDamage<ShamanDamageClass>() += 0.08f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == Mod.Find<ModItem>("DarkShamanChest").Type && legs.type == Mod.Find<ModItem>("DarkShamanLegs").Type;
		}

		public override void UpdateArmorSet(Player player)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			player.setBonus = "Shamanic fire bonds cause attacks to shadowburn"
							+ "\n             Your shamanic bonds will last 3 seconds longer";
			modPlayer.shamanBuffTimer += 3;
			modPlayer.shamanDemonite = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DemoniteBar, 15);
			recipe.AddIngredient(ItemID.ShadowScale, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
