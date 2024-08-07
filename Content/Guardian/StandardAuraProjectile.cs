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
	public class StandardAuraProjectile : OrchidModGuardianProjectile
	{
		private static Texture2D TextureMain;
		public int SelectedItem { get; set; } = -1;
		public Item StandardItem => Main.player[Projectile.owner].inventory[this.SelectedItem];

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
			Projectile.timeLeft = 30;
			Projectile.tileCollide = false;
			Projectile.scale = 0.1f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
			Projectile.alpha = 240;
			TextureMain ??= ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
		}

		public override void AI()
		{
			Projectile.scale *= 1.1f;
			Projectile.alpha -= 8;
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (StandardItem.ModItem is not OrchidModGuardianStandard standard)
			{
				Projectile.Kill();
			}
			else
			{
				spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
				spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

				// Draw code here

				Player owner = Main.player[Projectile.owner];
				Vector2 drawPosition = Vector2.Transform(owner.Center.Floor() + new Vector2(0f, owner.gfxOffY) - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);
				spriteBatch.Draw(TextureMain, drawPosition, null, standard.GetColor() * (Projectile.alpha / 255f), Projectile.rotation, TextureMain.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);

				// Draw code ends here

				spriteBatch.End();
				spriteBatch.Begin(spriteBatchSnapshot);
			}
			return false;
		}
	}
}	