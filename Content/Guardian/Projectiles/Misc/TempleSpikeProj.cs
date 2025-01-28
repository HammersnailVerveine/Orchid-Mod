using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Projectiles.Misc
{
	public class TempleSpikeProj : OrchidModGuardianProjectile
	{
        const int maxTime = 10;
		public override void SafeSetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.timeLeft = maxTime;
			Projectile.penetrate = -1;
			Projectile.hide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override void AI()
		{
			if (Projectile.rotation == 0)
			{
				Player player = Main.player[Projectile.owner];
				Projectile.rotation = Projectile.velocity.ToRotation();
				NPC target = null;
				float distanceClosest = 560f;
				foreach (NPC npc in Main.npc)
				{
					float distance = npc.Center.Distance(Projectile.Center);
					if (IsValidTarget(npc) && distance < distanceClosest)
					{
						float targetRotOffs = (npc.Center - Projectile.Center).ToRotation() - Projectile.rotation;
						if (targetRotOffs > MathHelper.Pi) targetRotOffs -= MathHelper.TwoPi;
						if (Math.Abs(targetRotOffs) < MathHelper.PiOver4)
						{
							target = npc;
							distanceClosest = distance;
						}
					}
				}
				if (target != null) Projectile.rotation = (target.Center - Projectile.Center).ToRotation();
				Projectile.velocity = Vector2.Zero;
				Projectile.friendly = true;
				Projectile.hide = false;
			}
			Projectile.scale = Projectile.timeLeft / (float)maxTime;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + new Vector2(560, 0).RotatedBy(Projectile.rotation)))
				return true;
			else return false;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			float drawRotation = Projectile.rotation + MathHelper.PiOver2;
			Vector2 drawIncrement = new Vector2(0, -8).RotatedBy(drawRotation);
			Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
			Color tWhite = new (1f, 1f, 1f, 0f);
			spriteBatch.Draw(texture, drawPosition, null, tWhite, drawRotation, new Vector2(9, 8), new Vector2(Projectile.scale, 1), SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, drawPosition + drawIncrement * 26f, new Rectangle(4, 0, 10, 2), tWhite, drawRotation, new Vector2(5, 1), new Vector2(Projectile.scale, 200), SpriteEffects.None, 0f);
			drawPosition += drawIncrement * 50;
			for (int i = 20; i > 0; i--)
			{
				spriteBatch.Draw(texture, drawPosition, new Rectangle(4, 0, 10, 2), tWhite, drawRotation, new Vector2(5, 4), new Vector2(Projectile.scale * i / 20f, 4), SpriteEffects.None, 0f);
				drawPosition += drawIncrement;
			}
			return false;
		}
	}
}