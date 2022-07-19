using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.Harpy
{
	[AutoloadEquip(EquipType.Body)]
	public class HarpyLightArmor : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 34;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 25, 50);
			Item.rare = ItemRarityID.Green;
			Item.defense = 5;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harpy Tunic");
			Tooltip.SetDefault("8% increased shamanic critical strike chance");
		}

		public override void UpdateEquip(Player player)
		{
			player.GetCritChance<ShamanDamageClass>() += 8;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(null, "HarpyTalon", 3);
			recipe.AddIngredient(ItemID.Feather, 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
