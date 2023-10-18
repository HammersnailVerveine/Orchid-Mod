using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using OrchidMod.Common.UIs;
using System.Collections.Generic;
using ReLogic.Content;
using Terraria.ID;

namespace OrchidMod.Content.Gambler.UI
{
	public class GamblerDiceUIState : OrchidUIState
	{
		public static Texture2D DiceTexture;
		public static int DiceTextureType;

		public override int InsertionIndex(List<GameInterfaceLayer> layers)
			=> layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

		public override void OnInitialize()
		{
			DiceTexture ??= ModContent.Request<Texture2D>("OrchidMod/Content/Gambler/Weapons/Dice/GamblingDie_UI", AssetRequestMode.ImmediateLoad).Value;
			DiceTextureType = ItemID.None;
			Recalculate();
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Recalculate();
			Player player = Main.player[Main.myPlayer];
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			
			Vector2 vector = new Vector2((float)((int)(Main.LocalPlayer.position.X - Main.screenPosition.X) - Main.GameViewMatrix.Translation.X - (float)(Main.LocalPlayer.bodyFrame.Width / 2) + (float)(Main.LocalPlayer.width / 2)), (float)((int)(Main.LocalPlayer.position.Y - Main.screenPosition.Y) - Main.GameViewMatrix.Translation.Y + (float)Main.LocalPlayer.height - (float)Main.LocalPlayer.bodyFrame.Height + 12f + player.gfxOffY)) + Main.LocalPlayer.bodyPosition + new Vector2((float)(Main.LocalPlayer.bodyFrame.Width / 2));
			vector *= Main.GameViewMatrix.Zoom;
			vector /= Main.UIScale;

			this.Left.Set(vector.X, 0f);
			this.Top.Set(vector.Y, 0f);

			CalculatedStyle dimensions = GetDimensions();
			Point point = new Point((int)dimensions.X, (int)dimensions.Y);
			int width = (int)Math.Ceiling(dimensions.Width);
			int height = (int)Math.Ceiling(dimensions.Height);

			if (!player.dead)
			{
				if (modPlayer.gamblerDieDisplay)
				{
					Rectangle newBounds = DiceTexture.Bounds;
					newBounds.Height /= 13;
					newBounds.Width /= 7;
					Rectangle newBoundsNumber = DiceTexture.Bounds;
					newBoundsNumber.Height /= 13;
					newBoundsNumber.Width /= 7;
					Rectangle newBoundsNumberPrevious = DiceTexture.Bounds;
					newBoundsNumberPrevious.Height /= 13;
					newBoundsNumberPrevious.Width /= 7;
					int newFrame = GetDieFrame(modPlayer);
					newBounds.Y += newBounds.Height * newFrame;
					newBoundsNumber.Y += newBounds.Height * newFrame;
					newBoundsNumberPrevious.Y += newBounds.Height * (newFrame + 7);

					newBoundsNumber.X += newBoundsNumber.Width * modPlayer.gamblerDieValueCurrent;
					newBoundsNumberPrevious.X += newBoundsNumberPrevious.Width * modPlayer.gamblerDieValuePrevious;

					Vector2 newPosition = new Vector2(point.X -= newBounds.Width / 2, point.Y - 100);

					spriteBatch.Draw(DiceTexture, newPosition, newBounds, Color.White);
					spriteBatch.Draw(DiceTexture, newPosition, newBoundsNumber, Color.White);
					if (newFrame != 6) spriteBatch.Draw(DiceTexture, newPosition, newBoundsNumberPrevious, Color.White);
				}
			}
		}

		public int GetDieFrame(OrchidGambler modPlayer)
		{
			if (modPlayer.gamblerDieAnimation >= OrchidModGamblerDie.AnimationDuration) return 6;
			else
			{
				int counter = 0 + modPlayer.gamblerDieAnimation;
				int frame = 0;
				while (counter > 3)
				{
					counter -= 3;
					frame++;
				}
				return frame;
			}
		}
	}
}