using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Standards
{
	public class CopperStandard : OrchidModGuardianStandard
	{
		public override void SafeSetDefaults()
		{
			Item.width = 42;
			Item.height = 42;
			Item.value = Item.sellPrice(0, 0, 3, 45);
			Item.rare = ItemRarityID.White;
			Item.useTime = 35;
			Item.UseSound = SoundID.DD2_BetsyWindAttack;
			slamStacks = 1;
			flagOffset = 6;
			auraRange = 10;
			duration = 1800;
			affectNearbyPlayers = true;
		}

		public override Color GetColor()
		{
			return new Color(205, 134, 71);
		}

		public override void NearbyPlayerEffect(Player player, OrchidGuardian guardian, bool isLocalPlayer, bool reinforced)
		{
			player.statDefense += 3;
			if (isLocalPlayer && reinforced) player.statDefense += 4;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.CopperBar, 8);
			recipe.AddIngredient(ItemID.Silk, 3);
			recipe.Register();
		}
	}
}
