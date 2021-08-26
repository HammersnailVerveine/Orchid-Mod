using Terraria;
using Terraria.ID;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class PiratesGlory : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 12;
			item.channel = true;
			item.width = 30;
			item.height = 30;
			item.useTime = 6;
			item.useAnimation = 30;
			item.knockBack = 0.5f;
			item.rare = 5;
			item.value = Item.sellPrice(0, 3, 50, 0);
			item.UseSound = SoundID.Item15;
			item.autoReuse = true;
			item.shootSpeed = 15f;
			item.shoot = mod.ProjectileType("PiratesGloryProj");
			this.empowermentType = 2;
			this.energy = 3;
			OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeaponNoUsetimeReforge = true;
		}

		public override void SafeModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			flat += (OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, mod) * 2f);
		}


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pirates Glory");
			Tooltip.SetDefault("Shoots a continuous laser beam"
							+ "\nWeapon damage increases with the number of active shamanic bonds"
							+ "\nHaving more than 3 of them active will make your foes drop more gold");
		}
	}
}
