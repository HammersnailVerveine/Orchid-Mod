using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.UIs;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;

namespace OrchidMod.Content.Shapeshifter
{
	public class ShapeshifterUIState : OrchidUIState
	{
		public static Texture2D TextureUI;
		public static Texture2D TextureUIBack;

		public override int InsertionIndex(List<GameInterfaceLayer> layers)
			=> layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

		public override void OnInitialize()
		{
			Width.Set(0f, 0f);
			Height.Set(0f, 0f);
			Left.Set(Main.screenWidth / 2, 0f);
			Top.Set(Main.screenHeight / 2, 0f);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Recalculate();
			Player player = Main.LocalPlayer;
			OrchidShapeshifter shapeshifter = player.GetModPlayer<OrchidShapeshifter>();

			if (!player.dead && shapeshifter.IsShapeshifted && TextureUI != null)
			{
				if (shapeshifter.Shapeshift.ShapeshiftTypeUI != ShapeshifterShapeshiftTypeUI.None)
				{
					Vector2 position = (player.position + new Vector2(player.width * 0.5f, player.height + player.gfxOffY + 12)).Floor();
					Vector2 drawpos = position;
					position -= Main.screenPosition;

					spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
					spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

					int uiCount = 0;
					int uiCountMax = 0;

					shapeshifter.Shapeshift.ShapeshiftGetUIInfo(shapeshifter.ShapeshiftAnchor.Projectile, shapeshifter.ShapeshiftAnchor, player, shapeshifter, ref uiCount, ref uiCountMax);

					if (shapeshifter.Shapeshift.ShapeshiftTypeUI == ShapeshifterShapeshiftTypeUI.List)
					{ // uiCount is the number of "active" symbol out of "uiCountMax"
						int offSet = (int)(uiCountMax / 2f * (TextureUIBack.Width + 2));
						int offSetIcons = offSet;
						for (int i = 0; i < uiCountMax; i++)
						{
							Texture2D texture = uiCount > i ? TextureUI : TextureUIBack;
							drawpos = new Vector2(position.X - offSet + (texture.Width + 2) * i, position.Y);
							spriteBatch.Draw(texture, drawpos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
						}
					}

					spriteBatch.End();
					spriteBatch.Begin(spriteBatchSnapshot);
				}
			}
		}
	}
}