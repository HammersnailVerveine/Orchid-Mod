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

		public static Texture2D textureHorizonLanceOn;
		public static Texture2D textureHorizonLanceOff;
		public static Texture2D textureHorizonLanceReady;

		public static Texture2D textureGuardianNeedleOn;
		public static Texture2D textureGuardianNeedleOff;
		public static Texture2D textureGuardianNeedleReady;

		public static Texture2D blockOn;
		public static Texture2D blockOff;

		public static Texture2D textureIconStandardOn;
		public static Texture2D textureIconStandardOff;
		public static Texture2D textureIconStandardHighlight;
		public static Texture2D textureIconRuneOn;
		public static Texture2D textureIconRuneOff;

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

			textureHorizonLanceOn ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/HorizonLanceOn", AssetRequestMode.ImmediateLoad).Value;
			textureHorizonLanceOff ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/HorizonLanceOff", AssetRequestMode.ImmediateLoad).Value;
			textureHorizonLanceReady ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/HorizonLanceReady", AssetRequestMode.ImmediateLoad).Value;

			textureGuardianNeedleOn ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/GuardianNeedleOn", AssetRequestMode.ImmediateLoad).Value;
			textureGuardianNeedleOff ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/GuardianNeedleOff", AssetRequestMode.ImmediateLoad).Value;
			textureGuardianNeedleReady ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/GuardianNeedleReady", AssetRequestMode.ImmediateLoad).Value;

			blockOn ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/BlockOn", AssetRequestMode.ImmediateLoad).Value;
			blockOff ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/BlockOff", AssetRequestMode.ImmediateLoad).Value;

			textureIconStandardOn ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/IconStandardOn", AssetRequestMode.ImmediateLoad).Value;
			textureIconStandardOff ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/IconStandardOff", AssetRequestMode.ImmediateLoad).Value;
			textureIconStandardHighlight ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/IconStandardHighlight", AssetRequestMode.ImmediateLoad).Value;
			textureIconRuneOn ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/IconRuneOn", AssetRequestMode.ImmediateLoad).Value;
			textureIconRuneOff ??= ModContent.Request<Texture2D>("OrchidMod/Content/Guardian/UI/Textures/IconRuneOff", AssetRequestMode.ImmediateLoad).Value;

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

				bool minHoldTimer = modPlayer.ChargeHoldTimer > ModContent.GetInstance<OrchidClientConfig>().GuardianMinHoldTimer;
				bool drawAtCursor = ModContent.GetInstance<OrchidClientConfig>().GuardianChargeCursor;
				Vector2 position = (player.position + new Vector2(player.width * 0.5f, player.height + player.gfxOffY + 12)).Floor();
				if (player.gravDir < 0) position.Y -= 81;
				Vector2 drawpos = position;
				position = position - Main.screenPosition;
				SpriteEffects effect = player.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;

				Texture2D chargeTextureOn = null;
				Texture2D chargeTextureOff = null;
				Texture2D chargeTextureReady = null;

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
					Vector2 drawposHighlight = new Vector2(position.X - offSet - 2 + 18 * i, position.Y + 16 * player.gravDir + 2f * (player.gravDir - 1));
					spriteBatch.Draw(texture, drawpos, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
					if (modPlayer.SlamCostUI > i)
					{
						spriteBatch.Draw(textureSlamHighlight, drawposHighlight, null, (check ? Color.White * 0.8f : Color.DarkGray) * 0.8f, 0f, Vector2.Zero, 1f, effect, 0f);
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
					if (modPlayer.GuardianCounter && modPlayer.GuardianCounterTime > 0 && ((modPlayer.GuardianSlam <= 1 && i == 0) || modPlayer.GuardianSlam - 1 == i))
					{
						float flash = 0.4f + (float)Math.Sin(Main.timeForVisualEffects * 0.1f) * 0.2f;
						Color color = !modPlayer.GuardianCounterHorizon
							? new Color(flash, 0, 0, 1f - flash)
							: GuardianHorizonLanceAnchor.GetHorizonGlowColor(Math.Sin(Main.timeForVisualEffects * 0.05f), 0.4f + (float)Math.Cos(Main.timeForVisualEffects * 0.1f) * 0.1f, 0.8f - flash);
						int chargeHeight = (int)(Math.Min(modPlayer.GuardianCounterTime / 30f, 1f) * textureSlamHighlight.Height);
						spriteBatch.Draw(textureSlamHighlight, drawposHighlight + new Vector2(0, textureSlamHighlight.Height - chargeHeight), new Rectangle(0, textureSlamHighlight.Height - chargeHeight, textureSlamHighlight.Width, chargeHeight), color);
					}
				}

				offSet = (int)((textureIconRuneOn.Width + 2) * 0.5f);
				if (modPlayer.RuneProjectiles.Count > 0)
				{
					float colorMult = (modPlayer.RuneProjectiles[0].timeLeft > 275 || player.HeldItem.ModItem is OrchidModGuardianStandard) ? 1f : (float)Math.Abs(Math.Sin((modPlayer.RuneProjectiles[0].timeLeft * 0.5f) / Math.PI / 4f));
					offSet = (int)(offSet * (modPlayer.GuardianCurrentStandardAnchor == null ? 1f : 2f));
					drawpos = new Vector2(position.X - offSet, position.Y + 36 * player.gravDir + (player.gravDir - 1));
					spriteBatch.Draw(textureIconRuneOff, drawpos, null, Color.White * colorMult, 0f, Vector2.Zero, 1f, effect, 0f);
					spriteBatch.Draw(textureIconRuneOn, drawpos, null, Color.White * colorMult, 0f, Vector2.Zero, 1f, effect, 0f);
					offSet -= textureIconRuneOn.Width + 2;
				}

				if (modPlayer.GuardianCurrentStandardAnchor != null && modPlayer.GuardianCurrentStandardAnchor.ModProjectile is GuardianStandardAnchor anchor && anchor.StandardItem.ModItem is OrchidModGuardianStandard standardItem && anchor.BuffItem != null)
				{
					float standardDuration = modPlayer.GuardianCurrentStandardAnchor.ai[1];
					float standardDurationHighlight = 0f;
					if (standardDuration > standardItem.StandardDuration * modPlayer.GuardianStandardTimer)
					{
						standardDurationHighlight = standardDuration - standardItem.StandardDuration * modPlayer.GuardianStandardTimer;
						standardDuration = standardItem.StandardDuration * modPlayer.GuardianStandardTimer;

						if (standardDurationHighlight > standardItem.StandardDuration * modPlayer.GuardianStandardTimer)
						{ // can happen if modPlayer.GuardianStandardTimer lowers while a standard is active
							standardDurationHighlight = standardItem.StandardDuration * modPlayer.GuardianStandardTimer;
						}
					}

					float colorMult = standardDuration > 275 ? 1f : (float)Math.Abs(Math.Sin((standardDuration * 0.5f) / Math.PI / 4f));
					int remainingDurationOffset = (int)(textureIconStandardOn.Height * (1 - standardDuration / (standardItem.StandardDuration * modPlayer.GuardianStandardTimer)));
					drawpos = new Vector2(position.X - offSet, position.Y + 36 * player.gravDir + (player.gravDir - 1));

					spriteBatch.Draw(textureIconStandardOff, drawpos, null, Color.White * colorMult, 0f, Vector2.Zero, 1f, effect, 0f);
					Rectangle drawRectangle = new Rectangle(0, remainingDurationOffset, textureIconStandardOn.Width, textureIconStandardOn.Height - remainingDurationOffset);
					spriteBatch.Draw(textureIconStandardOn, drawpos + new Vector2(0, remainingDurationOffset), drawRectangle, Color.White * colorMult, 0f, Vector2.Zero, 1f, effect, 0f);

					if (standardDurationHighlight > 0)
					{ // draws a border if duration is >100% of the standard duration
						remainingDurationOffset = (int)(textureIconStandardHighlight.Height * (1 - standardDurationHighlight / (standardItem.StandardDuration * modPlayer.GuardianStandardTimer)));
						drawRectangle = new Rectangle(0, remainingDurationOffset, textureIconStandardHighlight.Width, textureIconStandardHighlight.Height - remainingDurationOffset);
						spriteBatch.Draw(textureIconStandardHighlight, drawpos + new Vector2(-2, remainingDurationOffset - 2), drawRectangle, Color.White * colorMult * 0.8f, 0f, Vector2.Zero, 1f, effect, 0f);
					}
				}

				if (player.HeldItem.ModItem is OrchidModGuardianItem)
				{
					if (minHoldTimer && modPlayer.GuardianItemCharge > (70 * player.GetTotalAttackSpeed(DamageClass.Melee) - player.HeldItem.useTime) / 2.5f && player.HeldItem.ModItem is OrchidModGuardianHammer)
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
							chargeTextureOff = textureHammerOff;
							chargeTextureOn = textureHammerOn;
							chargeTextureReady = textureHammerReady;
						}
					}

					if (minHoldTimer && modPlayer.GuardianItemCharge > (70 * player.GetTotalAttackSpeed(DamageClass.Melee) - player.HeldItem.useTime) / 2.5f && player.HeldItem.ModItem is OrchidModGuardianStandard)
					{
						chargeTextureOff = textureStandardOff;
						chargeTextureOn = textureStandardOn;
						chargeTextureReady = textureStandardReady;
					}

					if (minHoldTimer && modPlayer.GuardianItemCharge > (70 * player.GetTotalAttackSpeed(DamageClass.Melee) - player.HeldItem.useTime) / 2.5f && player.HeldItem.ModItem is HorizonLance)
					{
						chargeTextureOn = textureHorizonLanceOn;
						chargeTextureOff = textureHorizonLanceOff;
						chargeTextureReady = textureHorizonLanceReady;
					}

					if (minHoldTimer && modPlayer.GuardianItemCharge > (70 * player.GetTotalAttackSpeed(DamageClass.Melee) - player.HeldItem.useTime) / 2.5f && player.HeldItem.ModItem is GuardianNeedle)
					{
						chargeTextureOn = textureGuardianNeedleOn;
						chargeTextureOff = textureGuardianNeedleOff;
						chargeTextureReady = textureGuardianNeedleReady;
					}

					if (minHoldTimer && modPlayer.GuardianItemCharge > (23 * player.GetTotalAttackSpeed(DamageClass.Melee) - player.HeldItem.useTime) / 2.5f && player.HeldItem.ModItem is OrchidModGuardianRune)
					{
						chargeTextureOn = textureRuneOn;
						chargeTextureOff = textureRuneOff;
						chargeTextureReady = textureRuneReady;
					}

					if (player.HeldItem.ModItem is OrchidModGuardianQuarterstaff)
					{
						if (minHoldTimer && modPlayer.GuardianItemCharge > (70 * player.GetTotalAttackSpeed(DamageClass.Melee) - player.HeldItem.useTime) / 2.5f)
						{
							chargeTextureOn = textureQuarterstaffOn;
							chargeTextureOff = textureQuarterstaffOff;
							chargeTextureReady = textureQuarterstaffReady;
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

					if (minHoldTimer && modPlayer.GuardianItemCharge > (70 * player.GetTotalAttackSpeed(DamageClass.Melee) - player.HeldItem.useTime) / 2.5f && player.HeldItem.ModItem is OrchidModGuardianGauntlet)
					{
						chargeTextureOn = textureGauntletOn;
						chargeTextureOff = textureGauntletOff;
						chargeTextureReady = textureGauntletReady;
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

					if (chargeTextureOn != null)
					{
						int val = chargeTextureOn.Height;
						if (modPlayer.GuardianItemCharge < 180f)
						{
							float charge = modPlayer.GuardianItemCharge;
							while (charge < 180f)
							{
								charge += 7.5f;
								val--;
							}
						}

						Rectangle rectangle = chargeTextureOn.Bounds;
						rectangle.Height = val;
						rectangle.Y = chargeTextureOn.Height - val;

						if (drawAtCursor)
						{
							spriteBatch.End();
							spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

							drawpos = Main.MouseScreen + new Vector2(18f, 18f);

							if (modPlayer.GuardianItemCharge >= 180f)
							{
								spriteBatch.Draw(chargeTextureReady, drawpos - new Vector2(2, 2), null, Color.White * 0.8f, 0f, Vector2.Zero, 1f, effect, 0f);
							}

							spriteBatch.Draw(chargeTextureOff, drawpos, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
							drawpos.Y += chargeTextureOn.Height - val;
							spriteBatch.Draw(chargeTextureOn, drawpos, rectangle, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);

							spriteBatch.End();
							spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
						}
						else
						{
							if (modPlayer.GuardianItemCharge >= 180f)
							{
								drawpos = new Vector2(position.X - 11, position.Y - 96 * player.gravDir + 5f * (player.gravDir - 1));
								spriteBatch.Draw(chargeTextureReady, drawpos, null, Color.White * 0.8f, 0f, Vector2.Zero, 1f, effect, 0f);
							}

							drawpos = new Vector2(position.X - 9, position.Y - 94 * player.gravDir + 3f * (player.gravDir - 1));
							spriteBatch.Draw(chargeTextureOff, drawpos, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
							drawpos = new Vector2(position.X - 9, position.Y - 94 * player.gravDir + chargeTextureOn.Height - val + 3f * (player.gravDir - 1));
							if (player.gravDir < 0) drawpos.Y -= (chargeTextureOn.Height - rectangle.Height);
							spriteBatch.Draw(chargeTextureOn, drawpos, rectangle, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
						}
					}
				}

				spriteBatch.End();
				spriteBatch.Begin(spriteBatchSnapshot);
			}
		}
	}
}