using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;


namespace OrchidMod.Alchemist.Weapons.Air
{
	public class CorruptionFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 12;
			item.width = 28;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 1, 0, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.AIR;
			this.rightClickDust = 16;
			this.colorR = 121;
			this.colorG = 152;
			this.colorB = 239;
			this.secondaryDamage = 30;
			this.secondaryScaling = 30f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vitriol Mycelium");
		    Tooltip.SetDefault("e");
		}
		
		public override void KillSecond(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem) {
			// int nb = 2 + Main.rand.Next(2);
			// for (int i = 0 ; i < nb ; i ++) {
				// Vector2	vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
				// int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.GlowingMushroomVialProjAlt2>();
				// Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
			// }
			
			int dmg = getSecondaryDamage(modPlayer, alchProj.nbElements);
			int projType = ProjectileType<Alchemist.Projectiles.Air.CorruptionFlaskProj>();
			Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, -0.1f, projType, dmg, 0f, projectile.owner);
		}
	}
}
