using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Weapons
{
	public class ShadowWeaver : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 30;
			item.width = 36;
			item.height = 38;
			item.useTime = 45;
			item.useAnimation = 45;
			item.knockBack = 5.5f;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 60, 0);
			item.UseSound = SoundID.Item45;
			item.autoReuse = true;
			item.shootSpeed = 9.5f;
			item.shoot = mod.ProjectileType("DemoniteScepterProj");
			this.empowermentType = 2;
			this.energy = 5;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Weaver");
			Tooltip.SetDefault("\nHitting an enemy will grant you a shadow orb"
							  + "\nIf you have 3 shadow orbs, your next hit will empower you with dark energy for 30 seconds");
		}
	}
}
