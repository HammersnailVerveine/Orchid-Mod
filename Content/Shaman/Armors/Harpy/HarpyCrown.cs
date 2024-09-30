using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Armors.Harpy
{
	[AutoloadEquip(EquipType.Head)]
	public class HarpyCrown : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 16;
			Item.value = Item.sellPrice(0, 0, 15, 50);
			Item.rare = ItemRarityID.Green;
			Item.defense = 4;
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			player.GetDamage<ShamanDamageClass>() += 0.06f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<HarpyLightArmor>() && legs.type == ModContent.ItemType<HarpyLegs>();
		}

		public override void UpdateArmorSet(Player player)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			player.setBonus = "Shamanic air bonds slow falling speed"
							+ "\n             press DOWN to fall faster"
							+ "\n             Your shamanic bonds will last 3 seconds longer";

			// effect
			if (!player.controlDown && modPlayer.IsShamanicBondReleased(ShamanElement.AIR)) player.gravity /= 3;
		}

		/*
		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Feather, 4);
			recipe.AddIngredient(ItemID.ShadowScale, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
			
			recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Feather, 4);
			recipe.AddIngredient(ItemID.TissueSample, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
		*/
	}
}
