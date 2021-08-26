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
			item.width = 30;
			item.height = 20;
			item.value = Item.sellPrice(0, 0, 4, 0);
			item.rare = 1;
			item.defense = 2;
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

		public override bool DrawHead()
		{
			return true;
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			drawHair = false;
			drawAltHair = true;
		}

		public override void AddRecipes()
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;

			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 5);
			recipe.AddIngredient(ItemID.GoldBar, 5);
			recipe.AddIngredient((thoriumMod != null) ? thoriumMod.ItemType("BirdTalon") : ItemType<VultureTalon>(), 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 5);
			recipe.AddIngredient(ItemID.PlatinumBar, 5);
			recipe.AddIngredient((thoriumMod != null) ? thoriumMod.ItemType("BirdTalon") : ItemType<VultureTalon>(), 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
