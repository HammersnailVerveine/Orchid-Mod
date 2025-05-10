using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Sage
{
	public class SageCorruptionProj : OrchidModShapeshifterProjectile
	{
		public int Timespent = 0;

		public override void SafeSetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.friendly = false;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 900;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			if (!Initialized)
			{
				Initialized = true;

				for (int i = 0; i < 10; i ++)
				{
					Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.ToxicBubble);
					dust2.scale = Main.rand.NextFloat(1.2f, 1.5f);
					dust2.noGravity = true;
				}
			}

			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.ToxicBubble);
			dust.scale = Main.rand.NextFloat(1.2f, 1.5f);
			dust.velocity *= 0.1f;
			dust.velocity += Projectile.velocity * 0.25f;
			dust.noGravity = true;

			Dust dust4 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.CorruptGibs);
			dust4.scale = Main.rand.NextFloat(0.8f, 1.2f);
			dust4.velocity *= 0.1f;
			dust4.velocity += Projectile.velocity * 0.25f;
			dust4.noGravity = true;

			Vector2 targetLocation = new Vector2(Projectile.ai[0], Projectile.ai[1]);
			if (targetLocation != Vector2.Zero)
			{
				Vector2 velocity = Vector2.Normalize(targetLocation - Projectile.Center) * 0.3f;
				Projectile.velocity += velocity;
				if (velocity.Length() > 16f)
				{
					Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 0.1f;
				}

				if (Projectile.Center.Distance(targetLocation) < 16f)
				{
					Projectile.Kill();

					int projectileType = ModContent.ProjectileType<SageCorruptionProjAlt>();
					Projectile newProjectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), targetLocation, Vector2.Zero, projectileType, Projectile.damage, 0f, Projectile.owner);
					newProjectile.CritChance = Projectile.CritChance;
					newProjectile.netUpdate = true;
				}
			}
		}
	}
}