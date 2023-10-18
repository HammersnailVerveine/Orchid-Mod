using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Alchemist.Projectiles.Air
{
	public class CloudInAVialProj : OrchidModAlchemistProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 100;
			Projectile.height = 100;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 3;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Airxplosion");
		}

		public override void AI()
		{
			for (int l = 0; l < Main.npc.Length; l++)
			{
				NPC target = Main.npc[l];
				if (Projectile.Hitbox.Intersects(target.Hitbox) && !(target.boss || target.type == NPCID.TargetDummy) && target.knockBackResist > 0f)
				{
					target.velocity.Y = -(Projectile.ai[1] * 4);
				}
			}
		}
	}
}