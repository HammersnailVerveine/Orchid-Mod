using OrchidMod.Common.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Thorium
{
	public class ThunderScepter : OrchidModShamanItem, ICrossmodItem
	{
		public string CrossmodName => "Thorium Mod";

		public override void SafeSetDefaults()
		{
			Item.damage = 12;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.knockBack = 3.15f;
			Item.rare = 1;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.UseSound = SoundID.Item93;
			Item.autoReuse = true;
			Item.shootSpeed = 12f;
			Item.shoot = Mod.Find<ModProjectile>("ThunderScepterProj").Type;
			this.empowermentType = 3;
			this.energy = 3;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Thunder Scepter");
			Tooltip.SetDefault("Rapidly zaps your foes"
							+ "\nHitting will charge up energy above you"
							+ "\nWhen fully loaded, potent wind gusts will be released");
		}
	}
}
