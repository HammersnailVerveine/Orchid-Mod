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
			Item.width = 26;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 4, 30, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 25;
			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			modPlayer.GuardianSlamMax += 2;
			modPlayer.GuardianGuardMax += 2;
			player.aggro += 250;
			player.GetDamage<GuardianDamageClass>() += 0.12f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return (body.type == ItemType<GuardianEmpressChest>() || body.type == ItemType<GuardianEmpressChestAlt>()) && legs.type == ItemType<GuardianEmpressLegs>();
		}

		public override void UpdateArmorSet(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			//player.setBonus = "Guardian runes will summon additional projectiles";
			if (player.armor[1].type == ItemType<GuardianEmpressChest>())
			{
				player.setBonus = "Greatly increases the amount of rune projectiles\nGuardian runes will last 50% longer";
				modPlayer.GuardianBonusRune += 2;
				modPlayer.GuardianRuneTimer += 0.5f;
			}
			else
			{
				player.setBonus = "Reduces damage taken by 20% while holding a pavise\nEnemies are drastically more likely to target you";
				if (player.HeldItem.ModItem is OrchidModGuardianShield)
				{
					player.endurance += 0.2f;
					player.aggro += 1000;
				}
			}
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
