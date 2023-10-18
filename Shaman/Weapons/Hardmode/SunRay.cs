using OrchidMod.Shaman.Projectiles;
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
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.UseSound = SoundID.Item15;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			//Item.shoot = ModContent.ProjectileType<SunRayProj>();
			this.Element = 1;
			this.energy = 2;
		}

		public override void SafeModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int nbBonds = modPlayer.GetNbShamanicBonds();
			damage += nbBonds * 0.1f;
			if (!Main.dayTime) damage.Flat -= 0.1f;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Sun Ray");
			/* Tooltip.SetDefault("Shoots a continuous sun beam"
							 + "\nDamage scales with the number of active shamanic bonds"
							 + "\n10% increased damage during the day"); */
		}
	}
}
