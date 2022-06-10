using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace OrchidMod.Content.Items.Materials
{
	public class JungleLily : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jungle Lily Bud");
			Tooltip.SetDefault("It closed when you picked it up"
							+ "\nMaybe the chemist could help you?");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 5, 0);
			
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<JungleLilyTile>();
		}
	}

	public class JungleLilyTile : OrchidTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileLighted[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.AnchorValidTiles = new int[] { TileID.JungleGrass };
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 4;
			TileObjectData.newTile.RandomStyleRange = 4;
			TileObjectData.addTile(Type);

			this.CreateMapEntry("Jungle Lily", new Color(177, 46, 77));

			DustType = ModContent.DustType<Content.Dusts.BloomingDust>();
			soundType = SoundID.Grass;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			const float power = 0.085f;

			r = 0.85f * power;
			g = 0.65f * power;
			b = 0.05f * power;
		}

		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
		{
			if (!Main.gamePaused && Main.instance.IsActive && (!Lighting.UpdateEveryFrame || Main.rand.NextBool(4)))
			{
				Tile tile = Main.tile[i, j];
				short frameY = tile.TileFrameY;

				if (frameY == 0 && Main.rand.NextBool(20))
				{
					Dust.NewDust(new Vector2(i * 16 + 4, j * 16 + 4), 8, 8, ModContent.DustType<Dusts.JungleLilyDust>(), 0f, 0f);
				}
			}
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 16, ModContent.ItemType<JungleLily>());
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			var tile = Main.tile[i, j];
			if (tile == null || tile.TileFrameX % 36 == 0 || tile.TileFrameY != 18) return;

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
			{
				Color color = Lighting.GetColor(i, j, Color.White);
				float power = 1 - (color.R + color.G + color.B) / 255f / 3f;

				Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
				if (Main.drawToScreen) zero = Vector2.Zero;

				Rectangle rectangle = new Rectangle(tile.TileFrameX - 18, 0, 36, 38);

				var texture = ModContent.GetTexture("OrchidMod/Assets/Textures/Tiles/" + this.Name + "_Glow");
				var offset = new Vector2(20, 22);
				var position = new Vector2(i * 16 + offset.X - (int)Main.screenPosition.X, j * 16 + offset.Y - (int)Main.screenPosition.Y) + zero;
				var origin = new Vector2(texture.Width / 4, texture.Height);
				spriteBatch.Draw(texture, position, rectangle, Color.White * 0.55f * power, 0f, origin, 1f, SpriteEffects.None, 0f);

				for (int k = 0; k < 3; k++)
				{
					offset = new Vector2(20, 22) + new Vector2(2 * power, 0).RotatedBy(k * 2.0943f + Main.GlobalTimeWrappedHourly * 3);
					position = new Vector2(i * 16 + offset.X - (int)Main.screenPosition.X, j * 16 + offset.Y - (int)Main.screenPosition.Y) + zero;
					spriteBatch.Draw(texture, position, rectangle, Color.White * 0.35f * power, 0f, origin, 1f, SpriteEffects.None, 0f);
				}
			}
			spriteBatch.End();
			spriteBatch.Begin();
		}
	}
}