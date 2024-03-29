using OrchidMod.Content.Shaman.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Hardmode
{
	public class RuneScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 43;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.knockBack = 3f;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(0, 3, 10, 0);
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shootSpeed = 13f;
			//Item.shoot = ModContent.ProjectileType<RuneScepterProj>();
			this.Element = ShamanElement.FIRE;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("Rune Scepter");
			/* Tooltip.SetDefault("Shoots short ranged rune bolts"
							  + "\nProjectile range and damage scales with the number of active shamanic bonds"); */
		}

		public override void SafeModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int nbBonds = modPlayer.CountShamanicBonds();
			damage += nbBonds * 0.1f;
		}

		public override void UpdateInventory(Player player)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int nbBonds = modPlayer.CountShamanicBonds();
			Item.shootSpeed = 13f + (2f * nbBonds);
		}
	}
}
