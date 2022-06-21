using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Armors.Dungeon
{
	[AutoloadEquip(EquipType.Head)]
	public class GamblerDungeonHead : OrchidModGamblerEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 34;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 6;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tyche Headgear");
			Tooltip.SetDefault("Maximum chips increased by 5");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.gamblerChipsMax += 5;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemType<Gambler.Armors.Dungeon.GamblerDungeonBody>() && legs.type == ItemType<Gambler.Armors.Dungeon.GamblerDungeonLegs>();
		}

		public override void UpdateArmorSet(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			player.setBonus = "Drawing a card gives between 0 and 2 chips";
			modPlayer.gamblerDungeon = true;
		}

		public override bool DrawHead()/* tModPorter Note: Removed. In SetStaticDefaults, use ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false if you returned false */
		{
			return true;
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)/* tModPorter Note: Removed. In SetStaticDefaults, use ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true if you had drawHair set to true, and ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true if you had drawAltHair set to true */
		{
			drawHair = false;
			drawAltHair = false;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(null, "TiamatRelic", 1);
			recipe.AddIngredient(ItemID.Bone, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
			recipe.Register();
		}
	}
}
