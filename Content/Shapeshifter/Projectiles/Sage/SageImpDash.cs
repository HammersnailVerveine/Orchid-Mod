using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Sage
{
	public class SageImpDash : OrchidModShapeshifterProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 26;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 2;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
		}

		public override void AI()
		{
			if (!Initialized)
			{
				SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);
				Vector2 position = Projectile.Center;
				Vector2 offSet = Projectile.velocity;

				for (int i = 0; i < 10; i++)
				{
					Dust dust = Dust.NewDustDirect(position - new Vector2(Projectile.width, Projectile.height) * 0.5f, Projectile.width, Projectile.height, DustID.Torch, Scale: Main.rand.NextFloat(1.2f, 1.2f));
					dust.noLight = true;
					dust.velocity.Y -= 1f;
				}

				Player player = Owner;
				for (int i = 0; i < 32; i++)
				{
					position += Collision.TileCollision(position, offSet, Projectile.width, Projectile.height, true, true, (int)player.gravDir);
					Dust dust = Dust.NewDustDirect(position - new Vector2(4, 4), 8, 8, DustID.Torch, Scale: Main.rand.NextFloat(1f, 1.3f));
					dust.noGravity = true;
				}

				for (int i = 0; i < 10; i++)
				{
					Dust dust = Dust.NewDustDirect(position - new Vector2(Projectile.width, Projectile.height) * 0.5f, Projectile.width, Projectile.height, DustID.Torch, Scale: Main.rand.NextFloat(1.2f, 1.5f));
					dust.noLight = true;
					dust.velocity.Y -= 1f;
				}
			}
		}
	}
}