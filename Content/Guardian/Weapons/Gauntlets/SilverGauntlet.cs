using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Gauntlets
{
	public class SilverGauntlet : OrchidModGuardianGauntlet
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 32;
			Item.knockBack = 5f;
			Item.damage = 45;
			Item.value = Item.sellPrice(0, 0, 8, 40);
			Item.rare = ItemRarityID.White;
			Item.useTime = 30;
			strikeVelocity = 15f;
			parryDuration = 60;
			color = new Color(209, 216, 217);
		}

		public override void OnHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, NPC.HitInfo hit, bool charged)
		{
			if (charged) guardian.AddBlock(1);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.SilverBar, 8);
			recipe.Register();
		}
	}
}
