using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class ShadowWeaver : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 30;
			Item.width = 36;
			Item.height = 38;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.knockBack = 5.5f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.UseSound = SoundID.Item45;
			Item.autoReuse = true;
			Item.shootSpeed = 9.5f;
			//Item.shoot = ModContent.ProjectileType<DemoniteScepterProj>();
			this.Element = 2;
			this.energy = 5;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Shadow Weaver");
			/* Tooltip.SetDefault("\nHitting an enemy will grant you a shadow orb"
							  + "\nIf you have 3 shadow orbs, your next hit will empower you with dark energy for 30 seconds"); */
		}
	}
}
