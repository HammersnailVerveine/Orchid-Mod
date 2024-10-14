using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Guardian.Armors.Meteorite
{
	[AutoloadEquip(EquipType.Head)]
	public class GuardianMeteoriteHead : OrchidModGuardianEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 9;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			modPlayer.GuardianGuardMax++;
			player.GetDamage<GuardianDamageClass>() += 0.05f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemType<GuardianMeteoriteChest>() && legs.type == ItemType<GuardianMeteoriteLegs>();
		}

		public override void UpdateArmorSet(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			player.setBonus = "Blocks or parries have a 50% chance to give an additional slam charge";
			modPlayer.GuardianMeteorite = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MeteoriteBar, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
