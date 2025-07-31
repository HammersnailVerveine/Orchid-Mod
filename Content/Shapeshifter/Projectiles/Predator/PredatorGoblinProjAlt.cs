using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Predator
{
	public class PredatorGoblinProjAlt : OrchidModShapeshifterProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.friendly = false;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 30;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			NPC npc = Main.npc[(int)Projectile.ai[0]];
			if (IsValidTarget(npc))
			{
				Projectile.Center = npc.Center;

				if (Projectile.timeLeft % 6 == 0)
				{
					int projType = ModContent.ProjectileType<PredatorGoblinProjSlash>();
					ShapeshifterNewProjectile(Projectile.Center, Vector2.Zero, projType, Projectile.damage, Projectile.CritChance, 0f, Projectile.owner, Main.rand.NextFloat(MathHelper.TwoPi), Projectile.ai[0]);
				}
			}
			else
			{
				Projectile.Kill();
			}
		}
	}
}