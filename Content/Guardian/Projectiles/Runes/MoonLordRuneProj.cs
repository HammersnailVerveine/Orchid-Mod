using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Runes
{
	public class MoonLordRuneProj : GuardianRuneProjectile
	{
		public int TimeSpent = 0;
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;
		public float TargetDistance = 0;

		public override void RuneSetDefaults()
		{
			Projectile.width = 60;
			Projectile.height = 60;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			TargetDistance = 0;
		}

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.localAI[0] = Main.rand.Next(60);
		}

		public override bool SafeAI()
		{
			TimeSpent++;
			Projectile.rotation += 0.1f;
			TargetDistance = (float)(125 + Math.Sin(TimeSpent * 0.1f) * 25);
			Spin((float)Math.Sin(TimeSpent * 0.1f) + 1f);
			SetDistance(Distance + (TargetDistance - Distance) * 0.05f);
			Projectile.localAI[0]--;

			if (Projectile.localAI[0] <= 0 && IsLocalOwner)
			{
				int projType = ModContent.ProjectileType<MoonLordRuneProjAlt>();
				foreach (NPC npc in Main.npc)
				{
					if (IsValidTarget(npc) && npc.Center.Distance(Projectile.Center) < 320f)
					{
						Vector2 velocity = Vector2.Normalize(npc.Center - Projectile.Center) * 15f;
						Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity, projType, (int)(Projectile.damage * 0.75f), 0f, Projectile.owner);
						SoundEngine.PlaySound(SoundID.Item104, Projectile.Center);
						Projectile.localAI[0] = 60;
						if (Main.rand.NextBool()) break;
					}
				}
			}

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 pos = OldPosition[i];
				pos.Y -= Main.rand.NextFloat(4f);
				pos.X += Main.rand.NextFloat(4f) - 2f;
				OldPosition[i] = pos;
			}

			OldPosition.Add(Projectile.Center + new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f)));
			OldRotation.Add(Projectile.rotation);

			if (OldPosition.Count > 10)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}

			if (Main.rand.NextBool(10))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Vortex, Scale: Main.rand.NextFloat(0.8f, 1f));
				dust.velocity = dust.velocity * 0.5f + Owner.velocity * 0.25f;
				dust.noGravity = true;
			}
			return true;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			if (Projectile.timeLeft < 60) colorMult *= Projectile.timeLeft / 60f;

			for (int i = 0; i < OldPosition.Count - 1; i++)
			{
				Vector2 drawPosition = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPosition, null, Color.White * (i + 1) * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.1f, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);

			Vector2 drawPosition2 = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(TextureMain, drawPosition2, null, Color.White * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(TextureMain, drawPosition2, null, Color.White * colorMult, -Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale * 0.8f, SpriteEffects.FlipHorizontally, 0f);
			return false;
		}
	}
}