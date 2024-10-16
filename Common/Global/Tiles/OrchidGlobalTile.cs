using OrchidMod.Content.Alchemist.Weapons.Catalysts;
using OrchidMod.Content.Gambler.Weapons.Cards;
using OrchidMod.Content.Shaman.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Common.Global.Tiles
{
	public class OrchidGlobalTile : GlobalTile
	{
		public override void Drop(int i, int j, int type)/* tModPorter Suggestion: Use CanDrop to decide if items can drop, use this method to drop additional items. See documentation. */
		{
			var tile = Main.tile[i, j];

			switch (type)
			{
				/*
				case TileID.Pots:
					DropTileBreakItem<HealingPotionCard>(i, j, 1, 1500);
					break;
					*/
				case TileID.ShadowOrbs:
					if (tile.TileFrameY == 0 && tile.TileFrameX % 36 == 0)
					{
						bool isShadowOrb = tile.TileFrameX == 0;

						//DropTileBreakItem(isShadowOrb ? ModContent.ItemType<ShadowWeaver>() : ModContent.ItemType<BloodCaller>(), i, j, 1, 4);
						//DropTileBreakItem(isShadowOrb ? ModContent.ItemType<DemoniteCatalyst>() : ModContent.ItemType<CrimtaneCatalyst>(), i, j, 1, 4);
					}
					break;
			}

			//return base.Drop(i, j, type);
		}

		// ...

		private static void DropTileBreakItem<T>(int i, int j, int stack = 1, int chanceDenominator = 1) where T : ModItem
			=> DropTileBreakItem(ModContent.ItemType<T>(), i, j, stack, chanceDenominator);

		private static void DropTileBreakItem(int type, int i, int j, int stack = 1, int chanceDenominator = 1)
		{
			if (!Main.rand.NextBool(chanceDenominator)) return;

			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, type, stack);
		}
	}
}