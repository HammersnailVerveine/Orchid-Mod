using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class SunRay : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 60;
			Item.channel = true;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 6;
			Item.useAnimation = 30;
			Item.knockBack = 4.15f;
			Item.rare = 8;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.UseSound = SoundID.Item15;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			Item.shoot = Mod.Find<ModProjectile>("SunRayProj").Type;
			this.empowermentType = 1;
			this.energy = 2;
		}

		public override void SafeModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod);
			mult *= modPlayer.shamanDamage + (nbBonds * 0.1f);
			if (!Main.dayTime) add -= 0.1f;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Sun Ray");
			Tooltip.SetDefault("Shoots a continuous sun beam"
							 + "\nDamage scales with the number of active shamanic bonds"
							 + "\n10% increased damage during the day");
		}
	}
}
