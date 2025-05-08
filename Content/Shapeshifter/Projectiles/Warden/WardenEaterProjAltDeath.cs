using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Warden
{
	public class WardenEaterProjAltDeath : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public int Timespent;

		public override void SafeSetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = false;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 180;
			Projectile.scale = 1f;
			Projectile.penetrate = 1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			Timespent = 0;
		}

		public override void AI()
		{ // Homes towards its targeted fruit then dies
			OldPosition.Add(Projectile.Center);
			Timespent++;

			Projectile.rotation = Projectile.velocity.ToRotation();

			if (OldPosition.Count > 10)
			{
				OldPosition.RemoveAt(0);
			}

			if (Projectile.timeLeft < 10)
			{
				Projectile.velocity *= 0.5f;
				return;
			}

			Projectile target = Main.projectile[(int)Projectile.ai[0]];

			if (target.active && target.type == ModContent.ProjectileType<WardenEaterProjAlt>() && target.frame != 0)
			{ // target is ripe, pick another one
				float distanceMax = 400f;
				int targetID = -1;
				foreach (Projectile projectile in Main.projectile)
				{
					float distance = Projectile.Center.Distance(projectile.Center);
					if (projectile.type == ModContent.ProjectileType<WardenEaterProjAlt>() && projectile.owner == Projectile.owner && distance < distanceMax && projectile.frame == 0)
					{
						distanceMax = distance;
						targetID = projectile.whoAmI;
					}
				}

				if (targetID >= 0)
				{
					target = Main.projectile[targetID];
					Projectile.ai[0] = targetID;
					Projectile.netUpdate = true;
				}
			}

			if (Projectile.timeLeft > 10 && target.active && target.type == ModContent.ProjectileType<WardenEaterProjAlt>())
			{
				float speed = Timespent * 0.04f;
				if (speed > 8f)
				{
					speed = 8f;
				}
				
				Projectile.velocity += (target.Center - Projectile.Center) * 0.025f;
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * speed;
			}
			else if (Projectile.timeLeft > 10)
			{
				Projectile.timeLeft = 10;
				return;
			}

			if (Projectile.Center.Distance(target.Center) < 8f)
			{
				Projectile.timeLeft = 10;
				SoundStyle soundStyle = SoundID.Item4;
				soundStyle.Volume *= 0.25f;
				soundStyle.Pitch -= 0.3f;
				SoundEngine.PlaySound(soundStyle, Projectile.Center);

				if (target.owner == Main.myPlayer && target.frame == 0)
				{
					target.ai[2]++;
					target.netUpdate = true;
				}

				for (int i = 0; i < 3; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.CursedTorch);
					dust.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(10f)) * Main.rand.NextFloat(0.4f, 0.75f);
					dust.scale = Main.rand.NextFloat(1f, 1.5f);
					dust.noGravity = true;
				}
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			if (Projectile.timeLeft < 7) colorMult *= Projectile.timeLeft / 7f;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition2 = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPosition2, null, Color.White * 0.15f * (i + 1) * colorMult, 0f, TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.08f, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);

			return false;
		}
	}
}