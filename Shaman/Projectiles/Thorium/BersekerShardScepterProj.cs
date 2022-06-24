using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class BersekerShardScepterProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 60;
			Projectile.scale = 1f;
			Projectile.alpha = 196;
			Projectile.penetrate = 2;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Berserker Bolt");
		}

		public override void AI()
		{
			Projectile.rotation += 0.2f;
			Projectile.velocity *= 1.01f;

			int dust = Main.rand.Next(2) == 0 ? 258 : 60;

			for (int i = 0; i < 2; i++)
			{
				int DustID = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dust, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 125, default(Color), 1.25f);
				Main.dust[DustID].noGravity = true;
				Main.dust[DustID].scale *= dust == 60 ? 1.5f : 1f;
				Main.dust[DustID].velocity = Projectile.velocity / 3;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 4; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Projectile.velocity / 2;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			if (modPlayer.GetNbShamanicBonds() > 4)
			{
				Mod thoriumMod = OrchidMod.ThoriumMod;
				if (thoriumMod != null)
				{
					target.AddBuff((thoriumMod.Find<ModBuff>("BerserkSoul").Type), 5 * 60);
				}
			}
		}
	}
}