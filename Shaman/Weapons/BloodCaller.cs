using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Weapons
{
	public class BloodCaller : OrchidModShamanItem
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
			item.shoot = mod.ProjectileType("CrimsonScepterProj");
			this.empowermentType = 1;
			this.energy = 7;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Caller");
			Tooltip.SetDefault("\nHitting an enemy will grant you a crimson heart"
							  + "\nIf you have 3 crimson hearts, your next hit will recover some life");
		}
	}
}
