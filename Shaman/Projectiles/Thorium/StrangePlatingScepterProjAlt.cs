using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class StrangePlatingScepterProjAlt : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			projectile.width = 6;
			projectile.height = 6;
			projectile.friendly = true;
			projectile.aiStyle = 1;
			projectile.timeLeft = 35;
			projectile.scale = 1f;
			aiType = ProjectileID.Bullet;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Lighting.AddLight(projectile.position, 0.75f, 0f, 0f);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Laser");
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) { }
	}
}