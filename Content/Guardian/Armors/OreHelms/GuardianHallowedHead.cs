using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Armors.OreHelms
{
	[AutoloadEquip(EquipType.Head)]
	public class GuardianHallowedHead : OrchidModGuardianEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.defense = 26;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			player.GetCritChance<GuardianDamageClass>() += 12;
			player.GetDamage<GuardianDamageClass>() += 0.12f;
			modPlayer.GuardianSlamMax += 1;
			modPlayer.GuardianGuardMax += 2;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemID.HallowedPlateMail && legs.type == ItemID.HallowedGreaves;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Become immune after striking an enemy";
			player.armorEffectDrawShadow = true;
			player.onHitDodge = true;
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawOutlines = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HallowedBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
