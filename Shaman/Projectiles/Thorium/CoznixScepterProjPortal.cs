using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class CoznixScepterProjPortal : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Gate");
			Main.projFrames[projectile.type] = 4;
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 46;
			projectile.height = 46;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.timeLeft = 600;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * 0.75f;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			projectile.spriteDirection = projectile.velocity.X > 0f ? -1 : 1;

			if (Main.rand.Next(3) == 0)
			{
				int DustID = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 62, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 125, default(Color), 1f);
				Main.dust[DustID].noGravity = true;
			}

			projectile.ai[1]++;
			if (projectile.ai[1] >= 0)
			{
				int dmg = (int)(35 * modPlayer.shamanDamage + 5E-06f);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y + 4, 0f, 14f, mod.ProjectileType("CoznixScepterProjLaser"), dmg, 0f, projectile.owner, projectile.whoAmI, 0f);
				projectile.ai[1] = -360;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) { }

		public override void SafePostAI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter > 3)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
			}
			if (projectile.frame >= 4)
			{
				projectile.frame = 0;
				return;
			}
		}
	}
}