using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod
{
	public class ModGlobalTile : GlobalTile
	{
		public override bool Drop(int i, int j, int type)
		{
			if (type == TileID.Pots)
			{
				if (Main.rand.Next(1500) == 0)
				{
					Item.NewItem(i * 16, j * 16, 32, 32, mod.ItemType("HealingPotionCard"));
				}
			}
			if (type == TileID.ShadowOrbs && Main.tile[i, j].frameY == 0 && Main.tile[i, j].frameX % 36 == 0)
			{
				if (Main.rand.Next(6) == 0)
				{
					Item.NewItem(i * 16, j * 16, 32, 32, (Main.tile[i, j].frameX == 0) ? mod.ItemType("ShadowWeaver") : mod.ItemType("BloodCaller"));
				}
				if (Main.rand.Next(5) == 0)
				{
					Item.NewItem(i * 16, j * 16, 32, 32, (Main.tile[i, j].frameX == 0) ? mod.ItemType("DemoniteCatalyst") : mod.ItemType("CrimtaneCatalyst"));
				}
			}
			return base.Drop(i, j, type);
		}
	}
}