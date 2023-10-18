using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Misc
{
	public class AttractiteShurikenProj : OrchidModAlchemistProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Attractite Shuriken");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.aiStyle = 2;
			Projectile.timeLeft = 600;
			Projectile.friendly = true;
			Projectile.penetrate = 3;
		}

		public override void AI()
		{
			if (Main.rand.NextBool(4))
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale *= 1.5f;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			// SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
			return true;
		}

		public override void SafeOnHitNPC(NPC target, OrchidModAlchemistNPC modTarget, int damage, float knockback, bool crit, Player player, OrchidAlchemist modPlayer)
		{
			target.AddBuff(BuffType<Alchemist.Debuffs.Attraction>(), 60 * 5);
		}
	}
}