using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Armors.OreHelms
{
	[AutoloadEquip(EquipType.Head)]
	public class GuardianAdamantiteHead : OrchidModGuardianEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.defense = 24;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			player.GetCritChance<GuardianDamageClass>() += 12;
			player.GetDamage<GuardianDamageClass>() += 0.12f;
			modPlayer.GuardianSlamMax += 1;
			modPlayer.GuardianGuardMax += 1;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemID.AdamantiteBreastplate && legs.type == ItemID.AdamantiteLeggings;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Increases guardian charges generation by 20%";
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			modPlayer.GuardianRecharge -= 0.2f;
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawOutlines = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.AdamantiteBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
