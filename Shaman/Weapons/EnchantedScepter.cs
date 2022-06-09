using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons
{
	public class EnchantedScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 9;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.knockBack = 3.15f;
			Item.rare = 1;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shootSpeed = 9f;
			Item.shoot = Mod.Find<ModProjectile>("EnchantedScepterProj").Type;
			this.empowermentType = 1;
			this.energy = 7;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Scepter");
			Tooltip.SetDefault("Weapon damage increases with the number of active shamanic bonds");
		}

		public override void SafeModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			flat += (OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod) * 3f);
		}
	}
}
