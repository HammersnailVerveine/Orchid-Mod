using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.General.Items.Sets.StaticQuartz.Projectiles
{
	public class StaticQuartzDartProj : ModProjectile
	{
		private bool collided = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Static Quartz Dart");
		}

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 5;
			Projectile.timeLeft = 600;
			Projectile.tileCollide = true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
			Projectile.velocity *= 0f;
			Projectile.aiStyle = 0;
			if (!collided)
			{
				Projectile.damage = (int)(Projectile.damage * 2f);
				collided = true;
			}
			return false;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
		}
	}
}
