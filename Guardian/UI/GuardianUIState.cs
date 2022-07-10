using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.UIs;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace OrchidMod.Guardian.UI
{
	public class GuardianUIState : OrchidUIState
	{
		public Color backgroundColor = Color.White;
		public static Texture2D textureBlockOn;
		public static Texture2D textureBlockOff;
		public static Texture2D textureSlamOn;
		public static Texture2D textureSlamOff;

		public static Texture2D textureHammerMain;
		public static Texture2D textureHammerIcon;
		public static Texture2D textureHammerIconBig;

		public override int InsertionIndex(List<GameInterfaceLayer> layers)
			=> layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
		public override void OnInitialize()
		{
			textureBlockOn ??= ModContent.Request<Texture2D>("OrchidMod/Guardian/UI/Textures/BlockBarOn", AssetRequestMode.ImmediateLoad).Value;
			textureBlockOff ??= ModContent.Request<Texture2D>("OrchidMod/Guardian/UI/Textures/BlockBarOff", AssetRequestMode.ImmediateLoad).Value;
			textureSlamOn ??= ModContent.Request<Texture2D>("OrchidMod/Guardian/UI/Textures/SlamBarOn", AssetRequestMode.ImmediateLoad).Value;
			textureSlamOff ??= ModContent.Request<Texture2D>("OrchidMod/Guardian/UI/Textures/SlamBarOff", AssetRequestMode.ImmediateLoad).Value;
			textureHammerMain ??= ModContent.Request<Texture2D>("OrchidMod/Guardian/UI/Textures/HammerBar", AssetRequestMode.ImmediateLoad).Value;
			textureHammerIcon ??= ModContent.Request<Texture2D>("OrchidMod/Guardian/UI/Textures/HammerIcon1", AssetRequestMode.ImmediateLoad).Value;
			textureHammerIconBig ??= ModContent.Request<Texture2D>("OrchidMod/Guardian/UI/Textures/HammerIcon2", AssetRequestMode.ImmediateLoad).Value;


			Width.Set(10f, 0f);
			Height.Set(10f, 0f);
			Left.Set(Main.screenWidth / 2, 0f);
			Top.Set(Main.screenHeight / 2, 0f);
			backgroundColor = Color.White;

			Recalculate();
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Recalculate();
			Player player = Main.LocalPlayer;

			Vector2 vector = (player.position + new Vector2(player.width * 0.5f, player.gravDir > 0 ? player.height - 10 + player.gfxOffY : 10 + player.gfxOffY)).Floor();
			vector = Vector2.Transform(vector - Main.screenPosition, Main.GameViewMatrix.EffectMatrix * Main.GameViewMatrix.ZoomMatrix) / Main.UIScale;

			this.Left.Set(vector.X, 0f);
			this.Top.Set(vector.Y, 0f);

			CalculatedStyle dimensions = GetDimensions();
			Point point = new Point((int)dimensions.X, (int)dimensions.Y + 20);
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();

			if (!player.dead)
			{
				if (modPlayer.guardianDisplayUI > 0)
				{
					// Texture = 16*16
					int offSet = (int)(modPlayer.guardianBlockMax / 2 * 18) + 8;
					for (int i = 0 ; i < modPlayer.guardianBlockMax ; i ++) {
						Texture2D texture = modPlayer.guardianBlock > i ? textureBlockOn : textureBlockOff;
						spriteBatch.Draw(texture, new Vector2(point.X - offSet + 18 * i, point.Y), backgroundColor);
					}
					
					for (int i = 0 ; i < modPlayer.guardianSlamMax ; i ++) {
						Texture2D texture = modPlayer.guardianSlam > i ? textureSlamOn : textureSlamOff;
						spriteBatch.Draw(texture, new Vector2(point.X - offSet + 18 * i, point.Y + 18), backgroundColor);
					}

					if (modPlayer.holdingHammer)
					{
						Vector2 hammerPosition = new Vector2(point.X - textureHammerMain.Width / 2, point.Y - 100);
						spriteBatch.Draw(textureHammerMain, hammerPosition, backgroundColor);

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

					int projectileType = ModContent.ProjectileType<GuardianShieldAnchor>();
					for (int i = 0 ; i < Main.projectile.Length ; i ++)
					{
						Projectile proj = Main.projectile[i];
						if (proj.active && proj.owner == player.whoAmI && proj.type == projectileType && proj.ai[0] > 0f && proj.localAI[0] > 0f)
						{
							int val = 11;
							float block = proj.ai[0];
							while (block < proj.localAI[0]) 
							{
								block += proj.localAI[0] / 10f;
								val --;
							}
							val = val > 10 ? 10 : val;
							Texture2D texture = ModContent.Request<Texture2D>("OrchidMod/Guardian/UI/Textures/BlockDuration" + val, AssetRequestMode.ImmediateLoad).Value;
							spriteBatch.Draw(texture, new Vector2(point.X - 10, point.Y - 90), backgroundColor);
							return;
						}
					}
				}
			}
		}
	}
}