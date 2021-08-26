using Terraria;

namespace OrchidMod.Shaman.Accessories
{
	public class FragilePresent : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Item.sellPrice(0, 4, 0, 0);
			item.rare = 8;
			item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fragile Present");
			Tooltip.SetDefault("25% increased shamanic damage"
							+ "\nTaking direct damage will nullify the damage increase for 15 seconds");
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (!(Main.LocalPlayer.FindBuffIndex(mod.BuffType("BrokenPower")) > -1))
			{
				modPlayer.shamanDamage += 0.25f;
			}
			modPlayer.shamanSunBelt = true;
		}
	}
}