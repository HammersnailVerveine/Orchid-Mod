using OrchidMod.Gambler.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Armors.Outlaw
{
	[AutoloadEquip(EquipType.Head)]
	public class OutlawHat : OrchidModGamblerEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 4, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 2;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Outlaw Hat");
			Tooltip.SetDefault("Maximum chips increased by 3");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.gamblerChipsMax += 3;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemType<Gambler.Armors.Outlaw.OutlawVest>() && legs.type == ItemType<Gambler.Armors.Outlaw.OutlawPants>();
		}

		public override void UpdateArmorSet(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			player.setBonus = "Maximum redraws increased by 1";
			modPlayer.gamblerRedrawsMax += 1;
		}

		public override bool DrawHead()/* tModPorter Note: Removed. In SetStaticDefaults, use ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false if you returned false */
		{
			return true;
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)/* tModPorter Note: Removed. In SetStaticDefaults, use ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true if you had drawHair set to true, and ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true if you had drawAltHair set to true */
		{
			drawHair = false;
			drawAltHair = true;
		}

		public override void AddRecipes()
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;

			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Silk, 5);
			recipe.AddIngredient(ItemID.GoldBar, 5);
			recipe.AddIngredient((thoriumMod != null) ? thoriumMod.Find<ModItem>("BirdTalon").Type : ItemType<VultureTalon>(), 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
			recipe.Register();

			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Silk, 5);
			recipe.AddIngredient(ItemID.PlatinumBar, 5);
			recipe.AddIngredient((thoriumMod != null) ? thoriumMod.Find<ModItem>("BirdTalon").Type : ItemType<VultureTalon>(), 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
			recipe.Register();
		}
	}
}
