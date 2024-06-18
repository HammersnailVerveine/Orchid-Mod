using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian.Projectiles.Shields;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Shields
{
	public class SpectreShield : OrchidModGuardianShield
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 4, 32, 0);
			Item.width = 38;
			Item.height = 46;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item88;
			Item.knockBack = 6f;
			Item.damage = 183;
			Item.rare = ItemRarityID.Yellow;
			Item.useTime = 60;
			Item.shootSpeed = 13f;
			distance = 65f;
			bashDistance = 140f;
			blockDuration = 240;
		}

		public override void Slam(Player player, Projectile shield)
		{
			Projectile anchor = GetAnchor(player).Projectile;
			int type = ModContent.ProjectileType<SpectreShieldProj>();
			for (int i = 0; i < 3 + Main.rand.Next(5); i++)
			{
				Vector2 dir = Vector2.Normalize(Main.MouseWorld - player.Center).RotatedByRandom(MathHelper.ToRadians(30f)) * Item.shootSpeed;
				Projectile.NewProjectile(Item.GetSource_FromThis(), anchor.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * Main.rand.NextFloat(16f), dir, type, (int)(shield.damage * 0.75f), Item.knockBack, player.whoAmI);
			}
		}

		public override void Block(Player player, Projectile shield, Projectile projectile)
		{
			Projectile anchor = GetAnchor(player).Projectile;
			int type = ModContent.ProjectileType<SpectreShieldProj>();
			Vector2 dir = Vector2.Normalize(Main.MouseWorld - player.Center).RotatedByRandom(MathHelper.ToRadians(30f)) * Item.shootSpeed;
			Projectile.NewProjectile(Item.GetSource_FromThis(), anchor.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * Main.rand.NextFloat(16f), dir, type, (int)(shield.damage * 0.75f), Item.knockBack, player.whoAmI);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddIngredient(ItemID.SpectreBar, 18);
			recipe.Register();
		}
	}
}
