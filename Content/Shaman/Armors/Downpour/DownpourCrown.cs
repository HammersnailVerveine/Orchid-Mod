using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Armors.Downpour
{
	[AutoloadEquip(EquipType.Head)]
	public class DownpourCrown : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.defense = 8;
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Downpour Crown");
			// Tooltip.SetDefault("Your shamanic bonds will last 4 seconds longer");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			modPlayer.shamanBuffTimer += 4;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == Mod.Find<ModItem>("DownpourTunic").Type && legs.type == Mod.Find<ModItem>("DownpourKilt").Type;
		}

		public override void UpdateArmorSet(Player player)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			player.setBonus = " Dealing damage with 3 or more active bonds has a chance to electrocute the enemy"; // + bonds affects alchemic stats
			modPlayer.shamanDownpour = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.AdamantiteBar, 10);
			recipe.AddIngredient(null, "DownpourCrystal", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.TitaniumBar, 10);
			recipe.AddIngredient(null, "DownpourCrystal", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
