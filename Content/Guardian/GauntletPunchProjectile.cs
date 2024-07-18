using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using OrchidMod.Utilities;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian
{
	public class GauntletPunchProjectile : OrchidModProjectile
	{
		private static Texture2D TextureMain;
		public int SelectedItem { get; set; } = -1;
		public Item GauntletItem => Main.player[Projectile.owner].inventory[this.SelectedItem];
		public bool ChargedHit => Projectile.ai[0] == 1f;
		public bool FirstHit = false;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(SelectedItem);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			SelectedItem = reader.ReadInt32();
		}

		public override void AltSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 20;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
		}

		public override void AI()
		{
			var owner = Main.player[Projectile.owner];
			if (!owner.active || owner.dead || SelectedItem < 0 || GauntletItem.ModItem is not OrchidModGuardianGauntlet gauntlet)
			{
				Projectile.Kill();
			}
			else
			{
				if (gauntlet.ProjectileAI(owner, Projectile, ChargedHit))
				{
					Projectile.velocity *= 0.8f;
				}
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			var owner = Main.player[Projectile.owner];
			if (!owner.active || owner.dead || SelectedItem < 0 || GauntletItem.ModItem is not OrchidModGuardianGauntlet gauntlet)
			{
				return;
			}
			else
			{
				OrchidGuardian guardian = owner.GetModPlayer<OrchidGuardian>();
				if (!FirstHit)
				{
					FirstHit = true;
					gauntlet.OnHitFirst(owner, guardian, target, Projectile, hit, ChargedHit);
				}
				gauntlet.OnHit(owner, guardian, target, Projectile, hit, ChargedHit);
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			var owner = Main.player[Projectile.owner];
			if (!owner.active || owner.dead || SelectedItem < 0 || GauntletItem.ModItem is not OrchidModGuardianGauntlet gauntlet)
			{
				Projectile.Kill();
			}
			else
			{
				spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
				spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

				// Draw code here
				float colorMult = 0.8f;
				Vector2 offsetVector = new Vector2(0f, 12f).RotatedBy(Projectile.rotation - MathHelper.PiOver2);
				if (Projectile.timeLeft < 10) colorMult *= Projectile.timeLeft / 10f;
				SpriteEffects effect = SpriteEffects.None;
				if (Projectile.velocity.X < 0f) effect = SpriteEffects.FlipVertically;

				float scale = Projectile.scale * (ChargedHit ? 1.2f : 1f);
				Vector2 drawPosition = Vector2.Transform(Projectile.Center - offsetVector - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
				spriteBatch.Draw(TextureMain, drawPosition, null, gauntlet.color * colorMult, Projectile.rotation, TextureMain.Size() * 0.5f, scale, effect, 0f);

				// Draw code ends here

				spriteBatch.End();
				spriteBatch.Begin(spriteBatchSnapshot);
			}
			return false;
		}
	}
}	