using OrchidMod.Interfaces;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Weapons.Thorium
{
	public class GraniteEnergyScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			item.damage = 35;
			item.width = 30;
			item.height = 30;
			item.useTime = 45;
			item.useAnimation = 45;
			item.knockBack = 3.25f;
			item.rare = ItemRarityID.Orange;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.UseSound = SoundID.Item43;
			item.autoReuse = true;
			item.shootSpeed = 10f;
			item.shoot = mod.ProjectileType("GraniteEnergyScepterProj");
			this.empowermentType = 4;
			this.energy = 10;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coalesced Conduit");
			Tooltip.SetDefault("Fires out a surge of energy"
							+ "\nGrants you a granite energy orb on hit"
							+ "\nIf you have 4 energy orbs, your next hit will make them revolve around you");
		}
	}
}

