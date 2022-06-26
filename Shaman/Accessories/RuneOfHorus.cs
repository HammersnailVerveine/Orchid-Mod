using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	[AutoloadEquip(EquipType.Waist)]
	public class RuneOfHorus : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rune of Horus");
			Tooltip.SetDefault("Increases the shaman catalyst range"
							+ "\nIncreases shamanic damage by 5 when the catalyst is near maximum range");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			modPlayer.shamanHorus = true;

			int catalystType = ModContent.ProjectileType<CatalystAnchor>();
			if (player.ownedProjectileCounts[catalystType] != 0)
			{
				Vector2? catalystPosition = modPlayer.ShamanCatalystPosition;
				if (catalystPosition != null)
				{
					if (catalystPosition.Value.Distance(player.Center) > 150f)
						player.GetDamage<ShamanDamageClass>().Flat += 5;
				}
			}
		}
	}
}
