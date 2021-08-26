using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Misc
{
	public class AttractiteShurikenProj : OrchidModAlchemistProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Attractite Shuriken");
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 22;
			projectile.height = 22;
			projectile.aiStyle = 2;
			projectile.timeLeft = 600;
			projectile.friendly = true;
			projectile.penetrate = 3;
		}

		public override void AI()
		{
			if (Main.rand.Next(4) == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 60);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale *= 1.5f;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			return true;
		}

		public override void SafeOnHitNPC(NPC target, OrchidModAlchemistNPC modTarget, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			target.AddBuff(BuffType<Alchemist.Buffs.Debuffs.Attraction>(), 60 * 5);
		}
	}
}