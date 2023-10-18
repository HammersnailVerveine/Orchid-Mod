using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Utilities
{
	public static partial class OrchidUtils
	{
		public static void AddItemToShop<T>(NPCShop shop, int chanceDenominator = 1) where T : ModItem
			=> AddItemToShop(shop, ModContent.ItemType<T>(), chanceDenominator);

		public static void AddItemToShop<T>(Item[] shop, int chanceDenominator = 1) where T : ModItem
			=> AddItemToShop(shop, ModContent.ItemType<T>(), chanceDenominator);

		public static void AddItemToShop(NPCShop shop, int type, int chanceDenominator = 1)
		{
			if (!Main.rand.NextBool(chanceDenominator)) return;
			shop.Add(type);
		}
		public static void AddItemToShop(Item[] shop, int type, int chanceDenominator = 1)
		{
			if (!Main.rand.NextBool(chanceDenominator)) return;
			
			foreach (Item item in shop)
			{
				if (item.IsAir)
				{
					item.SetDefaults(type);
					return;
				}
			}
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