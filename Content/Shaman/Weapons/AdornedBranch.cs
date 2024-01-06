using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using OrchidMod.Content.Shaman.Projectiles;

namespace OrchidMod.Content.Shaman.Weapons
{
	public class AdornedBranch : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 6;
			Item.width = 34;
			Item.height = 32;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.knockBack = 1.25f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.UseSound = SoundID.Item8;
			Item.shootSpeed = 3f;
			Item.shoot = ModContent.ProjectileType<AdornedBranchProj>();
			Element = ShamanElement.FIRE;
			catalystMovement = ShamanSummonMovement.FLOATABOVE;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Adorned Branch");
			/* Tooltip.SetDefault("Shoots a burst of splinters"
							  + "\nShoots more projectiles if you have 2 or more active shamanic bonds"); */
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int randRef = modPlayer.GetNbShamanicBonds() > 0 ? 6 : 3;
			int rand = Main.rand.Next(randRef) + 3;
			for (int i = 0; i < rand; i++)
			{
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(30));
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				newVelocity *= scale;
				NewShamanProjectile(player, source, position, newVelocity, type, damage, knockback);
			}
			return false;
		}

		public override void CatalystSummonAI(Projectile projectile, int timeSpent)
		{
			if (timeSpent % (Item.useTime * 3) == 0)
			{
				Vector2 target = OrchidModProjectile.GetNearestTargetPosition(projectile);
				if (target!= Vector2.Zero)
				{
					int rand = Main.rand.Next(3) + 3;
					for (int i = 0; i < rand; i++)
					{
						target.Y -= 24;
						Vector2 velocity = target - projectile.Center;
						velocity.Normalize();
						velocity = velocity.RotatedByRandom(MathHelper.ToRadians(30)) * Item.shootSpeed;
						float scale = 1f - (Main.rand.NextFloat() * .3f);
						velocity *= scale;
						NewShamanProjectileFromProjectile(projectile, velocity, Item.shoot, projectile.damage, projectile.knockBack);
					}
				}
			}
		}
	}
}

