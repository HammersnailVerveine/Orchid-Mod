using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Utilities
{
	public static partial class OrchidUtils
	{
		public static void AddItemToShop<T>(Chest shop, ref int nextSlot, int chanceDenominator = 1) where T : ModItem
			=> AddItemToShop(shop, ref nextSlot, ModContent.ItemType<T>(), chanceDenominator);

		public static void AddItemToShop(Chest shop, ref int nextSlot, int type, int chanceDenominator = 1)
		{
			if (!Main.rand.NextBool(chanceDenominator)) return;

			shop.item[nextSlot].SetDefaults(type);
			nextSlot++;
		}

		public static void AddItemToShop<T>(int[] shop, ref int nextSlot, int chanceDenominator = 1) where T : ModItem
			=> AddItemToShop(shop, ref nextSlot, ModContent.ItemType<T>(), chanceDenominator);

		public static void AddItemToShop(int[] shop, ref int nextSlot, int type, int chanceDenominator = 1)
		{
			if (!Main.rand.NextBool(chanceDenominator)) return;

			shop[nextSlot] = type;
			nextSlot++;
		}
	}
}