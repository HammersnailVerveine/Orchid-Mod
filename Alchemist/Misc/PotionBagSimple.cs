using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Misc
{
	public class PotionBagSimple : OrchidModItem
	{
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 28;
			Item.maxStack = 1;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = ItemRarityID.Blue;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Potion Bag");
			Tooltip.SetDefault("Can carry up to 16 alchemical weapons"
							+  "\nRight click to fill with alchemist items in inventory order"
							+  "\nRight click when full or without alchemist items in inventory to empty"
							+  "\nItems in the bag will appear in alchemist mixing interfaces"
							+  "\nDropping the bag will release its content"
							+  "\nOnly one bag can be used at once"
							+  "\n'A prototype by the chemist, meant to be upgraded later'");
		}

		public override bool CanRightClick() {
			return true;
		}
		
		public override bool ConsumeItem(Player player) {
			return false;
		}
		
		public override void UpdateInventory(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistHasBag = 5;
		}
		
		public override void HoldItem(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistHasBag = 5;
		}
		
		public override void RightClick(Player player) {
			//player.QuickSpawnItem(ItemID.LifeCrystal, 1);
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			Item[] potionBag = modPlayer.alchemistPotionBag;
			bool addedItem = false;
			int index = 0;
			int firstSlot = 0;
			
			foreach (Item item in potionBag) {
				if (item.type != 0) {
					firstSlot ++;
				} else {
					break;
				}
			}
			
			
			while (modPlayer.alchemistPotionBag[potionBag.Length - 1].type == 0 && index < Main.InventorySlotsTotal) {
				Item item = Main.LocalPlayer.inventory[index];
				index ++;
				if (item.type != 0)
				{
					OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
					if (orchidItem.alchemistElement != AlchemistElement.NULL)
					{
						potionBag[firstSlot] = new Item();
						potionBag[firstSlot].SetDefaults(item.type, true);
						item.TurnToAir();
						addedItem = true;
						firstSlot ++;
					}
				}
			}
			
			if (!addedItem) {
				foreach (Item item in potionBag) {
					if (item.type != 0)
					{
						player.QuickSpawnItem(player.GetSource_OpenItem(item.type), item.type, 1);
						item.TurnToAir();
					}
				}
			}
		}
	}
}
