using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Air
{
	public class ShadowChestFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 25;
			item.width = 30;
			item.height = 30;
			item.rare = 3;
			item.value = Item.sellPrice(0, 2, 50, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.AIR;
			this.rightClickDust = 21;
			this.colorR = 139;
			this.colorG = 42;
			this.colorB = 156;
			this.secondaryDamage = 10;
			this.secondaryScaling = 20f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Demon Breath");
			Tooltip.SetDefault("Releases returning demon flames"
							+ "\nCoats hit enemies in alchemical air");
		}

		public override void KillSecond(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int dmg = getSecondaryDamage(modPlayer, alchProj.nbElements);
			int spawnProj = ProjectileType<Alchemist.Projectiles.Air.ShadowChestFlaskProj>();
			for (int i = 0; i < 4; i++)
			{
				Vector2 vel = (new Vector2(0f, 5f * alchProj.nbElements).RotatedBy(MathHelper.ToRadians(90 * i)));
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, projectile.owner);
			}
		}
	}
}
