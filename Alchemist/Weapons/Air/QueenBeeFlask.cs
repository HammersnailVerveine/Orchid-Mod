using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Air
{
	public class QueenBeeFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 15;
			Item.width = 30;
			Item.height = 30;
			Item.rare = 3;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			this.potencyCost = 3;
			this.element = AlchemistElement.AIR;
			this.rightClickDust = 153;
			this.colorR = 255;
			this.colorG = 156;
			this.colorB = 12;
			this.secondaryDamage = 15;
			this.secondaryScaling = 2f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Royal Jelly");
			Tooltip.SetDefault("If no fire element is used, summons bees on impact"
							+ "\nHas a chance to release a catalytic beehive");
		}

		public override void KillSecond(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int nb = 2 + Main.rand.Next(2);
			if (alchProj.fireFlask.type == 0)
			{
				for (int i = 0; i < nb; i++)
				{
					Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
					int spawnProj = ProjectileType<Alchemist.Projectiles.Air.QueenBeeFlaskProj>();
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
				}
				int dmg = getSecondaryDamage(player, modPlayer, alchProj.nbElements);
				int rand = alchProj.nbElements + Main.rand.Next(3) + 1;
				for (int i = 0; i < rand; i++)
				{
					Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(80)));
					if (player.strongBees && Main.rand.Next(2) == 0)
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, 566, (int)(dmg * 1.15f), 0f, projectile.owner, 0f, 0f);
					else
					{
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, 181, dmg, 0f, projectile.owner, 0f, 0f);
					}
				}
			}
		}

		public override void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer,
		OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int rand = alchProj.nbElements;
			if (Main.rand.Next(10) < rand)
			{
				int dmg = OrchidModAlchemistHelper.getSecondaryDamage(player, modPlayer, alchProj.nbElements);
				int proj = ProjectileType<Alchemist.Projectiles.Reactive.AlchemistHive>();
				Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, proj, dmg, 0f, projectile.owner);
			}
		}
	}
}
