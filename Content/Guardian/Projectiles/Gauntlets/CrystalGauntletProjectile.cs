using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Gauntlets
{
	public class CrystalGauntletProjectile : OrchidModGuardianProjectile
	{

		public override void SafeSetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.timeLeft = 40;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Main.projFrames[Projectile.type] = 3;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 5;
			Projectile.penetrate = 3; //effectively 2, see safeonhitnpc
			Projectile.ArmorPenetration = 50;
		}
		
		public override void AI()
		{
			if (Projectile.timeLeft == 40) Projectile.frame = Main.rand.Next(3);
			Projectile.rotation += Projectile.velocity.X;
			Projectile.light = Projectile.scale / 2f;
			Projectile.velocity *= 0.92f;
			if (Projectile.timeLeft < 17) Projectile.scale -= 0.05f;
		}
		
		public override Color? GetAlpha(Color color)
		{
			return new Color(Math.Max(200, (int)color.R), Math.Max(200, (int)color.G), Math.Max(200, (int)color.B), 50);
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidGuardian guardian)
		{
			//dies before reaching 1 penetration so it still applies idstatic iframes
			if (Projectile.penetrate == 2) Projectile.Kill();
		}

		public override void OnKill(int timeLeft)
		{
			if (timeLeft > 10)
			for (int i = 0; i < 2; i++)
			{
				Dust shardDust = Projectile.frame switch
				{
					0 => Dust.NewDustPerfect(Projectile.Center, DustID.BlueCrystalShard),
					1 => Dust.NewDustPerfect(Projectile.Center, DustID.PurpleCrystalShard),
					_ => Dust.NewDustPerfect(Projectile.Center, DustID.UndergroundHallowedEnemies) //pinkcrystalshard is ugly for some reason. wasn't implemented right? idk
				};
				shardDust.alpha = 100;
				shardDust.velocity *= Main.rand.NextFloat(0.4f);
				shardDust.noGravity = true;
			}
		}
	}
}