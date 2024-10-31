using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shapeshifter.Buffs.Debuffs;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Warden
{
	public class WardenSpiderProj : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureMain;
		public List<float> OldAI;
		public Color drawColor;

		public override void SafeSetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 30;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
			OldAI = new List<float>();
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 30)
			{
				Projectile.rotation = (Owner.Center - Projectile.Center).ToRotation();
				Projectile.localAI[0] = -32f;
				Projectile.localAI[1] = -0.75f;

				if (Main.rand.NextBool())
				{
					drawColor = new Color(227, 101, 28);
				}
				else
				{
					drawColor = new Color(234, 173, 83);
				}
			}

			Projectile.friendly = Projectile.timeLeft > 20;
			Projectile.localAI[0] *= 0.8f;
			Projectile.localAI[1] *= 0.8f;
			Projectile.velocity *= 0.8f;

			if (Projectile.timeLeft > 15)
			{
				OldAI.Add(Projectile.localAI[0]);

				if (OldAI.Count > 10)
				{
					OldAI.RemoveAt(0);
				}
			}
			else
			{
				if (OldAI.Count > 0)
				{
					OldAI.RemoveAt(0);
				}
			}
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidShapeshifter shapeshifter)
		{
			target.AddBuff(BuffID.Poisoned, 300);

			if (target.HasBuff<WardenSpiderDebuff>())
			{
				shapeshifter.modPlayer.TryHeal(1 + Main.rand.Next(2));
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			if (Projectile.timeLeft < 7) colorMult *= Projectile.timeLeft / 7f;

			if (lightColor.R < 96) lightColor.R = 96;
			if (lightColor.G < 96) lightColor.G = 96;
			if (lightColor.B < 96) lightColor.B = 96;

			for (int i = -1; i < 2 ; i += 2) 
			{
				SpriteEffects effect = i < 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;

				for (int j = 0; j < OldAI.Count; j++)
				{
					Vector2 drawPosition2 = Projectile.Center - new Vector2(0f, (OldAI[j] + 0.35f) * i).RotatedBy(Projectile.rotation) - Main.screenPosition;
					spriteBatch.Draw(TextureMain, drawPosition2, null, lightColor * 0.06f * (j + 1) * colorMult, Projectile.rotation + (Projectile.localAI[1] - 0.2f) * i, TextureMain.Size() * 0.5f, Projectile.scale, effect, 0f);
				}

				Vector2 drawPosition = Projectile.Center - new Vector2(0f, (Projectile.localAI[0] + 0.35f) * i).RotatedBy(Projectile.rotation) - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPosition, null, lightColor.MultiplyRGBA(drawColor) * colorMult, Projectile.rotation + (Projectile.localAI[1] - 0.2f) * i, TextureMain.Size() * 0.5f, Projectile.scale, effect, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			return false;
		}
	}
}