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
		public static Texture2D textureHammerReady;

		public static Texture2D textureGauntletOn;
		public static Texture2D textureGauntletOff;
		public static Texture2D textureGauntletReady;

		public static Texture2D textureStandardOn;
		public static Texture2D textureStandardOff;
		public static Texture2D textureStandardReady;

		public static Texture2D blockOn;
		public static Texture2D blockOff;

		public static Texture2D textureIconStandard;
		public static Texture2D textureIconRune;

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

			textureGauntletOn ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/GauntletOn", AssetRequestMode.ImmediateLoad).Value;
			textureGauntletOff ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/GauntletOff", AssetRequestMode.ImmediateLoad).Value;
			textureGauntletReady ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/GauntletReady", AssetRequestMode.ImmediateLoad).Value;

			textureStandardOn ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/StandardOn", AssetRequestMode.ImmediateLoad).Value;
			textureStandardOff ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/StandardOff", AssetRequestMode.ImmediateLoad).Value;
			textureStandardReady ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/StandardReady", AssetRequestMode.ImmediateLoad).Value;
			
			textureHammerOn ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/HammerOn", AssetRequestMode.ImmediateLoad).Value;
			textureHammerOff ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/HammerOff", AssetRequestMode.ImmediateLoad).Value;
			textureHammerReady ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/HammerReady", AssetRequestMode.ImmediateLoad).Value;

			blockOn ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/BlockOn", AssetRequestMode.ImmediateLoad).Value;
			blockOff ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/BlockOff", AssetRequestMode.ImmediateLoad).Value;

			textureIconStandard ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/IconStandard", AssetRequestMode.ImmediateLoad).Value;
			textureIconRune ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/IconRune", AssetRequestMode.ImmediateLoad).Value;

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

				int offSet = (int)(modPlayer.GuardianGuardMax / 2f * (textureBlockOn.Width + 2));
				int offSetIcons = offSet;
				for (int i = 0; i < modPlayer.GuardianGuardMax; i++)
				{
					Texture2D texture = modPlayer.GuardianGuard > i ? textureBlockOn : textureBlockOff;
					spriteBatch.Draw(texture, new Vector2(position.X - offSet + (textureBlockOn.Width + 2) * i, position.Y), Color.White);
				}

				offSet = (int)(modPlayer.GuardianSlamMax / 2f * (textureSlamOn.Width + 2));
				if (offSet > offSetIcons) offSetIcons = offSet; // Calculates the offset for banner and rune icon based on the largest offset between block (guard) and slam stacks
				for (int i = 0; i < modPlayer.GuardianSlamMax; i++)
				{
					bool check = modPlayer.GuardianSlam > i;
					Texture2D texture = check ? textureSlamOn : textureSlamOff;
					spriteBatch.Draw(texture, new Vector2(position.X - offSet + 18 * i, position.Y + 18), Color.White);
					if (modPlayer.SlamCostUI > i)
						spriteBatch.Draw(textureSlamHighlight, new Vector2(position.X - offSet - 2 + 18 * i, position.Y + 16), (check ? Color.White * 0.8f : Color.DarkGray) * 0.8f);
				}

				offSet = (int)((textureIconRune.Width + 2) * 0.5f);
				if (modPlayer.RuneProjectiles.Count > 0)
				{
					float colorMult = modPlayer.RuneProjectiles[0].timeLeft > 275 ? 1f : (float)Math.Abs(Math.Sin((modPlayer.RuneProjectiles[0].timeLeft * 0.5f) / Math.PI / 4f));
					offSet = (int)(offSet * (modPlayer.StandardAnchor == null ? 1f : 2f));
					spriteBatch.Draw(textureIconRune, new Vector2(position.X - offSet, position.Y + 36), Color.White * colorMult);
					offSet -= textureIconRune.Width + 2;
				}

				if (modPlayer.StandardAnchor != null)
				{
					float colorMult = modPlayer.StandardAnchor.ai[1] > 275 ? 1f : (float)Math.Abs(Math.Sin((modPlayer.StandardAnchor.ai[1] * 0.5f) / Math.PI / 4f));
					spriteBatch.Draw(textureIconStandard, new Vector2(position.X - offSet, position.Y + 36), Color.White * colorMult);
				}

				if (player.HeldItem.useTime > 0)
				{
					if (ModContent.GetInstance<OrchidClientConfig>().UseOldGuardianHammerUi)
					{
						if (modPlayer.GuardianHammerCharge > (70 * player.GetAttackSpeed(DamageClass.Melee) - player.HeldItem.useTime) / 2.5f)
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
						if (modPlayer.GuardianHammerCharge > (70 * player.GetAttackSpeed(DamageClass.Melee) - player.HeldItem.useTime) / 2.5f)
						{
							int val = 24;
							if (modPlayer.GuardianHammerCharge > 180f)
							{
								spriteBatch.Draw(textureHammerReady, new Vector2(position.X - 14, position.Y - 96), Color.White * 0.8f);
							}
							else
							{
								float charge = modPlayer.GuardianHammerCharge;
								while (charge < 180f)
								{
									charge += 7.5f;
									val--;
								}
							}

							Rectangle rectangle = textureHammerOn.Bounds;
							rectangle.Height = val;
							rectangle.Y = textureHammerOn.Height - val;
							spriteBatch.Draw(textureHammerOff, new Vector2(position.X - 12, position.Y - 94), Color.White);
							spriteBatch.Draw(textureHammerOn, new Vector2(position.X - 12, position.Y - 94 + textureHammerOn.Height - val), rectangle, Color.White);
						}
					}

					if (modPlayer.GuardianStandardCharge > (70 * player.GetAttackSpeed(DamageClass.Melee) - player.HeldItem.useTime) / 2.5f)
					{
						int val = textureStandardOn.Height;
						if (modPlayer.GuardianStandardCharge >= 180f)
						{
							spriteBatch.Draw(textureStandardReady, new Vector2(position.X - 11, position.Y - 96), Color.White * 0.8f);
						}
						else
						{
							float charge = modPlayer.GuardianStandardCharge;
							while (charge < 180f)
							{
								charge += 7.5f;
								val--;
							}
						}

						Rectangle rectangle = textureStandardOn.Bounds;
						rectangle.Height = val;
						rectangle.Y = textureStandardOn.Height - val;
						spriteBatch.Draw(textureStandardOff, new Vector2(position.X - 9, position.Y - 94), Color.White);
						spriteBatch.Draw(textureStandardOn, new Vector2(position.X - 9, position.Y - 94 + textureStandardOn.Height - val), rectangle, Color.White);
					}

					if (modPlayer.GuardianGauntletCharge > (70 * player.GetAttackSpeed(DamageClass.Melee) - player.HeldItem.useTime) / 2.5f)
					{
						int val = textureGauntletOn.Height;
						if (modPlayer.GuardianGauntletCharge >= 180f)
						{
							spriteBatch.Draw(textureGauntletReady, new Vector2(position.X - 10, position.Y - 96), Color.White * 0.8f);
						}
						else
						{
							float charge = modPlayer.GuardianGauntletCharge;
							while (charge < 180f)
							{
								charge += 7.5f;
								val--;
							}
						}

						Rectangle rectangle = textureGauntletOn.Bounds;
						rectangle.Height = val;
						rectangle.Y = textureGauntletOn.Height - val;
						spriteBatch.Draw(textureGauntletOff, new Vector2(position.X - 8, position.Y - 94), Color.White);
						spriteBatch.Draw(textureGauntletOn, new Vector2(position.X - 8, position.Y - 94 + textureGauntletOn.Height - val), rectangle, Color.White);
					}
					else
					{
						int projectileType = ModContent.ProjectileType<GuardianShieldAnchor>();
						int projectileType2 = ModContent.ProjectileType<GuardianGauntletAnchor>();
						for (int i = 0; i < Main.projectile.Length; i++)
						{
							Projectile proj = Main.projectile[i];
							if (proj.active && proj.owner == player.whoAmI && (proj.type == projectileType || proj.type == projectileType2) && proj.ai[0] > 0f && proj.localAI[0] > 0f)
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
								spriteBatch.Draw(blockOff, new Vector2(position.X - 10, position.Y - 92), Color.White);
								spriteBatch.Draw(blockOn, new Vector2(position.X - 10, position.Y - 92 + blockOn.Height - val), rectangle, Color.White);
								return;
							}
						}
					}
				}

				spriteBatch.End();
				spriteBatch.Begin(spriteBatchSnapshot);
			}
		}
	}
}