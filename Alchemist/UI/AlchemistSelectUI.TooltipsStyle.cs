using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.UI
{
	public partial class AlchemistSelectUI
	{
		private struct AlchemistSelectUITooltipsStyle : ITooltipsStyle
		{
			Vector2 ITooltipsStyle.PositionOffset => new Vector2(5);

			void ITooltipsStyle.ModifyDrawableLines(List<DrawableTooltipLine> lines)
			{
				var count = lines.Count;

				if (count <= 1) return;

				lines[0].BaseScale = new Vector2(1.1f);

				var scale = new Vector2(0.85f);

				for (int i = 1; i < count; i++)
				{
					lines[i].BaseScale = scale;
				}
			}

			void ITooltipsStyle.PreDraw(SpriteBatch sb, Rectangle rectangle)
			{
				var texture = Fields.TooltipsBackgroundTexture.Value;
				var color = Color.White * 0.8f;

				var x = rectangle.X - 18;
				var y = rectangle.Y - 15;
				var w = rectangle.Width + 36;
				var h = rectangle.Height + 24;

				sb.Draw(texture, new Rectangle(x, y, 10, 10), new Rectangle?(new Rectangle(0, 0, 10, 10)), color);
				sb.Draw(texture, new Rectangle(x + 10, y, w - 20, 10), new Rectangle?(new Rectangle(10, 0, 10, 10)), color);
				sb.Draw(texture, new Rectangle(x + w - 10, y, 10, 10), new Rectangle?(new Rectangle(texture.Width - 10, 0, 10, 10)), color);
				sb.Draw(texture, new Rectangle(x, y + 10, 10, h - 20), new Rectangle?(new Rectangle(0, 10, 10, 10)), color);
				sb.Draw(texture, new Rectangle(x + 10, y + 10, w - 20, h - 20), new Rectangle?(new Rectangle(10, 10, 10, 10)), color);
				sb.Draw(texture, new Rectangle(x + w - 10, y + 10, 10, h - 20), new Rectangle?(new Rectangle(texture.Width - 10, 10, 10, 10)), color);
				sb.Draw(texture, new Rectangle(x, y + h - 10, 10, 10), new Rectangle?(new Rectangle(0, texture.Height - 10, 10, 10)), color);
				sb.Draw(texture, new Rectangle(x + 10, y + h - 10, w - 20, 10), new Rectangle?(new Rectangle(10, texture.Height - 10, 10, 10)), color);
				sb.Draw(texture, new Rectangle(x + w - 10, y + h - 10, 10, 10), new Rectangle?(new Rectangle(texture.Width - 10, texture.Height - 10, 10, 10)), color);
			}
		}
	}
}