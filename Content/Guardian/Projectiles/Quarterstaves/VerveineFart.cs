using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;

namespace OrchidMod.Content.Guardian.Projectiles.Quarterstaves
{
	public class VerveineFart : OrchidModGuardianProjectile
	{
		public override string Texture => $"Terraria/Images/Gore_435";

		public override void SafeSetDefaults()
		{
			Projectile.width = 80;
			Projectile.height = 80;
			Projectile.timeLeft = 90;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
		}

		public override void SafeModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.HitDirectionOverride = target.velocity.X > 0 ? -1 : 1;
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidGuardian guardian)
		{
			target.AddBuff(BuffID.Poisoned, 300);
			target.AddBuff(BuffID.Stinky, 300);
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(BuffID.Poisoned, 300);
			target.AddBuff(BuffID.Stinky, 300);
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 90)
			{
				SoundEngine.PlaySound(SoundID.Item16, Projectile.Center);
				for (int i = 0; i < 16; i++)
				{
					Gore fartCloud = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.UnitY * 1.5f, GoreID.FartCloud1 + Main.rand.Next(3));
					fartCloud.velocity.X *= 0.5f;
					fartCloud.velocity.Y *= 1.5f;
				}
			}
		}
	}
}