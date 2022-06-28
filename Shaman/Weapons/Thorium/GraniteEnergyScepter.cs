using OrchidMod.Common.Attributes;
using OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Circle;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium
{
	[CrossmodContent("ThoriumMod")]
	public class GraniteEnergyScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 35;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.knockBack = 3.25f;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shootSpeed = 10f;
			Item.shoot = ModContent.ProjectileType<GraniteEnergyScepterProj>();
			this.empowermentType = 4;
			this.catalystType = ShamanCatalystType.ROTATE;
			this.energy = 10;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Coalesced Conduit");
			Tooltip.SetDefault("Fires out a surge of energy"
							+ "\nGrants you a granite energy orb on hit"
							+ "\nIf you have 4 energy orbs, your next hit will make them revolve around you");
		}
	}
}

