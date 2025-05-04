using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Shapeshifter.Buffs.Debuffs;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Sage
{
	public class SageBatHellProj : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureAlt;
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<Vector2> OldPosition2;
		public List<float> OldRotation;
		public Color DrawColor;
		public NPC LastTarget;
		public int Timespent = 0;
		public List<NPC> HitNPCs;

		public override void SafeSetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 60;
			Projectile.scale = 0.5f;
			Projectile.alpha = 96;
			Projectile.penetrate = 1;
			Projectile.alpha = 255;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			TextureAlt ??= ModContent.Request<Texture2D>(Texture.Replace("SageBatProjAlt", "SageBatProj"), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldPosition2 = new List<Vector2>();
			OldRotation = new List<float>();
			HitNPCs = new List<NPC>();
			DrawColor = new Color(255, 102, 20);
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (target.whoAmI != (int)Projectile.ai[2] || Projectile.penetrate != 1) return false;
			return base.CanHitNPC(target);
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidShapeshifter shapeshifter)
		{
			Projectile.penetrate = -1;
			Projectile.timeLeft = 10;
		}

		public override void AI()
		{
			OldPosition.Add(Projectile.Center);
			OldRotation.Add(Projectile.rotation);
			Timespent++;

			Projectile.rotation = Projectile.velocity.ToRotation();

			if (OldPosition.Count > 10 || Projectile.penetrate != 1)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}

			if (Timespent % 5 == 0)
			{
				OldPosition2.Add(Projectile.Center);

				if (OldPosition2.Count > 5)
				{
					OldPosition2.RemoveAt(0);
				}
			}

			if (Projectile.penetrate < 1) Projectile.velocity *= 0.75f;

			NPC target = Main.npc[(int)Projectile.ai[2]];

			if (IsValidTarget(target) && Projectile.timeLeft > 10 && Timespent < 200)
			{
				if (Projectile.ai[0] < 30)
				{ // Changes color when it becomes homing
					Projectile.tileCollide = false;
					Projectile.ai[0]++;
					Projectile.scale += 0.01f;
					DrawColor.G += 2;
					DrawColor.B += 2;
				}

				Projectile.velocity += (target.Center - Projectile.Center) * 0.025f;
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 9f;
				LastTarget = target;
			}
			else if (Projectile.timeLeft > 10)
			{
				Projectile.timeLeft = 10;
			}

			if (Main.rand.NextBool(4))
			{
				Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch).velocity *= 0.25f;
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			float scaleMult = 1.1f + (float)Math.Sin(Timespent * 0.15f) * 0.25f;
			if (Projectile.timeLeft < 7) colorMult *= Projectile.timeLeft / 7f;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition2 = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPosition2, null, DrawColor * 0.15f * (i + 1) * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.08f * scaleMult, SpriteEffects.None, 0f);
			}

			for (int i = 0; i < OldPosition2.Count; i++)
			{
				Vector2 drawPosition2 = OldPosition2[i] - Main.screenPosition;
				spriteBatch.Draw(TextureAlt, drawPosition2, null, DrawColor * 0.175f * (i + 1) * colorMult, OldRotation[i], TextureAlt.Size() * 0.5f, Projectile.scale * scaleMult * (i + 1) * 0.15f, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);

			return false;
		}
	}
}