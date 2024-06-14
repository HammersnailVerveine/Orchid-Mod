using OrchidMod.Content.Guardian.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Guardian.Armors.Empress
{
	[AutoloadEquip(EquipType.Head)]
	public class GuardianEmpressHead : OrchidModGuardianEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 4, 30, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 25;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			modPlayer.GuardianSlamMax += 2;
			modPlayer.GuardianBlockMax += 2;
			player.GetDamage<GuardianDamageClass>() += 0.12f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemType<GuardianEmpressChest>() && legs.type == ItemType<GuardianEmpressLegs>();
		}

		public override void UpdateArmorSet(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			//player.setBonus = "Guardian runes will summon additional projectiles";
			player.setBonus = "Greatly increases the amount of rune projectiles\nGuardian runes will last 50% longer";
			modPlayer.GuardianBonusRune += 2;
			modPlayer.GuardianRuneTimer += 0.5f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient<GuardianEmpressMaterial>(8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
