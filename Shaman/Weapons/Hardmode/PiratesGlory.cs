using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class PiratesGlory : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 12;
			Item.channel = true;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 6;
			Item.useAnimation = 30;
			Item.knockBack = 0.5f;
			Item.rare = 5;
			Item.value = Item.sellPrice(0, 3, 50, 0);
			Item.UseSound = SoundID.Item15;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			Item.shoot = Mod.Find<ModProjectile>("PiratesGloryProj").Type;
			this.empowermentType = 2;
			this.energy = 3;
			OrchidModGlobalItem orchidItem = Item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeaponNoUsetimeReforge = true;
		}

		public override void SafeModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			flat += (OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod) * 2f);
		}


		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Pirates Glory");
			Tooltip.SetDefault("Shoots a continuous laser beam"
							+ "\nWeapon damage increases with the number of active shamanic bonds"
							+ "\nHaving more than 3 of them active will make your foes drop more gold");
		}
	}
}
