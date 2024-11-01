using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Guardian.Armors.Bamboo
{
	[AutoloadEquip(EquipType.Head)]
	public class GuardianBambooHead : OrchidModGuardianEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 0, 30);
			Item.rare = ItemRarityID.White;
			Item.defense = 2;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			modPlayer.GuardianGuardMax++;
			modPlayer.GuardianRecharge -= 0.15f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemType<GuardianBambooChest>() && legs.type == ItemType<GuardianBambooLegs>();
		}

		public override void UpdateArmorSet(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			player.setBonus = "Taking damage grants a guard charge";
			modPlayer.GuardianBamboo = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.BambooBlock, 20);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
