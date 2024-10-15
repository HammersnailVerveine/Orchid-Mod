using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Content.Guardian;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Player;

namespace OrchidMod.Content.Shapeshifter
{
	public class ShapeshifterShapeshiftAnchor : OrchidModShapeshifterProjectile
	{
		public bool NeedNetUpdate = false;
		public int SelectedItem { get; set; } = -1;
		public Item ShapeshifterItem => Main.player[Projectile.owner].inventory[SelectedItem];

		public List<Vector2> OldPosition;
		public List<float> OldRotation;
		public List<int> OldFrame;
		public int Frame = 0;
		public int Timespent = 0;
		public Texture2D TextureShapeshift;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(SelectedItem);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			SelectedItem = reader.ReadInt32();
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			overPlayers.Add(index);
		}

		public override void AltSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 60;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			Projectile.netImportant = true;
			OldPosition = new List<Vector2>();
			OldRotation = new List<float>();
			OldFrame = new List<int>();
		}

		public void OnChangeSelectedItem(Player owner)
		{
			OrchidShapeshifter shapeshifter = owner.GetModPlayer<OrchidShapeshifter>();
			if (owner.inventory[owner.selectedItem].ModItem is OrchidModShapeshifterShapeshift shapeshiftItem)
			{
				SelectedItem = owner.selectedItem;
				shapeshifter.Shapeshift = shapeshiftItem;
				shapeshifter.ShapeshiftAnchor = this;
				Projectile.width = shapeshiftItem.ShapeshiftWidth;
				Projectile.height = shapeshiftItem.ShapeshiftHeight;
				shapeshiftItem.ShapeshiftAnchorOnShapeshift(Projectile, this);
				TextureShapeshift = ModContent.Request<Texture2D>(shapeshiftItem.ShapeshiftTexture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				SoundEngine.PlaySound(shapeshiftItem.Item.UseSound, owner.Center);
			}
			Projectile.netUpdate = true;
		}

		public override void AI()
		{
			var owner = Owner;
			OrchidShapeshifter shapeshifter = owner.GetModPlayer<OrchidShapeshifter>();

			if ((SelectedItem < 0 || !owner.active || owner.dead || owner.HeldItem.ModItem is not OrchidModShapeshifterShapeshift) && IsLocalOwner)
			{
				Projectile.Kill();
				return;
			}
			else
			{
				Projectile.timeLeft = 5;

				if (ShapeshifterItem.ModItem is OrchidModShapeshifterShapeshift shapeshifterItem)
				{
				}
			}
		}

		public override void OnKill(int timeLeft)
		{
			var owner = Owner;
			OrchidShapeshifter shapeshifter = owner.GetModPlayer<OrchidShapeshifter>();
			shapeshifter.Shapeshift = null;
			shapeshifter.ShapeshiftAnchor = null;

			for (int i = 0; i < 3; i++)
			{
				Main.dust[Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke)].velocity *= 0.25f;
			}
		}

		public override bool? CanCutTiles() => false;

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (ShapeshifterItem.ModItem is not OrchidModShapeshifterShapeshift shapeshifterItem || TextureShapeshift == null) return false;
			var player = Main.player[Projectile.owner];

			if (shapeshifterItem.PreDrawShapeshift(spriteBatch, Projectile, player, ref lightColor))
			{
				Vector2 drawPosition = Vector2.Transform(Projectile.Center - Main.screenPosition + Vector2.UnitY * player.gfxOffY, Main.GameViewMatrix.EffectMatrix);
				Rectangle drawRectangle = TextureShapeshift.Bounds;
				drawRectangle.Height = drawRectangle.Width;
				var effect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

				spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
				spriteBatch.Begin(spriteBatchSnapshot with { BlendState = BlendState.Additive });

				for (int i = 0; i < OldPosition.Count; i++)
				{
					drawRectangle.Y = drawRectangle.Height * OldFrame[i];
					Vector2 drawPosition2 = Vector2.Transform(OldPosition[i] - Main.screenPosition + Vector2.UnitY * player.gfxOffY, Main.GameViewMatrix.EffectMatrix);
					spriteBatch.Draw(TextureShapeshift, drawPosition2, drawRectangle, lightColor * 0.075f * (i + 1), OldRotation[i], drawRectangle.Size() * 0.5f, Projectile.scale, effect, 0f);
				}


				spriteBatch.End();
				spriteBatch.Begin(spriteBatchSnapshot);
				//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
				//GameShaders.Misc["OrchidMod:HorizonGlow"].Apply();

				drawRectangle.Y = drawRectangle.Height * Frame;
				spriteBatch.Draw(TextureShapeshift, drawPosition, drawRectangle, lightColor, Projectile.rotation, drawRectangle.Size() * 0.5f, Projectile.scale, effect, 0f);

				//spriteBatch.End();
				//spriteBatch.Begin(spriteBatchSnapshot);
				return false;

			}
			shapeshifterItem.PostDrawShapeshift(spriteBatch, Projectile, player, lightColor);

			return false;
		}
	}
}