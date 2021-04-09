using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Water
{
	public class IceChestFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 12;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 1, 0, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.WATER;
			this.rightClickDust = 261;
			this.colorR = 19;
			this.colorG = 188;
			this.colorB = 236;
			this.secondaryDamage = 10;
			this.secondaryScaling = 5f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flash Freeze");
		    Tooltip.SetDefault("Freezes all spores, catalytic elements and lingering particles in an area"
							+ "\nFrozen projectiles will fall to the ground and shatter"
							+ "\nEnemies hit by the area will be slowed, duration is increased against water-coated ones"
							+ "\nUsing a fire ingredient cancels all these effects, and coats hit enemy with alchemical water");
		}
		
		public override void KillFirst(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem) {
			if (alchProj.fireFlask.type == 0) {
				int range = 135 * alchProj.nbElements;
				int nb = 20 * alchProj.nbElements;
				OrchidModProjectile.spawnDustCircle(projectile.Center, this.rightClickDust, (int)(range * 0.75), nb, true, 1.5f, 1f, 8f);
				OrchidModProjectile.spawnDustCircle(projectile.Center, this.rightClickDust, (int)(range * 0.5), (int)(nb / 3), true, 1.5f, 1f, 16f, true, true, false, 0, 0, true);
				
				int projType = ProjectileType<Alchemist.Projectiles.Water.IceChestFlaskProj>();
				int damage = getSecondaryDamage(modPlayer, alchProj.nbElements);
				int newProjectileInt = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, projType, damage, 0f, projectile.owner);
				Projectile newProjectile = Main.projectile[newProjectileInt];
				newProjectile.width = range * 2;
				newProjectile.height = range * 2;
				newProjectile.position.X = projectile.Center.X - (newProjectile.width / 2);
				newProjectile.position.Y = projectile.Center.Y - (newProjectile.width / 2);
				newProjectile.netUpdate = true;
			}
		}
		
		public override void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer, 
		OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem) {
			if (alchProj.fireFlask.type != 0) {
				modTarget.alchemistWater = 60 * 10;
			}
		}
	}
}
