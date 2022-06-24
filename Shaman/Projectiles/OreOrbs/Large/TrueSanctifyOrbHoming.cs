using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Large
{
	public class TrueSanctifyOrbHoming : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Sanctify Orb");
		}
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.timeLeft = 12960000;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			Main.projFrames[Projectile.type] = 24;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			Projectile.timeLeft = 350;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();

			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 65)
				Projectile.frame = 1;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 70)
				Projectile.frame = 2;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 75)
				Projectile.frame = 3;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 80)
				Projectile.frame = 4;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 85)
				Projectile.frame = 5;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 90)
				Projectile.frame = 6;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 95)
				Projectile.frame = 7;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 100)
				Projectile.frame = 8;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 105)
				Projectile.frame = 9;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 110)
				Projectile.frame = 10;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 115)
				Projectile.frame = 11;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 0)
				Projectile.frame = 12;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 5)
				Projectile.frame = 13;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 10)
				Projectile.frame = 14;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 15)
				Projectile.frame = 15;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 20)
				Projectile.frame = 16;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 25)
				Projectile.frame = 17;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 30)
				Projectile.frame = 18;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 35)
				Projectile.frame = 19;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 40)
				Projectile.frame = 20;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 45)
				Projectile.frame = 21;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 50)
				Projectile.frame = 22;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 55)
				Projectile.frame = 23;
			if (player.GetModPlayer<OrchidShaman>().modPlayer.timer120 == 60)
				Projectile.frame = 0;

			if (Main.rand.Next(5) == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 254);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity /= 2f;
			}

			if (Projectile.localAI[0] == 0f)
			{
				AdjustMagnitude(ref Projectile.velocity);
				Projectile.localAI[0] = 1f;
			}
			Vector2 move = Vector2.Zero;
			float distance;
			if (Projectile.timeLeft < 300)
				distance = 1000f;
			else distance = 10f;
			bool target = false;
			for (int k = 0; k < 200; k++)
			{
				if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy)
				{
					Vector2 newMove = Main.npc[k].Center - Projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo < distance)
					{
						move = newMove;
						distance = distanceTo;
						target = true;
					}
				}
			}
			if (target)
			{
				AdjustMagnitude(ref move);
				Projectile.velocity = (10 * Projectile.velocity + move) / 7f;
				AdjustMagnitude(ref Projectile.velocity);
			}
		}
		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 6f)
			{
				vector *= 6f / magnitude;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 254);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer) { }
	}
}
