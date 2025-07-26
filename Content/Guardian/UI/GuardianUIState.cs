using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Common.UIs;
using OrchidMod.Content.Guardian.Projectiles.Misc;
using OrchidMod.Content.Guardian.Weapons.Misc;
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

		public static Texture2D textureRuneOn;
		public static Texture2D textureRuneOff;
		public static Texture2D textureRuneReady;

		public static Texture2D textureQuarterstaffOn;
		public static Texture2D textureQuarterstaffOff;
		public static Texture2D textureQuarterstaffReady;

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

			textureRuneOn ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/RuneOn", AssetRequestMode.ImmediateLoad).Value;
			textureRuneOff ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/RuneOff", AssetRequestMode.ImmediateLoad).Value;
			textureRuneReady ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/RuneReady", AssetRequestMode.ImmediateLoad).Value;

			textureQuarterstaffOn ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/QuarterstaffOn", AssetRequestMode.ImmediateLoad).Value;
			textureQuarterstaffOff ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/QuarterstaffOff", AssetRequestMode.ImmediateLoad).Value;
			textureQuarterstaffReady ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/QuarterstaffReady", AssetRequestMode.ImmediateLoad).Value;

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

				Vector2 position = (player.position + new Vector2(player.width * 0.5f, player.height + player.gfxOffY + 12)).Floor();
				if (player.gravDir < 0) position.Y -= 81;
				Vector2 drawpos = position;
				position = position - Main.screenPosition;
				SpriteEffects effect = player.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;

				int offSet = (int)(modPlayer.GuardianGuardMax / 2f * (textureBlockOn.Width + 2));
				int offSetIcons = offSet;
				for (int i = 0; i < modPlayer.GuardianGuardMax; i++)
				{
					Texture2D texture = modPlayer.GuardianGuard > i ? textureBlockOn : textureBlockOff;
					drawpos = new Vector2(position.X - offSet + (textureBlockOn.Width + 2) * i, position.Y);
					spriteBatch.Draw(texture, drawpos, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
					if (modPlayer.GuardianGuard - 1 == i && modPlayer.GuardianGuardRecharging < 0)
					{
						int chargingWidth = (int)(-modPlayer.GuardianGuardRecharging * textureBlockOn.Width + 0.5f);
						float flash = 0.6f + (float)Math.Sin(player.miscCounterNormalized * 2 * MathHelper.TwoPi) * 0.2f;
						spriteBatch.Draw(textureBlockOff, drawpos + Vector2.UnitX * (textureBlockOn.Width - chargingWidth), new Rectangle(textureBlockOn.Width - chargingWidth, 0, chargingWidth, textureBlockOn.Height), new Color(flash, flash, flash, 0.6f));
					}
					else if (modPlayer.GuardianGuard == i && modPlayer.GuardianGuardRecharging > 0)
					{
						float flash = 0.5f + (float)Math.Sin((player.miscCounterNormalized + modPlayer.GuardianGuardRecharging * 1.5f) * 2 * MathHelper.TwoPi) * 0.1f;
						spriteBatch.Draw(textureBlockOn, drawpos, new Rectangle(0, 0, (int)(modPlayer.GuardianGuardRecharging * textureBlockOn.Width + 0.5f), textureBlockOn.Height), new Color(flash, flash, flash, 0.5f));
					}
				}

				offSet = (int)(modPlayer.GuardianSlamMax / 2f * (textureSlamOn.Width + 2));
				if (offSet > offSetIcons) offSetIcons = offSet; // Calculates the offset for banner and rune icon based on the largest offset between block (guard) and slam stacks
				for (int i = 0; i < modPlayer.GuardianSlamMax; i++)
				{
					bool check = modPlayer.GuardianSlam > i;
					Texture2D texture = check ? textureSlamOn : textureSlamOff;
					drawpos = new Vector2(position.X - offSet + 18 * i, position.Y + 18 * player.gravDir);
					spriteBatch.Draw(texture, drawpos, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
					if (modPlayer.SlamCostUI > i)
					{
						Vector2 drawposCost = new Vector2(position.X - offSet - 2 + 18 * i, position.Y + 16 * player.gravDir + 2f * (player.gravDir - 1));
						spriteBatch.Draw(textureSlamHighlight, drawposCost, null, (check ? Color.White * 0.8f : Color.DarkGray) * 0.8f, 0f, Vector2.Zero, 1f, effect, 0f);
					}
					if (modPlayer.GuardianSlam - 1 == i && modPlayer.GuardianSlamRecharging < 0)
					{
						int chargingWidth = (int)(-modPlayer.GuardianSlamRecharging * textureSlamOn.Width + 0.5f);
						float flash = 0.6f + (float)Math.Sin(player.miscCounterNormalized * 2 * MathHelper.TwoPi) * 0.2f;
						spriteBatch.Draw(textureSlamOff, drawpos + Vector2.UnitX * (textureSlamOn.Width - chargingWidth), new Rectangle(textureSlamOn.Width - chargingWidth, 0, chargingWidth, textureSlamOn.Height), new Color(flash, flash, flash, 0.6f));
					}
					else if (modPlayer.GuardianSlam == i && modPlayer.GuardianSlamRecharging > 0)
					{
						float flash = 0.5f + (float)Math.Sin((player.miscCounterNormalized + modPlayer.GuardianSlamRecharging * 1.5f) * 2 * MathHelper.TwoPi) * 0.1f;
						spriteBatch.Draw(textureSlamOn, drawpos, new Rectangle(0, 0, (int)(modPlayer.GuardianSlamRecharging * textureSlamOn.Width + 0.5f), textureSlamOn.Height), new Color(flash, flash, flash, 0.5f));
					}
				}

				offSet = (int)((textureIconRune.Width + 2) * 0.5f);
				if (modPlayer.RuneProjectiles.Count > 0)
				{
					float colorMult = modPlayer.RuneProjectiles[0].timeLeft > 275 ? 1f : (float)Math.Abs(Math.Sin((modPlayer.RuneProjectiles[0].timeLeft * 0.5f) / Math.PI / 4f));
					offSet = (int)(offSet * (modPlayer.GuardianCurrentStandardAnchor == null ? 1f : 2f));
					drawpos = new Vector2(position.X - offSet, position.Y + 36 * player.gravDir + (player.gravDir - 1));
					spriteBatch.Draw(textureIconRune, drawpos, null, Color.White * colorMult, 0f, Vector2.Zero, 1f, effect, 0f);
					offSet -= textureIconRune.Width + 2;
				}

				if (modPlayer.GuardianCurrentStandardAnchor != null)
				{
					float colorMult = modPlayer.GuardianCurrentStandardAnchor.ai[1] > 275 ? 1f : (float)Math.Abs(Math.Sin((modPlayer.GuardianCurrentStandardAnchor.ai[1] * 0.5f) / Math.PI / 4f));
					drawpos = new Vector2(position.X - offSet, position.Y + 36 * player.gravDir + (player.gravDir - 1));
					spriteBatch.Draw(textureIconStandard, drawpos, null, Color.White * colorMult, 0f, Vector2.Zero, 1f, effect, 0f);
				}

				if (player.HeldItem.ModItem is OrchidModGuardianItem)
				{
					if (modPlayer.GuardianHammerCharge > (70 * player.GetTotalAttackSpeed(DamageClass.Melee) - player.HeldItem.useTime) / 2.5f && player.HeldItem.ModItem is OrchidModGuardianHammer)
					{
						if (ModContent.GetInstance<OrchidClientConfig>().GuardianUseOldHammerUi)
						{
							Vector2 hammerPosition = new Vector2(position.X - textureHammerMain.Width / 2, position.Y - 100 * player.gravDir);
							spriteBatch.Draw(textureHammerMain, hammerPosition, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);

							int throwCharge = modPlayer.ThrowLevel();
							if (throwCharge > 0)
							{
								drawpos = hammerPosition + new Vector2(4, 2 * player.gravDir);
								Color color = new Color(87, 220, 0);
								spriteBatch.Draw(textureHammerIcon, drawpos, null, color, 0f, Vector2.Zero, 1f, effect, 0f);
							}

							if (throwCharge > 1)
							{
								drawpos = hammerPosition + new Vector2(16, 2 * player.gravDir);
								Color color = new Color(255, 223, 0);
								spriteBatch.Draw(textureHammerIcon, drawpos, null, color, 0f, Vector2.Zero, 1f, effect, 0f);
							}

							if (throwCharge > 2)
							{
								drawpos = hammerPosition + new Vector2(28, 2 * player.gravDir);
								Color color = new Color(255, 150, 0);
								spriteBatch.Draw(textureHammerIcon, drawpos, null, color, 0f, Vector2.Zero, 1f, effect, 0f);
							}

							if (throwCharge > 3)
							{
								drawpos = hammerPosition + new Vector2(40, 0);
								Color color = new Color(255, 27, 0);
								spriteBatch.Draw(textureHammerIconBig, drawpos, null, color, 0f, Vector2.Zero, 1f, effect, 0f);
							}
						}
						else
						{
							int val = 24;
							if (modPlayer.GuardianHammerCharge > 180f)
							{
								drawpos = new Vector2(position.X - 14, position.Y - 96 * player.gravDir + 6f * (player.gravDir - 1));
								spriteBatch.Draw(textureHammerReady, drawpos, null, Color.White * 0.8f, 0f, Vector2.Zero, 1f, effect, 0f);
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
							drawpos = new Vector2(position.X - 12, position.Y - 94 * player.gravDir + 4f * (player.gravDir - 1));
							spriteBatch.Draw(textureHammerOff, drawpos, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
							drawpos = new Vector2(position.X - 12, position.Y - 94 * player.gravDir + textureHammerOn.Height - val + 4f * (player.gravDir - 1));
							if (player.gravDir < 0) drawpos.Y -= (textureHammerOn.Height - rectangle.Height);
							spriteBatch.Draw(textureHammerOn, drawpos, rectangle, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
						}
					}

					if (modPlayer.GuardianStandardCharge > (70 * player.GetTotalAttackSpeed(DamageClass.Melee) - player.HeldItem.useTime) / 2.5f && (player.HeldItem.ModItem is OrchidModGuardianStandard || player.HeldItem.ModItem is HorizonLance))
					{
						int val = textureStandardOn.Height;
						if (modPlayer.GuardianStandardCharge >= 180f)
						{
							drawpos = new Vector2(position.X - 11, position.Y - 96 * player.gravDir + 5f * (player.gravDir - 1));
							spriteBatch.Draw(textureStandardReady, drawpos, null, Color.White * 0.8f, 0f, Vector2.Zero, 1f, effect, 0f);
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

						drawpos = new Vector2(position.X - 9, position.Y - 94 * player.gravDir + 3f * (player.gravDir - 1));
						spriteBatch.Draw(textureStandardOff, drawpos, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
						drawpos = new Vector2(position.X - 9, position.Y - 94 * player.gravDir + textureStandardOn.Height - val + 3f * (player.gravDir - 1));
						if (player.gravDir < 0) drawpos.Y -= (textureStandardOn.Height - rectangle.Height);
						spriteBatch.Draw(textureStandardOn, drawpos, rectangle, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
					}

					if (modPlayer.GuardianRuneCharge > (23 * player.GetTotalAttackSpeed(DamageClass.Melee) - player.HeldItem.useTime) / 2.5f && player.HeldItem.ModItem is OrchidModGuardianRune)
					{
						int val = textureRuneOn.Height;
						if (modPlayer.GuardianRuneCharge >= 180f)
						{
							drawpos = new Vector2(position.X - 11, position.Y - 96 * player.gravDir + 5f * (player.gravDir - 1));
							spriteBatch.Draw(textureRuneReady, drawpos, null, Color.White * 0.8f, 0f, Vector2.Zero, 1f, effect, 0f);
						}
						else
						{
							float charge = modPlayer.GuardianRuneCharge;
							while (charge < 180f)
							{
								charge += 7.5f;
								val--;
							}
						}

						Rectangle rectangle = textureRuneOn.Bounds;
						rectangle.Height = val;
						rectangle.Y = textureRuneOn.Height - val;
						drawpos = new Vector2(position.X - 9, position.Y - 94 * player.gravDir + 3f * (player.gravDir - 1));
						spriteBatch.Draw(textureRuneOff, drawpos, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
						drawpos = new Vector2(position.X - 9, position.Y - 94 * player.gravDir + textureRuneOn.Height - val + 3f * (player.gravDir - 1));
						if (player.gravDir < 0) drawpos.Y -= (textureRuneOn.Height - rectangle.Height);
						spriteBatch.Draw(textureRuneOn, drawpos, rectangle, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
					}

					if (player.HeldItem.ModItem is OrchidModGuardianQuarterstaff)
					{
						if (modPlayer.GuardianGauntletCharge > (70 * player.GetTotalAttackSpeed(DamageClass.Melee) - player.HeldItem.useTime) / 2.5f)
						{
							int val = textureQuarterstaffOn.Height;
							if (modPlayer.GuardianGauntletCharge >= 180f)
							{
								drawpos = new Vector2(position.X - 10, position.Y - 96 * player.gravDir + 6f * (player.gravDir - 1));
								spriteBatch.Draw(textureQuarterstaffReady, drawpos, null, Color.White * 0.8f, 0f, Vector2.Zero, 1f, effect, 0f);
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

							Rectangle rectangle = textureQuarterstaffOn.Bounds;
							rectangle.Height = val;
							rectangle.Y = textureQuarterstaffOn.Height - val;
							drawpos = new Vector2(position.X - 8, position.Y - 94 * player.gravDir + 4f * (player.gravDir - 1));
							spriteBatch.Draw(textureQuarterstaffOff, drawpos, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
							drawpos = new Vector2(position.X - 8, position.Y - 94 * player.gravDir + textureQuarterstaffOn.Height - val + 4f * (player.gravDir - 1));
							if (player.gravDir < 0) drawpos.Y -= (textureQuarterstaffOn.Height - rectangle.Height);
							spriteBatch.Draw(textureQuarterstaffOn, drawpos, rectangle, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
						}
						else
						{
							int projectileType = ModContent.ProjectileType<GuardianQuarterstaffAnchor>();
							for (int i = 0; i < Main.projectile.Length; i++)
							{
								Projectile proj = Main.projectile[i];
								if (proj.active && proj.owner == player.whoAmI && proj.type == projectileType && proj.ai[2] > 0f)
								{
									int val = 22;
									float block = proj.ai[2];
									float step = (player.HeldItem.ModItem as OrchidModGuardianQuarterstaff).ParryDuration;
									while (block < step)
									{
										block += step / 20f;
										val--;
									}

									Rectangle rectangle = blockOn.Bounds;
									rectangle.Height = val;
									rectangle.Y = blockOn.Height - val;
									drawpos = new Vector2(position.X - 10, position.Y - 92 * player.gravDir + 3f * (player.gravDir - 1));
									spriteBatch.Draw(blockOff, drawpos, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
									drawpos = new Vector2(position.X - 10, position.Y - 92 * player.gravDir + blockOn.Height - val + 3f * (player.gravDir - 1));
									if (player.gravDir < 0) drawpos.Y -= (blockOn.Height - rectangle.Height);
									spriteBatch.Draw(blockOn, drawpos, rectangle, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
									return;
								}
							}
						}
					}

					if (modPlayer.GuardianGauntletCharge > (70 * player.GetTotalAttackSpeed(DamageClass.Melee) - player.HeldItem.useTime) / 2.5f && player.HeldItem.ModItem is OrchidModGuardianGauntlet)
					{
						int val = textureGauntletOn.Height;
						if (modPlayer.GuardianGauntletCharge >= 180f)
						{
							drawpos = new Vector2(position.X - 10, position.Y - 96 * player.gravDir + 6f * (player.gravDir - 1));
							spriteBatch.Draw(textureGauntletReady, drawpos, null, Color.White * 0.8f, 0f, Vector2.Zero, 1f, effect, 0f);
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
						drawpos = new Vector2(position.X - 8, position.Y - 94 * player.gravDir + 4f * (player.gravDir - 1));
						spriteBatch.Draw(textureGauntletOff, drawpos, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
						drawpos = new Vector2(position.X - 8, position.Y - 94 * player.gravDir + textureGauntletOn.Height - val + 4f * (player.gravDir - 1));
						if (player.gravDir < 0) drawpos.Y -= (textureGauntletOn.Height - rectangle.Height);
						spriteBatch.Draw(textureGauntletOn, drawpos, rectangle, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
					}
					else
					{
						int projectileType = ModContent.ProjectileType<GuardianShieldAnchor>();
						int projectileType2 = ModContent.ProjectileType<GuardianGauntletAnchor>();
						int projectileType3 = ModContent.ProjectileType<GuardianHorizonLanceAnchor>();
						for (int i = 0; i < Main.projectile.Length; i++)
						{
							Projectile proj = Main.projectile[i];
							if (proj.active && proj.owner == player.whoAmI && (proj.type == projectileType || proj.type == projectileType2 || proj.type == projectileType3) && proj.ai[0] > 0f && proj.localAI[0] > 0f)
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
								drawpos = new Vector2(position.X - 10, position.Y - 92 * player.gravDir + 3f * (player.gravDir - 1));
								spriteBatch.Draw(blockOff, drawpos, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
								drawpos = new Vector2(position.X - 10, position.Y - 92 * player.gravDir + blockOn.Height - val + 3f * (player.gravDir - 1));
								if (player.gravDir < 0) drawpos.Y -= (blockOn.Height - rectangle.Height);
								spriteBatch.Draw(blockOn, drawpos, rectangle, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
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