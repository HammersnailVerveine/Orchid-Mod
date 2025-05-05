using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using OrchidMod.Common;
using OrchidMod.Common.Attributes;

namespace OrchidMod.Content.Dancer.Armors.OreHelms
{
	[AutoloadEquip(EquipType.Head)]
	[ClassTag(ClassTags.Dancer)]
	public class DancerHallowedHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.defense = 8;
		}

		public override void UpdateEquip(Player player)
		{
			//
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			bool validBody = body.type == ItemID.HallowedPlateMail || body.type == ItemID.AncientHallowedPlateMail;
			bool validLegs = legs.type == ItemID.HallowedGreaves || legs.type == ItemID.AncientHallowedGreaves;
			return validBody && validLegs;
		}


		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = Language.GetTextValue("ArmorSetBonus.Hallowed");
			player.armorEffectDrawShadow = true;
			player.onHitDodge = true;
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawOutlines = true;
		}

		/*public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HallowedBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}*/
	}
}
