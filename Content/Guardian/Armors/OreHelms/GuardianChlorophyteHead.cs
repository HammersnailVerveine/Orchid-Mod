using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Armors.OreHelms
{
	[AutoloadEquip(EquipType.Head)]
	public class GuardianChlorophyteHead : OrchidModGuardianEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 6, 0, 0);
			Item.rare = ItemRarityID.Lime;
			Item.defense = 24;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			player.GetDamage<GuardianDamageClass>() += 0.16f;
			modPlayer.GuardianSlamMax += 2;
			modPlayer.GuardianBlockMax += 1;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemID.ChlorophytePlateMail && legs.type == ItemID.ChlorophyteGreaves;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Summons a powerful leaf crystal to shoot at nearby enemies";
			player.AddBuff((60), 1);
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowSubtle = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
