using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Common.UIs;
using OrchidMod.Utilities;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace OrchidMod.Content.Guardian.UI
{
	public class GuardianUIState : OrchidUIState
	{
		public static Texture2D textureBlockOn;
		public static Texture2D textureBlockOff;
		public static Texture2D textureSlamOn;
		public static Texture2D textureSlamOff;
		public static Texture2D textureSlamHighlight;

		public static Texture2D textureHammerMain;
		public static Texture2D textureHammerIcon;
		public static Texture2D textureHammerIconBig;

		public static Texture2D textureHammerOn;
		public static Texture2D textureHammerOff;

		public static Texture2D blockOn;
		public static Texture2D blockOff;

		public override int InsertionIndex(List<GameInterfaceLayer> layers)
			=> layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
		public override void OnInitialize()
		{
			textureBlockOn ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/BlockBarOn", AssetRequestMode.ImmediateLoad).Value;
			textureBlockOff ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/BlockBarOff", AssetRequestMode.ImmediateLoad).Value;
			textureSlamOn ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/SlamBarOn", AssetRequestMode.ImmediateLoad).Value;
			textureSlamOff ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/SlamBarOff", AssetRequestMode.ImmediateLoad).Value;
			textureSlamHighlight ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/SlamBarHighlight", AssetRequestMode.ImmediateLoad).Value;
			
			textureHammerMain ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/HammerBar", AssetRequestMode.ImmediateLoad).Value;
			textureHammerIcon ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/HammerIcon1", AssetRequestMode.ImmediateLoad).Value;
			textureHammerIconBig ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/HammerIcon2", AssetRequestMode.ImmediateLoad).Value;
			
			textureHammerOn ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/HammerOn", AssetRequestMode.ImmediateLoad).Value;
			textureHammerOff ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/HammerOff", AssetRequestMode.ImmediateLoad).Value;
			blockOn ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/BlockOn", AssetRequestMode.ImmediateLoad).Value;
			blockOff ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/BlockOff", AssetRequestMode.ImmediateLoad).Value;

			Width.Set(0f, 0f);
			Height.Set(0f, 0f);
			Left.Set(Main.screenWidth / 2, 0f);
			Top.Set(Main.screenHeight / 2, 0f);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Recalculate();
			Player player = Main.LocalPlayer;
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();

			if (!player.dead && modPlayer.GuardianDisplayUI > 0)
			{
				spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

				Vector2 position = (player.position + new Vector2(player.width * 0.5f, player.gravDir > 0 ? player.height + player.gfxOffY + 12 : 10 + player.gfxOffY)).Floor();
				position = Vector2.Transform(position - Main.screenPosition, Main.GameViewMatrix.EffectMatrix);

				int offSet = (int)(modPlayer.GuardianBlockMax / 2f * (textureBlockOn.Width + 2));
				for (int i = 0; i < modPlayer.GuardianBlockMax; i++)
				{
					Texture2D texture = modPlayer.GuardianBlock > i ? textureBlockOn : textureBlockOff;
					spriteBatch.Draw(texture, new Vector2(position.X - offSet + (textureBlockOn.Width + 2) * i, position.Y), Color.White);
				}

				offSet = (int)(modPlayer.GuardianSlamMax / 2f * (textureSlamOn.Width + 2));
				for (int i = 0; i < modPlayer.GuardianSlamMax; i++)
				{
					bool check = modPlayer.GuardianSlam > i;
					Texture2D texture = check ? textureSlamOn : textureSlamOff;
					spriteBatch.Draw(texture, new Vector2(position.X - offSet + 18 * i, position.Y + 18), Color.White);
					if (modPlayer.SlamCostUI > i)
						spriteBatch.Draw(textureSlamHighlight, new Vector2(position.X - offSet - 2 + 18 * i, position.Y + 16), check ? Color.White : Color.DarkGray);
				}

				if (ModContent.GetInstance<OrchidClientConfig>().UseOldGuardianHammerUi)
				{
					if (modPlayer.GuardianThrowCharge > 0) // Player is preparing to throw a hammer
					{
						Vector2 hammerPosition = new Vector2(position.X - textureHammerMain.Width / 2, position.Y - 100);
						spriteBatch.Draw(textureHammerMain, hammerPosition, Color.White);

						int throwCharge = modPlayer.ThrowLevel();
						if (throwCharge > 0)
						{
							Vector2 iconPosition = hammerPosition + new Vector2(4, 2);
							Color color = new Color(87, 220, 0);
							spriteBatch.Draw(textureHammerIcon, iconPosition, color);
						}

						if (throwCharge > 1)
						{
							Vector2 iconPosition = hammerPosition + new Vector2(16, 2);
							Color color = new Color(255, 223, 0);
							spriteBatch.Draw(textureHammerIcon, iconPosition, color);
						}

						if (throwCharge > 2)
						{
							Vector2 iconPosition = hammerPosition + new Vector2(28, 2);
							Color color = new Color(255, 150, 0);
							spriteBatch.Draw(textureHammerIcon, iconPosition, color);
						}

						if (throwCharge > 3)
						{
							Vector2 iconPosition = hammerPosition + new Vector2(40, 0);
							Color color = new Color(255, 27, 0);
							spriteBatch.Draw(textureHammerIconBig, iconPosition, color);
						}
					}
				}
				else
				{
					if (modPlayer.GuardianThrowCharge > 0f)
					{
						int val = 24;
						float block = modPlayer.GuardianThrowCharge;
						while (block < 180f)
						{
							block += 7.5f;
							val--;
						}

						Rectangle rectangle = textureHammerOn.Bounds;
						rectangle.Height = val;
						rectangle.Y = textureHammerOn.Height - val;
						spriteBatch.Draw(textureHammerOff, new Vector2(position.X - 12, position.Y - 92), Color.White);
						spriteBatch.Draw(textureHammerOn, new Vector2(position.X - 12, position.Y - 92 + textureHammerOn.Height - val), rectangle, Color.White);
					}
				}


				int projectileType = ModContent.ProjectileType<GuardianShieldAnchor>();
				for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile proj = Main.projectile[i];
					if (proj.active && proj.owner == player.whoAmI && proj.type == projectileType && proj.ai[0] > 0f && proj.localAI[0] > 0f)
					{
						int val = 22;
						float block = proj.ai[0];
						while (block < proj.localAI[0])
						{
							block += proj.localAI[0] / 20f;
							val--;
						}

						Rectangle rectangle = blockOn.Bounds;
						rectangle.Height = val;
						rectangle.Y = blockOn.Height - val;
						spriteBatch.Draw(blockOff, new Vector2(position.X - 10, position.Y - 90), Color.White);
						spriteBatch.Draw(blockOn, new Vector2(position.X - 10, position.Y - 90 + blockOn.Height - val), rectangle, Color.White);
						return;
					}
				}

				spriteBatch.End();
				spriteBatch.Begin(spriteBatchSnapshot);
			}
		}
	}
}