using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian.Projectiles.Shields;
using OrchidMod.Content.Guardian.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Shields
{
	public class MagnetosphereShield : OrchidModGuardianShield
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.width = 42;
			Item.height = 50;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item88;
			Item.knockBack = 6f;
			Item.damage = 702;
			Item.rare = ItemRarityID.Red;
			Item.useTime = 60;
			Item.shootSpeed = 12f;
			distance = 70f;
			slamDistance = 150f;
			blockDuration = 300;
		}

		public override void Slam(Player player, Projectile shield)
		{
			Projectile anchor = GetAnchor(player).Projectile;
			int type = ModContent.ProjectileType<MagnetosphereShieldProj>();
			Vector2 dir = Vector2.Normalize(Main.MouseWorld - player.Center) * Item.shootSpeed;
			Projectile.NewProjectile(Item.GetSource_FromThis(), anchor.Center, dir, type, (int)(shield.damage * 0.5f), Item.knockBack, player.whoAmI);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient<GuardianFragmentMaterial>(18);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
