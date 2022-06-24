using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles
{
	public class EnchantedScepterProj : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Bolt");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 50;
			Projectile.penetrate = 2;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			int Type = Main.rand.Next(3);
			if (Type == 0) Type = 15;
			if (Type == 1) Type = 57;
			if (Type == 2) Type = 58;
			int index2 = Dust.NewDust(Projectile.position - Projectile.velocity * 0.25f, Projectile.width, Projectile.height, Type, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(80, 110) * 0.013f);
			Main.dust[index2].velocity *= 0.2f;
			Main.dust[index2].noGravity = true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.penetrate--;
			if (Projectile.penetrate < 0) Projectile.Kill();
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X / 2;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y / 2;
			SoundEngine.PlaySound(SoundID.Item10);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 13; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 15);
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayerShaman modPlayer) { }
	}
}