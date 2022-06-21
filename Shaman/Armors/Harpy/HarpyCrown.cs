using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.Harpy
{
	[AutoloadEquip(EquipType.Head)]
	public class HarpyCrown : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 16;
			Item.value = Item.sellPrice(0, 0, 15, 50);
			Item.rare = ItemRarityID.Green;
			Item.defense = 4;
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harpy Crown");
			Tooltip.SetDefault("6% increased shamanic damage");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanDamage += 0.06f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == Mod.Find<ModItem>("HarpyLightArmor").Type && legs.type == Mod.Find<ModItem>("HarpyLegs").Type;
		}

		public override void UpdateArmorSet(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			player.setBonus = "Shamanic air bonds slow falling speed"
							+ "\n             press DOWN to fall faster"
							+ "\n             Your shamanic bonds will last 3 seconds longer";
			modPlayer.shamanBuffTimer += 3;
			modPlayer.shamanFeather = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(null, "HarpyTalon", 1);
			recipe.AddIngredient(ItemID.Feather, 3);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
