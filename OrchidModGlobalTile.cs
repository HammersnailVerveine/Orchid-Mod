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
					Item.NewItem(i * 16, j * 16, 32, 32, Mod.Find<ModItem>("HealingPotionCard").Type);
				}
			}
			if (type == TileID.ShadowOrbs && Main.tile[i, j].TileFrameY == 0 && Main.tile[i, j].TileFrameX % 36 == 0)
			{
				if (Main.rand.Next(6) == 0)
				{
					Item.NewItem(i * 16, j * 16, 32, 32, (Main.tile[i, j].TileFrameX == 0) ? Mod.Find<ModItem>("ShadowWeaver").Type : Mod.Find<ModItem>("BloodCaller").Type);
				}
				if (Main.rand.Next(5) == 0)
				{
					Item.NewItem(i * 16, j * 16, 32, 32, (Main.tile[i, j].TileFrameX == 0) ? Mod.Find<ModItem>("DemoniteCatalyst").Type : Mod.Find<ModItem>("CrimtaneCatalyst").Type);
				}
			}
			return base.Drop(i, j, type);
		}
	}
}