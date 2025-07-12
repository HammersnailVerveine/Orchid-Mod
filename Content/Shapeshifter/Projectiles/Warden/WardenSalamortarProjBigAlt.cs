using Terraria;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Warden
{
	public class WardenSalamortarProjBigAlt : OrchidModShapeshifterProjectile
	{
		bool Healed = false;

		public override void SafeSetDefaults()
		{
			Projectile.width = 200;
			Projectile.height = 200;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 2;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
			Projectile.friendly = true;
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidShapeshifter shapeshifter)
		{
			if (!Healed)
			{
				Healed = true;
				shapeshifter.modPlayer.TryHeal(shapeshifter.GetShapeshifterHealing(Projectile.ai[0] * 5));
			}
		}
	}
}