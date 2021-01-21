using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Catalysts
{
	public class IronCatalyst : OrchidModAlchemistCatalyst
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Catalytic Syringe");
			Tooltip.SetDefault("Used to interact with alchemist catalytic elements");
		}
		
		public override void SafeSetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.rare = 0;
			item.value = Item.sellPrice(0, 0, 10, 0);
			this.catalystType = 1;
		}
		
		public override void CatalystInteractionEffect(Player player) {}
	}
}
