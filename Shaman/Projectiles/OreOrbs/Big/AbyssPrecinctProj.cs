using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Big
{
	public class AbyssPrecinctProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 36;
			Projectile.height = 36;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 100;
			Projectile.tileCollide = false;
			Projectile.alpha = 192;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abyss Bolt");
		}

		public override void AI()
		{
			Projectile.rotation += 0.1f;

			if (Projectile.timeLeft % 30 == 0)
			{
				spawnDustCircle(172, 50);
				spawnDustCircle(172, 100);
				spawnDustCircle(29, 75);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, Mod.Find<ModProjectile>("AbyssPrecinctProjExplosion").Type, Projectile.damage * 2, 0.0f, Projectile.owner, 0.0f, 0.0f);
				SoundEngine.PlaySound(SoundID.Item14);
			}

			int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 172);
			Main.dust[dust2].velocity /= 1f;
			Main.dust[dust2].scale = 1.7f;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].noLight = true;

			if (Projectile.localAI[0] == 0f)
			{
				AdjustMagnitude(ref Projectile.velocity);
				Projectile.localAI[0] = 1f;
			}

			Vector2 move = Vector2.Zero;
			float distance = 500f;
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
				Projectile.velocity = (10 * Projectile.velocity + move) / 3f;
				AdjustMagnitude(ref Projectile.velocity);
			}
		}

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			for (int i = 0; i < 20; i++)
			{
				double deg = (i * (36 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);

				float posX = Projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) - Projectile.width / 2 + Projectile.velocity.X + 4;
				float posY = Projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) - Projectile.height / 2 + Projectile.velocity.Y + 4;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index2].velocity = distToCenter == 50 ? Projectile.velocity : distToCenter == 75 ? Projectile.velocity * 0.75f : Projectile.velocity / 2;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = 1.5f;
				Main.dust[index2].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 172);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
				Main.dust[dust].scale = 1.5f;
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

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			Projectile.velocity *= 0.2f;
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, Mod.Find<ModProjectile>("AbyssPrecinctProjAlt").Type, Projectile.damage, 0.0f, Projectile.owner, 0.0f, 0.0f);

			if (modPlayer.shamanOrbBig != ShamanOrbBig.ABYSS)
			{
				modPlayer.shamanOrbBig = ShamanOrbBig.ABYSS;
				modPlayer.orbCountBig = 0;
			}
			modPlayer.orbCountBig++;

			if (modPlayer.orbCountBig == 3)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 30, player.position.Y - 30, 0f, 0f, Mod.Find<ModProjectile>("AbyssOrb").Type, 0, 0, Projectile.owner, 0f, 0f);

				if (player.FindBuffIndex(Mod.Find<ModBuff>("ShamanicBaubles").Type) > -1)
				{
					modPlayer.orbCountBig += 3;
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 15, player.position.Y - 38, 0f, 0f, Mod.Find<ModProjectile>("AbyssOrb").Type, 1, 0, Projectile.owner, 0f, 0f);
					player.ClearBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type);
				}
			}
			if (modPlayer.orbCountBig == 6)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 23, player.position.Y - 34, 0f, 0f, Mod.Find<ModProjectile>("AbyssOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 9)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 2, player.position.Y - 40, 0f, 0f, Mod.Find<ModProjectile>("AbyssOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 12)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + 19, player.position.Y - 34, 0f, 0f, Mod.Find<ModProjectile>("AbyssOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 15)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + 38, player.position.Y - 18, 0f, 0f, Mod.Find<ModProjectile>("AbyssOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig > 15)
			{
				player.AddBuff(Mod.Find<ModBuff>("AbyssEmpowerment").Type, 60 * 30);
				modPlayer.orbCountBig = -3;
			}
		}
	}
}