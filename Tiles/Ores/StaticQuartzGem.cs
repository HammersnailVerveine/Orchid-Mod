using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Tiles.Ores
{
	public class StaticQuartzGem : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileValue[Type] = 280;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 500;
			Main.tileObsidianKill[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileLighted[Type] = true;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Static Quartz");
			AddMapEntry(new Color(216, 21, 54), name);

			dustType = 60;
			drop = ItemType<General.Items.Sets.StaticQuartz.StaticQuartz>();
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.15f;
			g = 0f;
			b = 0f;
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			Tile tile = Main.tile[i, j];
			tile.frameX = (short)(Main.rand.Next(5) * 16);
			if (WorldGen.SolidTile(i, j + 1))
			{
				tile.frameY = 0;
			}
			else if (WorldGen.SolidTile(i - 1, j))
			{
				tile.frameY = 16;
			}
			else if (WorldGen.SolidTile(i, j - 1))
			{
				tile.frameY = 32;
			}
			else if (WorldGen.SolidTile(i + 1, j))
			{
				tile.frameY = 48;
			}
			else
			{
				WorldGen.KillTile(i, j, false, false, false);
			}
			return true;
		}

		public override bool CanPlace(int i, int j)
		{
			return ((WorldGen.SolidTile(i - 1, j)) || (WorldGen.SolidTile(i, j + 1)) || (WorldGen.SolidTile(i, j - 1)) || (WorldGen.SolidTile(i + 1, j)));
		}
	}
}