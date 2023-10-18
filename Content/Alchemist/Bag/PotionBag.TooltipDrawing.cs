using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace OrchidMod.Content.Alchemist.Bag
{
	public partial class PotionBag : ModItem
	{
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (InShop) return;

			var index = tooltips.FindLastIndex(i => i.Mod == "Terraria" && i.Name.StartsWith("Tooltip"));
			if (index++ < 0) return;

			var counter = 0;
			for (int i = 0; i < 3; i++)
			{
				tooltips.Insert(index++, new TooltipLine(Mod, "PotionBagSpace#" + counter++, INVISIBLE_LINE));
			}

			var keys = PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus[TriggerNames.SmartSelect];
			var autoSelectKey = keys.Any() ? keys[0] : null;
			var showInfo = autoSelectKey == null || PlayerInput.Triggers.Current.SmartSelect;

			keys = PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus[TriggerNames.MouseRight];
			var interactKey = keys.Any() ? keys[0] : null;

			keys = PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus[TriggerNames.Hotbar1];
			var hotbar1Key = keys.Any() ? keys[0] : null;

			keys = PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus[TriggerNames.Hotbar8];
			var hotbar8Key = keys.Any() ? keys[0] : null;

			keys = PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus[TriggerNames.Hotbar9];
			var hotbar9Key = keys.Any() ? keys[0] : null;

			keys = PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus[TriggerNames.Hotbar10];
			var hotbar10Key = keys.Any() ? keys[0] : null;

			counter = 0;
			void InsertInfoLine(string text)
			{
				tooltips.Insert(index++, new TooltipLine(Mod, "InterfaceInfo" + counter++, text)
				{
					OverrideColor = Color.Gray
				});
			}

			if (showInfo)
			{
				var dyeName = dye.IsAir ? "None" : String.Join(" ", dye.HoverName.Split(" ").Where(i => !i.Equals("Dye")));

				InsertInfoLine($"Change state     – 'Interact' key ({interactKey}) without item");
				InsertInfoLine($"Put in inventory – 'Interact' key ({interactKey}) with item");
				InsertInfoLine($"Pull out potion  – 'Hotbar #1-8' keys ({hotbar1Key}-{hotbar8Key})");
				InsertInfoLine($"Pull out potions – 'Hotbar #9' key ({hotbar9Key})");
				InsertInfoLine($"Pull out dye     – 'Hotbar #10' key ({hotbar10Key})");
				InsertInfoLine($"Dye: «{dyeName}»");
			}
			else
			{
				InsertInfoLine($"Hold 'Auto Select' key ({autoSelectKey}) to see more information");
			}

			var text = "State: ";

			if (IsActive)
			{
				text += $"[c/{Colors.AlphaDarken(new Color(150, 240, 100)).Hex3()}:Active] ";
				text += $"[c/{Colors.AlphaDarken(Color.Gray).Hex3()}:(Displayed in mixing UI)]";
			}
			else
			{
				text += $"[c/{Colors.AlphaDarken(new Color(240, 105, 100)).Hex3()}:Not active] ";
				text += $"[c/{Colors.AlphaDarken(Color.Gray).Hex3()}:(Will not be displayed in mixing UI)]";
			}

			var line = new TooltipLine(Mod, "PotionBagState", text);
			tooltips.Insert(index, line);
		}

		public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
			=> !line.Name.StartsWith("PotionBagSpace#");

		public override void PostDrawTooltip(ReadOnlyCollection<DrawableTooltipLine> lines)
		{
			var spaceLines = lines.Where(i => i.Name.StartsWith("PotionBagSpace#"));

			if (!spaceLines.Any()) return;

			var spriteBatch = Main.spriteBatch;

			var firstLine = spaceLines.First();
			var lastLine = spaceLines.Last();

			var topPos = new Vector2(firstLine.X, firstLine.Y);
			var bottomPos = new Vector2(lastLine.X, lastLine.Y + (int)ChatManager.GetStringSize(FontAssets.MouseText.Value, lastLine.Text, Vector2.One, -1f).Y - 10);

			(float width, float height) = GetTooltipDrawInfo(lines);

			// Background / Shadows (Shading behind lines) / Lines
			{
				var shadingTexture = OrchidAssets.GetExtraTexture(18);

				if (!Main.SettingsEnabled_OpaqueBoxBehindTooltips)
				{
					spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)topPos.X + 2, (int)topPos.Y, (int)width - 4, (int)(bottomPos.Y - topPos.Y)), Color.Black * 0.2f);
				}

				spriteBatch.Draw(shadingTexture.Value, new Rectangle((int)topPos.X + 2, (int)topPos.Y, (int)width - 4, shadingTexture.Height()), Color.White);
				spriteBatch.Draw(shadingTexture.Value, new Rectangle((int)bottomPos.X + 2, (int)bottomPos.Y - shadingTexture.Height(), (int)width - 4, shadingTexture.Height()), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0f);

				DrawTooltipWhiteLine(spriteBatch, topPos, width);
				DrawTooltipWhiteLine(spriteBatch, bottomPos, width);
			}

			// Item slots
			{
				var inventoryScale = 0.85f;
				var slotTexture = ModContent.Request<Texture2D>("OrchidMod/Content/Alchemist/UI/Textures/PotionBagSlot");

				(Main.inventoryScale, inventoryScale) = (inventoryScale, Main.inventoryScale);
				(TextureAssets.InventoryBack, slotTexture) = (slotTexture, TextureAssets.InventoryBack);

				var texture = TextureAssets.InventoryBack.Value;
				var startPos = topPos + new Vector2(width * 0.5f, height * 0.5f) - new Vector2(SLOTS_X, SLOTS_Y) / 2f * 56f * Main.inventoryScale + new Vector2(0, -2);
				var posMult = 56 * Main.inventoryScale;

				for (int j = 0; j < SLOTS_Y; j++)
				{
					for (int i = 0; i < SLOTS_X; i++)
					{
						var slotPos = startPos + new Vector2(i, j) * posMult;

						spriteBatch.Draw(TextureAssets.InventoryBack.Value, slotPos, null, Color.White * 0.8f, 0f, Vector2.Zero, Main.inventoryScale, SpriteEffects.None, 0f);
						ItemSlot.Draw(spriteBatch, inventory, -21, j * SLOTS_X + i, slotPos, Color.White);
					}
				}

				(TextureAssets.InventoryBack, slotTexture) = (slotTexture, TextureAssets.InventoryBack);
				(Main.inventoryScale, inventoryScale) = (inventoryScale, Main.inventoryScale);
			}
		}

		// ...

		private static (float width, float height) GetTooltipDrawInfo(ReadOnlyCollection<DrawableTooltipLine> lines)
		{
			var width = 0f;
			var height = 0f;

			for (int j = 0; j < lines.Count; j++)
			{
				var line = lines[j];
				var stringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, line.Text, Vector2.One, -1f);

				if (stringSize.X > width)
				{
					width = stringSize.X;
				}

				if (line.Name.Contains("PotionBagSpace#"))
				{
					height += stringSize.Y;
				}
			}

			return (width, height);
		}

		private static void DrawTooltipWhiteLine(SpriteBatch spriteBatch, Vector2 position, float width)
		{
			var lineRectangle = new Rectangle((int)position.X, (int)position.Y, (int)width, 2);

			for (int i = 0; i < 4; i++)
			{
				var shadowRectangle = lineRectangle;
				shadowRectangle.X += (int)ChatManager.ShadowDirections[i].X * 2;
				shadowRectangle.Y += (int)ChatManager.ShadowDirections[i].Y * 2;
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, shadowRectangle, Color.Black);
			}

			spriteBatch.Draw(TextureAssets.MagicPixel.Value, lineRectangle, Colors.AlphaDarken(Color.White));
		}
	}
}