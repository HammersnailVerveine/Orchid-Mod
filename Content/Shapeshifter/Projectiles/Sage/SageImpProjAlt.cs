using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Sage
{
	public class SageImpProjAlt : OrchidModShapeshifterProjectile
	{
		public int Timespent = 0;

		public override void SafeSetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 160;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 900;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidShapeshifter shapeshifter)
		{
			target.AddBuff(BuffID.OnFire, 300);
		}

		public override void AI()
		{
			if (Main.rand.NextBool(3))
			{
				Dust dust2 = Dust.NewDustDirect(Projectile.position + new Vector2(8, Projectile.height), 16, 0, DustID.Smoke);
				dust2.scale = Main.rand.NextFloat(1.25f, 1.75f);
				dust2.velocity.Y = Main.rand.NextFloat(-1f, -0.25f);
				dust2.velocity.X *= 0.6f;
			}

			if (Main.rand.NextBool(3))
			{
				Dust dust2 = Dust.NewDustDirect(Projectile.position + new Vector2(8, Projectile.height), 16, 0, DustID.Smoke);
				dust2.scale = Main.rand.NextFloat(1f, 1.5f);
				dust2.velocity.Y = Main.rand.NextFloat(-3f, -2f);
				dust2.velocity.X *= 0.25f;
			}

			Dust dust = Dust.NewDustDirect(Projectile.position + new Vector2(8, Projectile.height), 16, 0, DustID.Torch);
			dust.scale = Main.rand.NextFloat(1.25f, 1.75f);
			dust.velocity.Y = Main.rand.NextFloat(-4f, -3f);
			dust.velocity.X *= 0.5f;

			int fireballType = ModContent.ProjectileType<SageImpProj>();
			foreach (Projectile projectile in Main.projectile)
			{
				if (projectile.type == fireballType && projectile.ai[1] == 0 && projectile.Hitbox.Intersects(Projectile.Hitbox))
				{
					projectile.ai[1] = 1f;
					projectile.velocity *= 1.25f;
					projectile.damage = (int)(projectile.damage * 1.5f);
					projectile.penetrate += 2;
					projectile.scale *= 1.1f;

					SoundEngine.PlaySound(SoundID.Item20, projectile.Center);

					for (int i = 0; i < 10; i++)
					{
						Dust dust2 = Dust.NewDustDirect(projectile.Center, 0, 0, DustID.Torch);
						dust2.scale = Main.rand.NextFloat(1.5f, 2f);
						dust2.noGravity = true;
						dust2.velocity *= 0.5f;
						dust2.velocity += Vector2.Normalize(projectile.velocity).RotatedByRandom(MathHelper.ToRadians(30f)) * Main.rand.NextFloat(5f, 8f);
					}

					for (int i = 0; i < 3; i++)
					{
						Dust dust2 = Dust.NewDustDirect(projectile.Center, 0, 0, DustID.Torch);
						dust2.scale = Main.rand.NextFloat(1.5f, 2f);
						dust2.noGravity = true;
						dust2.velocity *= 0.5f;
						dust2.velocity += Vector2.Normalize(projectile.velocity).RotatedByRandom(MathHelper.ToRadians(20f)) * Main.rand.NextFloat(10f, 15f);
					}
				}
			}
		}
	}
}