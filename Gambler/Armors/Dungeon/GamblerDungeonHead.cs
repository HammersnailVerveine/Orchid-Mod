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
			item.width = 34;
			item.height = 18;
			item.value = Item.sellPrice(0, 0, 30, 0);
			item.rare = 2;
			item.defense = 6;
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

		public override bool DrawHead()
		{
			return true;
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			drawHair = false;
			drawAltHair = false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "TiamatRelic", 1);
			recipe.AddIngredient(ItemID.Bone, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
