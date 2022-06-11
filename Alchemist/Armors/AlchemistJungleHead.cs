using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Armors
{
	[AutoloadEquip(EquipType.Head)]
	public class AlchemistJungleHead : OrchidModAlchemistEquipable
	{
		public override string Texture => OrchidAssets.AlchemistJungleSetPath + Name;

		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.rare = 3;
			Item.defense = 6;
		}

		public override void SetStaticDefaults()
		{
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = false;

			DisplayName.SetDefault("Lily Hood");
			Tooltip.SetDefault("20% increased potency regeneration"
							  + "\nMaximum number of simultaneous alchemical elements increased by 1");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistRegenPotency -= 12;
			modPlayer.alchemistNbElementsMax += 1;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemType<Alchemist.Armors.Jungle.AlchemistJungleChest>() && legs.type == ItemType<Alchemist.Armors.Jungle.AlchemistJungleLegs>();
		}

		public override void UpdateArmorSet(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			player.setBonus = "Triggering enough catalytic reactions creates a catalytic flower bud";
			modPlayer.alchemistFlowerSet = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Content.Items.Materials.JungleLilyBloomed>(), 1);
			recipe.AddIngredient(ItemID.Vine, 1);
			recipe.AddIngredient(ItemID.JungleSpores, 3);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
