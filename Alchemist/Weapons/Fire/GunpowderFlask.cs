using OrchidMod.Alchemist.Projectiles;
using Terraria;
using static Terraria.ModLoader.ModContent;


namespace OrchidMod.Alchemist.Weapons.Fire
{
	public class GunpowderFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 15;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 40, 0);
			this.potencyCost = 4;
			this.element = AlchemistElement.FIRE;
			this.rightClickDust = 37;
			this.colorR = 66;
			this.colorG = 66;
			this.colorB = 66;
			this.secondaryDamage = 10;
			this.secondaryScaling = 15f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gunpowder Flask");
			Tooltip.SetDefault("Creates a small explosion"
							+ "\nCan be used to trigger catalytic elements, but prevents them from spawning");
		}

		public override void KillSecond(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int dmg = getSecondaryDamage(modPlayer, alchProj.nbElements);
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, ProjectileType<Alchemist.Projectiles.Fire.GunpowderFlaskProj>(), dmg, 3f, projectile.owner, 0.0f, 0.0f);
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
		}
	}
}
