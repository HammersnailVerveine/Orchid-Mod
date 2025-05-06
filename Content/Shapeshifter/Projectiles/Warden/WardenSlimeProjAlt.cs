using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Content.Shapeshifter.Weapons.Warden;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter.Projectiles.Warden
{
	public class WardenSlimeProjAlt : OrchidModShapeshifterProjectile
	{
		private static Texture2D TextureMain;
		public List<Vector2> OldPosition;
		public List<float> OldRotation;

		public override void SafeSetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 240;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = 3;
			Projectile.alpha = 255;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.extraUpdates = 1;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.penetrate < 1) return false;
			return base.CanHitNPC(target);
		}

		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone, Player player, OrchidShapeshifter shapeshifter)
		{
			if (Projectile.penetrate == 1)
			{
				Projectile.penetrate = -1;
				Projectile.timeLeft = 10;
			}

			if (shapeshifter.IsShapeshifted)
			{
				if (shapeshifter.ShapeshiftAnchor.ShapeshifterItem.ModItem is WardenSlime slime)
				{
					slime.TrySpawnHealingGoo(shapeshifter.ShapeshiftAnchor.Projectile, player);
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke);
				dust.scale = Main.rand.NextFloat(0.6f, 0.8f);
				dust.velocity *= Main.rand.NextFloat(0.25f, 0.75f);
			}

			return true;
		}

		public override void AI()
		{
			OldPosition.Add(Projectile.Center);
			OldRotation.Add(Projectile.rotation);

			Projectile.rotation = Projectile.velocity.ToRotation();

			if (OldPosition.Count > 15)
			{
				OldPosition.RemoveAt(0);
			}

			if (Main.rand.NextBool(4))
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Smoke);
				dust.velocity *= 0.1f;
				dust.scale = Main.rand.NextFloat(0.6f, 0.8f);
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, ref Color lightColor)
		{
			float colorMult = 1f;
			if (Projectile.timeLeft < 7) colorMult *= Projectile.timeLeft / 7f;

			for (int i = 0; i < OldPosition.Count; i++)
			{
				Vector2 drawPosition2 = OldPosition[i] - Main.screenPosition;
				spriteBatch.Draw(TextureMain, drawPosition2, null, lightColor * 0.075f * (i + 1) * colorMult, OldRotation[i], TextureMain.Size() * 0.5f, Projectile.scale * (i + 1) * 0.065f, SpriteEffects.None, 0f);
			}

			return true;
		}
	}
}