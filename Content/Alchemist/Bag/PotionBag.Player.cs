using OrchidMod.Content.Alchemist.UI;
using OrchidMod.Common.UIs;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Alchemist.Bag
{
	public class PotionBagPlayer : ModPlayer
	{
		// Fields

		private IList<PotionBag> potionBags;

		// Overrides

		public override void Initialize()
			=> potionBags = new List<PotionBag>();

		public override void PreUpdate()
			=> potionBags.Clear();

		public override void PostBuyItem(NPC vendor, Item[] shopInventory, Item item)
		{
			if (item.ModItem is PotionBag bag)
			{
				bag.InShop = false;
			}
		}

		// Public methods

		public void AddBagToList(PotionBag bag)
		{
			if (bag.IsActive)
			{
				potionBags.Add(bag);
			}
		}

		// Returns copies intentionally, because it is not known what will happen in the current interface
		public IEnumerable<Item> GetPotionsFromInventoryAndBags()
		{
			// Remove duplicates, but not with different prefixes

			var potions = new List<Item>();
			var hashSet = new HashSet<string>();

			foreach (var item in Player.inventory)
			{
				var name = item.AffixName();

				if (hashSet.Contains(name)
				 || !item.TryGetGlobalItem(out OrchidModGlobalItem global)
				 || global.alchemistElement == AlchemistElement.NULL) continue;

				hashSet.Add(name);
				potions.Add(item.Clone());
			}

			foreach (var bag in potionBags)
			{
				var inventory = bag.GetPotions;

				foreach (var item in inventory)
				{
					var name = item.AffixName();

					if (hashSet.Contains(name)) continue;

					hashSet.Add(name);
					potions.Add(item.Clone());
				}
			}

			return potions;
		}
	}
}