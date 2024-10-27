using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using OrchidMod.Content.Shaman.Projectiles;
using OrchidMod.Common.ModObjects;

namespace OrchidMod.Content.Shaman.Projectiles.Fire
{
	public class AdornedBranchProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 5;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 100;
			Projectile.extraUpdates = 1;
		}

		public override void SafeAI()
		{
			if (Projectile.timeLeft == 100 || Projectile.timeLeft == 1)
			{
				int dustType = 31;
				Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
				Main.dust[Dust.NewDust(pos, Projectile.width, Projectile.height, dustType)].velocity *= 0.25f;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer) { }
	}
}

