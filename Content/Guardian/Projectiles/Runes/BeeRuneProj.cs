using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Runes
{
	public class BeeRuneProj : GuardianRuneProjectile
	{
		public int TimeSpent = 0;
		private static Texture2D TextureMain;

		public override void RuneSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
		}

		public override void FirstFrame()
		{
			TimeSpent += Main.rand.Next(120);
		}

		public override bool SafeAI()
		{
			TimeSpent++;
			Projectile.rotation = (float)Math.Sin(TimeSpent * (MathHelper.Pi / 120f)) * 0.4f;
			Player player = Owner;

			int rand = 90;
			if (player.strongBees)
			{ // more bees with hive pack
				rand -= 18;
			}

			if (Main.rand.NextBool(rand))
			{
				foreach (NPC npc in Main.npc)
				{
					if (IsValidTarget(npc) && npc.Center.Distance(Projectile.Center) < 160f)
					{
						if (Main.player[Projectile.owner].strongBees && Main.rand.NextBool(2))
							Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * 5f, ProjectileID.GiantBee, (int)(Projectile.damage * 1.15f), 0f, Projectile.owner);
						else
							Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * 5f, ProjectileID.Bee, Projectile.damage, 0f, Projectile.owner);
						break;
					}
				}
			}

			return true;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			float colorMult = 1f;
			if (Projectile.timeLeft < 60) colorMult *= Projectile.timeLeft / 60f;
			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(TextureMain, drawPosition, null, Color.White * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}