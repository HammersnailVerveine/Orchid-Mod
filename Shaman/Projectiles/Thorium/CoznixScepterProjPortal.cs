using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class CoznixScepterProjPortal : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Gate");
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 46;
			Projectile.height = 46;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 600;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * 0.75f;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidModPlayerShaman modPlayer = player.GetModPlayer<OrchidModPlayerShaman>();

			Projectile.spriteDirection = Projectile.velocity.X > 0f ? -1 : 1;

			if (Main.rand.Next(3) == 0)
			{
				int DustID = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 62, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 125, default(Color), 1f);
				Main.dust[DustID].noGravity = true;
			}

			Projectile.ai[1]++;
			if (Projectile.ai[1] >= 0)
			{
				int dmg = (int)(35 * modPlayer.shamanDamage + 5E-06f);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y + 4, 0f, 14f, Mod.Find<ModProjectile>("CoznixScepterProjLaser").Type, dmg, 0f, Projectile.owner, Projectile.whoAmI, 0f);
				Projectile.ai[1] = -360;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayerShaman modPlayer) { }

		public override void SafePostAI()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter > 3)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
			}
			if (Projectile.frame >= 4)
			{
				Projectile.frame = 0;
				return;
			}
		}
	}
}