using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Tiles.Ambient
{
	public class FragileWood : ModTile
	{
		public override void SetDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			dustType = 7;
			AddMapEntry(new Color(151, 107, 75));
		}
		
		public override bool Dangersense(int i, int j, Player player) {
			return true;
		}
	}
}