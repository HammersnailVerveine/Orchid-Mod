using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.Crimson
{
	[AutoloadEquip(EquipType.Body)]
	public class BloodShamanChest : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 5;
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
			ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Shaman Tunic");
			Tooltip.SetDefault("7% increased shamanic critical chance");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanCrit += 7;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrimtaneBar, 25);
			recipe.AddIngredient(ItemID.TissueSample, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
