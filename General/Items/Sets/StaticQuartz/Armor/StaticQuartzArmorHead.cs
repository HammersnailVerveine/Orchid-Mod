using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.General.Items.Sets.StaticQuartz.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class StaticQuartzArmorHead : OrchidModItem
	{
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 16;
			Item.value = Item.sellPrice(0, 0, 5, 0);
			Item.rare = 1;
			Item.defense = 1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Static Quartz Headpiece");
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			int chestPiece = ItemType<General.Items.Sets.StaticQuartz.Armor.StaticQuartzArmorChest>();
			int legPiece = ItemType<General.Items.Sets.StaticQuartz.Armor.StaticQuartzArmorLegs>();
			return body.type == chestPiece && legs.type == legPiece;
		}

		public override void UpdateArmorSet(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			player.setBonus = "Moving after staying immobile for a while charges you up";
			modPlayer.generalStatic = true;
		}

		public override bool DrawHead()
		{
			return true;
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			drawHair = true;
			drawAltHair = false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(Mod);
			recipe.AddIngredient(ItemType<General.Items.Sets.StaticQuartz.StaticQuartz>(), 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
