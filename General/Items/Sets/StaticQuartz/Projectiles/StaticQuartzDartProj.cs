using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.General.Items.Sets.StaticQuartz.Projectiles
{
	public class StaticQuartzDartProj : ModProjectile
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Static Quartz Dart");
		}

		public override void SetDefaults() {
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.penetrate = 5;
			projectile.timeLeft = 600;
			projectile.tileCollide = true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) {
			Main.PlaySound(SoundID.Item10, projectile.Center);
			projectile.velocity *= 0f;
			projectile.aiStyle = 0;
			projectile.damage = (int)(projectile.damage * 2f);
			return false;
		}

		public override void Kill(int timeLeft) {
			Main.PlaySound(SoundID.Item10, projectile.Center);
		}
	}
}
