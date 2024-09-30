using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Armors.Hell
{
	[AutoloadEquip(EquipType.Head)]
	public class HellShamanTiara : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 90, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 7;
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidShaman shaman = player.GetModPlayer<OrchidShaman>();
			player.GetDamage<ShamanDamageClass>() += 0.06f;
			shaman.ShamanBondDuration += 3;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<HellShamanTunic>() && legs.type == ModContent.ItemType<HellShamanKilt>();
		}

		public override void UpdateArmorSet(Player player)
		{
			OrchidShaman shaman = player.GetModPlayer<OrchidShaman>();
			player.setBonus = "15% increased shamanic damage while a fire bond is active"
							+ "\nRetaliate upon taking damage while an earth bond is active";

			shaman.shamanHell = true;
			if (shaman.IsShamanicBondReleased(ShamanElement.FIRE)) player.GetDamage<ShamanDamageClass>() += 0.15f;
		}

		/*
		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HellstoneBar, 5);
			recipe.AddIngredient(ItemID.Silk, 10);
			recipe.AddIngredient(ItemID.Bone, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
		*/
	}
}
