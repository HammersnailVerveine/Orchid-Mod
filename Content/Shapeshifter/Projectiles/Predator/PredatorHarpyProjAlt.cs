using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Predator
{
	public class PredatorHarpyProjAlt : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void SafeSetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 40;
			Projectile.scale = 0.8f;
			Projectile.alpha = 96;
			Projectile.penetrate = 3;
			Projectile.alpha = 255;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if ((int)Projectile.ai[0] == -1 || (int)Projectile.ai[0] == target.whoAmI)
			{
				return null;

			}
			return false;
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 40 && Projectile.penetrate == 3)
			{
				Projectile.ai[0] = -1;
			}

			if (Projectile.penetrate < 3)
			{
				NPC npc = Main.npc[(int)Projectile.ai[0]];
				if (npc.active)
				{
					Projectile.velocity *= 0.6f;
					Projectile.ai[1] -= Projectile.velocity.X;
					Projectile.ai[2] -= Projectile.velocity.Y;

					Projectile.position.X = npc.position.X - Projectile.ai[1];
					Projectile.position.Y = npc.position.Y - Projectile.ai[2];
				}
				else 
				{
					Projectile.Kill();
				}
			}

			Projectile.rotation = Projectile.velocity.ToRotation();

			OldPosition.Add(Projectile.Center);
			OldRotation.Add(Projectile.rotation);

			if (OldPosition.Count > 15)
			{
				OldPosition.RemoveAt(0);
				OldRotation.RemoveAt(0);
			}

			Projectile.velocity *= 0.95f;
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidShapeshifter shapeshifter)
		{
			if (Projectile.ai[0] == -1)
			{
				Projectile.ai[0] = target.whoAmI;
				Projectile.ai[1] = target.position.X - Projectile.position.X;
				Projectile.ai[2] = target.position.Y - Projectile.position.Y;
			}
			Projectile.damage = (int)(Projectile.damage * 0.7f);
			Projectile.timeLeft = 70;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			SoundEngine.PlaySound(SoundID.Item64, Projectile.Center);
			return true;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			float colorMult = 1f;
			if (Projectile.timeLeft < 8) colorMult *= Projectile.timeLeft / 8f;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition2 = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPosition2, null, lightColor * 0.05f * (i + 1) * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.065f, SpriteEffects.None, 0f);
			}

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);


			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(TextureMain, drawPosition, null, lightColor * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}