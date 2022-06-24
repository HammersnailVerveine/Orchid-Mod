using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class UnfathomableFleshScepterProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 60;
			Projectile.scale = 1f;
			AIType = ProjectileID.Bullet;
			Projectile.alpha = 196;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flesh Bolt");
		}

		public override void AI()
		{
			Projectile.rotation += 0.2f;


			int dust = 258;

			switch (Main.rand.Next(2))
			{
				case 0:
					dust = 258;
					break;
				case 1:
					dust = 60;
					break;
				default:
					break;
			}
			for (int i = 0; i < 2; i++)
			{
				int DustID = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dust, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 125, default(Color), 1.25f);
				Main.dust[DustID].noGravity = true;
				Main.dust[DustID].velocity = -Projectile.velocity / 2;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 4; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 258);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = -Projectile.velocity / 2;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayerShaman modPlayer)
		{
			if (modPlayer.GetNbShamanicBonds() > 4 && player.statLifeMax2 > player.statLife)
			{
				if (Main.myPlayer == player.whoAmI)
					player.HealEffect(5, true);
				player.statLife += 5;

				Mod thoriumMod = OrchidMod.ThoriumMod;
				if (thoriumMod != null)
				{
					player.AddBuff((thoriumMod.Find<ModBuff>("LifeTransfusion").Type), 5 * 60);
				}
			}
		}
	}
}