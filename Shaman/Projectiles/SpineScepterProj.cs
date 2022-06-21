using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles
{
	public class SpineScepterProj : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spine Beam");
		}
		public override void SafeSetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.penetrate = 5;
			Projectile.timeLeft = 30;
			Projectile.extraUpdates = 10;
			Projectile.ignoreWater = true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			for (int i = 0; i < 2; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 258);
				Main.dust[dust].velocity /= 3f;
				Main.dust[dust].scale = 1.3f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Kill();
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 7; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 258);
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) { }
	}
}