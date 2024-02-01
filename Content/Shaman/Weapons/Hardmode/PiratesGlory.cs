using OrchidMod.Content.Shaman.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Hardmode
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
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(0, 3, 50, 0);
			Item.UseSound = SoundID.Item15;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			//Item.shoot = ModContent.ProjectileType<PiratesGloryProj>();
			this.Element = ShamanElement.WATER;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Pirates Glory");
			/* Tooltip.SetDefault("Shoots a continuous laser beam"
							+ "\nWeapon damage increases with the number of active shamanic bonds"
							+ "\nHaving more than 3 of them active will make your foes drop more gold"); */
		}

		public override void SafeModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			damage.Flat += (modPlayer.CountShamanicBonds() * 2);
		}
	}
}
