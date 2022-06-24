using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.Thorium.Viscount
{
	[AutoloadEquip(EquipType.Body)]
	public class VampireTunic : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 15, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 7;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vampire Tunic");
			Tooltip.SetDefault("8% increased shamanic critical chance");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			modPlayer.shamanCrit += 8;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(null, "ViscountMaterial", 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
