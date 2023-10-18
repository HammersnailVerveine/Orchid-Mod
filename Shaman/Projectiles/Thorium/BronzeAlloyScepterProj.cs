using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class BronzeAlloyScepterProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 32;
			Projectile.scale = 1f;
			AIType = ProjectileID.Bullet;
			this.projectileTrail = true;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Basilisk Tooth");
		}

		public override void AI()
		{
			if (Main.rand.Next(2) == 0)
			{
				int DustID = Dust.NewDust(new Vector2(Projectile.position.X + 4, Projectile.position.Y + 4), Projectile.width / 3, Projectile.height / 3, ModContent.DustType<Content.Dusts.ToxicDust>(), Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 125, default(Color), 1.25f);
				Main.dust[DustID].noGravity = true;
				Main.dust[DustID].velocity *= 0f;
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 4; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Content.Dusts.ToxicDust>());
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Projectile.velocity / 2;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			target.AddBuff(BuffID.Poisoned, 60 * 3 * modPlayer.GetNbShamanicBonds());
		}
	}
}