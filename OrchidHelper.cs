using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace OrchidMod
{
	public static class OrchidHelper
	{
		public static Texture2D GetExtraTexture(int index) => ModContent.GetTexture("OrchidMod/Assets/Textures/Misc/Extra_" + index);
		public static OrchidModPlayer GetOrchidPlayer(this Player player) => player.GetModPlayer<OrchidModPlayer>(); 

		// ...

		public static void SpawnDustCircle(Vector2 center, float radius, int count, int type, Action<Dust> onSpawn = null)
		{
			for (int i = 0; i < count; i++)
			{
				Vector2 position = center + new Vector2(radius, 0).RotatedBy(i / (float)count * MathHelper.TwoPi);
				var dust = Dust.NewDustPerfect(position, type);
				onSpawn?.Invoke(dust);
			}
		}

		public static void SpawnDustCircle(Vector2 center, float radius, int count, Func<int, int> type, Action<Dust, int, float> onSpawn = null)
		{
			for (int i = 0; i < count; i++)
			{
				float angle = i / (float)count * MathHelper.TwoPi;
				Vector2 position = center + new Vector2(radius, 0).RotatedBy(angle);
				int dustType = type?.Invoke(i) ?? -1;

				if (dustType != -1)
				{
					var dust = Dust.NewDustPerfect(position, dustType);
					onSpawn?.Invoke(dust, i, angle);
				}
			}
		}

		public static void DrawSimpleItemGlowmaskInWorld(Item item, SpriteBatch spriteBatch, Texture2D texture, Color color, float rotation, float scale)
		{
			Vector2 offset = new Vector2(0, 2); // Unnecessary in 1.4
			spriteBatch.Draw(texture, item.position - Main.screenPosition + item.Size * 0.5f + offset, null, color, rotation, item.Size * 0.5f, scale, SpriteEffects.None, 0f);
		}

		public static void DrawSimpleItemGlowmaskOnPlayer(PlayerDrawInfo drawInfo, Texture2D texture, Color? drawColor = null)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			Item heldItem = drawPlayer.inventory[drawPlayer.selectedItem];
			Color color = drawColor ?? new Color(250, 250, 250);

			if (drawInfo.shadow != 0f || drawPlayer.frozen || ((drawPlayer.itemAnimation <= 0 || heldItem.useStyle == 0) && (heldItem.holdStyle <= 0 || drawPlayer.pulley)) || heldItem.type <= ItemID.None || drawPlayer.dead || heldItem.noUseGraphic || (drawPlayer.wet && heldItem.noWet))
			{
				return;
			}

			Color color20 = Lighting.GetColor((int)((double)drawInfo.position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)drawInfo.position.Y + (double)drawPlayer.height * 0.5) / 16.0));
			Vector2 vector = drawInfo.itemLocation;

			if (drawPlayer.shroomiteStealth && heldItem.ranged)
			{
				float num64 = drawPlayer.stealth;
				if ((double)num64 < 0.03)
				{
					num64 = 0.03f;
				}
				float num65 = (1f + num64 * 10f) / 11f;
				color20 = new Color((int)((byte)((float)color20.R * num64)), (int)((byte)((float)color20.G * num64)), (int)((byte)((float)color20.B * num65)), (int)((byte)((float)color20.A * num64)));
			}

			if (drawPlayer.setVortex && heldItem.ranged)
			{
				float num66 = drawPlayer.stealth;
				if ((double)num66 < 0.03)
				{
					num66 = 0.03f;
				}
				color20 = color20.MultiplyRGBA(new Color(Vector4.Lerp(Vector4.One, new Vector4(0f, 0.12f, 0.16f, 0f), 1f - num66)));
			}

			Vector2 zero = Vector2.Zero;
			SpriteEffects effect;

			if (drawPlayer.gravDir == 1f)
			{
				if (drawPlayer.direction == 1) effect = SpriteEffects.None;
				else effect = SpriteEffects.FlipHorizontally;
			}
			else
			{
				if (drawPlayer.direction == 1) effect = SpriteEffects.FlipVertically;
				else effect = (SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically);
			}

			ItemSlot.GetItemLight(ref color20, drawPlayer.inventory[drawPlayer.selectedItem], false);

			if (heldItem.useStyle == ItemUseStyleID.HoldingOut)
			{
				if (Item.staff[heldItem.type])
				{
					float num84 = drawPlayer.itemRotation + 0.785f * (float)drawPlayer.direction;
					int num85 = 0;
					int num86 = 0;
					Vector2 zero2 = new Vector2(0f, texture.Height);

					if (drawPlayer.gravDir == -1f)
					{
						if (drawPlayer.direction == -1)
						{
							num84 += 1.57f;
							zero2 = new Vector2(texture.Width, 0f);
							num85 -= texture.Width;
						}
						else
						{
							num84 -= 1.57f;
							zero2 = Vector2.Zero;
						}
					}
					else if (drawPlayer.direction == -1)
					{
						zero2 = new Vector2(texture.Width, texture.Height);
						num85 -= texture.Width;
					}

					Vector2 zero3 = Vector2.Zero;
					ItemLoader.HoldoutOrigin(drawPlayer, ref zero3);
					var drawData = new DrawData(texture, new Vector2((float)((int)(vector.X - Main.screenPosition.X + zero2.X + (float)num85)), (float)((int)(vector.Y - Main.screenPosition.Y + (float)num86))), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), drawPlayer.GetImmuneAlpha(heldItem.GetAlpha(new Color(color.R, color.G, color.B, heldItem.alpha)), 0), num84, zero2 + zero3, heldItem.scale, effect, 0);
					Main.playerDrawData.Add(drawData);

					return;
				}
				else
				{
					Vector2 vector5 = new Vector2((float)(texture.Width / 2), (float)(texture.Height / 2));
					Vector2 vector6 = new Vector2(10f, (float)(texture.Height / 2));
					ItemLoader.HoldoutOffset(drawPlayer.gravDir, heldItem.type, ref vector6);

					int num87 = (int)vector6.X;
					vector5.Y = vector6.Y;
					Vector2 origin3 = new Vector2(-(float)num87, (float)(texture.Height / 2));

					if (drawPlayer.direction == -1)
					{
						origin3 = new Vector2((float)(texture.Width + num87), (float)(texture.Height / 2));
					}

					var drawData = new DrawData(texture, new Vector2((float)((int)(vector.X - Main.screenPosition.X + vector5.X)), (float)((int)(vector.Y - Main.screenPosition.Y + vector5.Y))), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), drawPlayer.GetImmuneAlpha(heldItem.GetAlpha(new Color(color.R, color.G, color.B, heldItem.alpha)), 0), drawPlayer.itemRotation, origin3, heldItem.scale, effect, 0);
					Main.playerDrawData.Add(drawData);

					return;
				}
			}
			else if (drawPlayer.gravDir == -1f)
			{
				var drawData = new DrawData(texture, new Vector2((float)((int)(vector.X - Main.screenPosition.X)), (float)((int)(vector.Y - Main.screenPosition.Y))), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), drawPlayer.GetImmuneAlpha(heldItem.GetAlpha(new Color(color.R, color.G, color.B, heldItem.alpha)), 0), drawPlayer.itemRotation, new Vector2((float)texture.Width * 0.5f - (float)texture.Width * 0.5f * (float)drawPlayer.direction, 0f) + zero, heldItem.scale, effect, 0);
				Main.playerDrawData.Add(drawData);

				return;
			}
			else
			{
				var drawData = new DrawData(texture, new Vector2((float)((int)(vector.X - Main.screenPosition.X)), (float)((int)(vector.Y - Main.screenPosition.Y))), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), drawPlayer.GetImmuneAlpha(heldItem.GetAlpha(new Color(color.R, color.G, color.B, heldItem.alpha)), 0), drawPlayer.itemRotation, new Vector2((float)texture.Width * 0.5f - (float)texture.Width * 0.5f * (float)drawPlayer.direction, (float)texture.Height) + zero, heldItem.scale, effect, 0);
				Main.playerDrawData.Add(drawData);
			}
		}
	}
}
