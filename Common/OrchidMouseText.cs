using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using OrchidMod.Common.UIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace OrchidMod.Common
{
	public class OrchidMouseText : ILoadable
	{
		private static readonly ITooltipsStyle NullStyle = new ITooltipsStyle.Invisible();
		private static readonly FieldInfo MouseTextCacheField = typeof(Main).GetField("_mouseTextCache", BindingFlags.NonPublic | BindingFlags.Instance);
		private static readonly FieldInfo CursorTextField = MouseTextCacheField?.GetValue(Main.instance).GetType().GetField("cursorText", BindingFlags.Public | BindingFlags.Instance);

		private List<TooltipLine> tooltips;
		private ITooltipsStyle style;
		private Mod mod;

		public bool Visible
			=> tooltips?.Any() ?? false && (((CursorTextField.GetValue(MouseTextCacheField.GetValue(Main.instance))) as string).Equals("[OMMT]"));

		// ...

		void ILoadable.Load(Mod mod)
		{
			this.mod = mod;
			this.tooltips = new();
			this.style = NullStyle;

			if (CursorTextField is null)
			{
				throw new Exception($"{nameof(OrchidMouseText)}.{nameof(CursorTextField)} is null...");
			}

			IL.Terraria.Main.MouseTextInner += (il) =>
			{
				var c = new ILCursor(il);

				c.EmitDelegate(() =>
				{
					if (Visible)
					{
						Draw(Main.spriteBatch);
						return true;
					}

					return false;
				});

				var label = c.DefineLabel();

				c.Emit(Mono.Cecil.Cil.OpCodes.Brfalse, label);
				c.Emit(Mono.Cecil.Cil.OpCodes.Ret);
				c.MarkLabel(label);
			};

			Main.OnPostDraw += ResetTooltipsData;
		}

		void ILoadable.Unload()
		{
			Main.OnPostDraw -= ResetTooltipsData;
		}

		// ...

		private void Draw(SpriteBatch spriteBatch)
		{
			var drawableLines = tooltips.Select((TooltipLine x, int i) => new DrawableTooltipLine(x, i, 0, 0, Color.White)).ToList<DrawableTooltipLine>();
			var position = new Vector2(Main.mouseX + 14, Main.mouseY + 14) + style.PositionOffset;
			var zero = Vector2.Zero;

			if (Main.ThickMouse)
			{
				position += new Vector2(6);
			}

			for (int j = 0; j < drawableLines.Count; j++)
			{
				var stringSize = ChatManager.GetStringSize(drawableLines[j].Font, drawableLines[j].Text, Vector2.One, -1f);

				if (stringSize.X > zero.X) zero.X = stringSize.X;

				zero.Y += stringSize.Y;
			}

			var e = 4;

			if (position.X + zero.X + e > Main.screenWidth) position.X = (int)(Main.screenWidth - zero.X - e);
			if (position.Y + zero.Y + e > Main.screenHeight) position.Y = (int)(Main.screenHeight - zero.Y - e);
			if (zero.X < 20) zero.X = 20;
			if (zero.Y < 20) zero.Y = 20;

			style.PreDraw(spriteBatch, new Rectangle((int)position.X, (int)position.Y, (int)zero.X, (int)zero.Y));

			var colorPulsing = Main.mouseTextColor / 255f;

			foreach (var line in drawableLines)
			{
				var color = (line.OverrideColor ?? Color.White) * colorPulsing;
				var linePosition = position + new Vector2(line.X, line.Y);

				line.X = (int)linePosition.X;
				line.Y = (int)linePosition.Y;

				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, line.Font, line.Text, linePosition, color, line.Rotation, line.Origin, line.BaseScale, line.MaxWidth, line.Spread);

				position.Y += line.Font.MeasureString(line.Text).Y * line.BaseScale.Y;
			}
		}

		// ...

		public static void SetTooltipsData(string text, ITooltipsStyle style = null)
		{
			var instance = ModContent.GetInstance<OrchidMouseText>();

			instance.tooltips.Clear();
			instance.tooltips.Add(new TooltipLine(instance.mod, "Text", text));

			SetTooltipsData(instance.tooltips, style);
		}

		public static void SetTooltipsData(List<TooltipLine> tooltipLines, ITooltipsStyle style = null)
		{
			Main.mouseText = true;

			var instance = ModContent.GetInstance<OrchidMouseText>();

			instance.tooltips = tooltipLines ?? instance.tooltips;
			instance.style = style ?? NullStyle;

			Main.instance.MouseText("[OMMT]");
		}

		private static void ResetTooltipsData(GameTime _)
		{
			var instance = ModContent.GetInstance<OrchidMouseText>();

			instance.tooltips.Clear();
			instance.style = NullStyle;
		}
	}

	public interface ITooltipsStyle
	{
		virtual Vector2 PositionOffset => Vector2.Zero;
		void PreDraw(SpriteBatch sb, Rectangle rectangle);

		// ...

		public struct Invisible : ITooltipsStyle
		{
			public void PreDraw(SpriteBatch sb, Rectangle rectangle) { }
		}

		public struct Vanilla : ITooltipsStyle
		{
			public Color Color;

			public Vanilla()
			{
				Color = new Color(63, 65, 151, 255) * 0.785f;
			}

			public Vanilla(Color color)
			{
				Color = color;
			}

			public Vector2 PositionOffset => new(10);

			public void PreDraw(SpriteBatch sb, Rectangle rectangle)
			{
				var texture = TextureAssets.InventoryBack13.Value;

				var x = rectangle.X - 14;
				var y = rectangle.Y - 9;
				var w = rectangle.Width + 28;
				var h = rectangle.Height + 12;

				sb.Draw(texture, new Rectangle(x, y, 10, 10), new Rectangle?(new Rectangle(0, 0, 10, 10)), Color);
				sb.Draw(texture, new Rectangle(x + 10, y, w - 20, 10), new Rectangle?(new Rectangle(10, 0, 10, 10)), Color);
				sb.Draw(texture, new Rectangle(x + w - 10, y, 10, 10), new Rectangle?(new Rectangle(texture.Width - 10, 0, 10, 10)), Color);
				sb.Draw(texture, new Rectangle(x, y + 10, 10, h - 20), new Rectangle?(new Rectangle(0, 10, 10, 10)), Color);
				sb.Draw(texture, new Rectangle(x + 10, y + 10, w - 20, h - 20), new Rectangle?(new Rectangle(10, 10, 10, 10)), Color);
				sb.Draw(texture, new Rectangle(x + w - 10, y + 10, 10, h - 20), new Rectangle?(new Rectangle(texture.Width - 10, 10, 10, 10)), Color);
				sb.Draw(texture, new Rectangle(x, y + h - 10, 10, 10), new Rectangle?(new Rectangle(0, texture.Height - 10, 10, 10)), Color);
				sb.Draw(texture, new Rectangle(x + 10, y + h - 10, w - 20, 10), new Rectangle?(new Rectangle(10, texture.Height - 10, 10, 10)), Color);
				sb.Draw(texture, new Rectangle(x + w - 10, y + h - 10, 10, 10), new Rectangle?(new Rectangle(texture.Width - 10, texture.Height - 10, 10, 10)), Color);
			}
		}
	}
}