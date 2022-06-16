using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	public class FragilePresent : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
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
			if (!(Main.LocalPlayer.FindBuffIndex(Mod.Find<ModBuff>("BrokenPower").Type) > -1))
			{
				modPlayer.shamanDamage += 0.25f;
			}
			modPlayer.shamanSunBelt = true;
		}
	}
}