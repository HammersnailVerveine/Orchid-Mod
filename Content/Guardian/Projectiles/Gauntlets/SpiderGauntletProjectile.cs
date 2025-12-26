using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Utilities;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Gauntlets
{
	public class SpiderGauntletProjectile : OrchidModGuardianProjectile
	{
		private static Texture2D TextureMain;
		private static Texture2D TextureAlt;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void SafeSetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 60;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = 1;
			Projectile.alpha = 255;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			TextureAlt ??= ModContent.Request<Texture2D>(Texture + "_Alt", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 60 && Projectile.ai[0] == 1) Strong = true;
			OldPosition.Add(Projectile.Center);
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (OldPosition.Count > 5) OldPosition.RemoveAt(0);

			Projectile.ai[1]++;
			if (Projectile.ai[1] > 20) Projectile.velocity *= 0.8f;
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidGuardian guardian)
		{
			if (Strong)
			{
				target.AddBuff(BuffID.Venom, 300);
				if (FirstHit && !player.dead) guardian.GuardianGuardRecharging += 0.5f;
			}
			else
			{
				target.AddBuff(BuffID.Venom, 180);
				if (FirstHit && !player.dead)
				{
						guardian.GuardianSlamRecharging += guardian.GauntletSlamPool;
						guardian.GauntletSlamPool *= 0.8f;
				}
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

			// Draw code here

			float colorMult = 1f;
			if (Projectile.timeLeft < 10) colorMult *= Projectile.timeLeft / 10f;
			Texture2D texture = Projectile.ai[0] == 1f ? TextureAlt : TextureMain;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPositionTrail = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(texture, drawPositionTrail, null, lightColor * 0.2f * (i + 1) * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.20f, SpriteEffects.None, 0f);
			}

			// Draw code ends here

			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);

			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(texture, drawPosition, null, lightColor * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale + Projectile.ai[0] * 0.1f, SpriteEffects.None, 0f);
			return false;
		}
	}
}