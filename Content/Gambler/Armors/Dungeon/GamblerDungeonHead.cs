using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Gambler.Armors.Dungeon
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
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Tyche Headgear");
			// Tooltip.SetDefault("Maximum chips increased by 5");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			modPlayer.gamblerChipsMax += 5;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemType<Gambler.Armors.Dungeon.GamblerDungeonBody>() && legs.type == ItemType<Gambler.Armors.Dungeon.GamblerDungeonLegs>();
		}

		public override void UpdateArmorSet(Player player)
		{
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			player.setBonus = Language.GetTextValue("Mods.OrchidMod.Items.GamblerDungeonHead.SetBonus");
			modPlayer.gamblerDungeon = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(null, "TiamatRelic", 1);
			recipe.AddIngredient(ItemID.Bone, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
