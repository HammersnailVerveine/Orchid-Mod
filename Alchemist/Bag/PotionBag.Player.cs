using OrchidMod.Alchemist.UI;
using OrchidMod.Common.UIs;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Bag
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
			=> GetPotionsFromBags().Concat(GetPotionsFromInventory());

		// Private methods

		private IEnumerable<Item> GetPotionsFromBags()
		{
			var potions = new List<Item>();

			foreach (var bag in potionBags)
			{
				potions.AddRange(bag.GetPotions.Select(item => item.Clone()));
			}

			return potions;
		}

		private IEnumerable<Item> GetPotionsFromInventory()
			=> Player.inventory.Where(i => i.TryGetGlobalItem(out OrchidModGlobalItem global) && global.alchemistElement != AlchemistElement.NULL).Select(item => item.Clone());
	}
}