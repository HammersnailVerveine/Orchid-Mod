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
	public class TsunamiInAVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 15;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 1, 0, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.AIR;
			this.rightClickDust = 33;
			this.colorR = 48;
			this.colorG = 68;
			this.colorB = 152;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tsunami in a Flask");
		    Tooltip.SetDefault("Launches hit enemy in the air"
							+ "\nIncreases the likelihood of spawning catalytic bubbles");
		}
		
		public override void KillSecond(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem) {
			int nb = 2 + Main.rand.Next(3);
			for (int i = 0 ; i < nb ; i ++) {
				Vector2 vel = (new Vector2(0f, -(float)((3 * alchProj.nbElements) + Main.rand.Next(3))).RotatedByRandom(MathHelper.ToRadians(10)));
				int spawnProj = Main.rand.Next(3) == 0 ? ProjectileType<Alchemist.Projectiles.AlchemistSmoke3>() : ProjectileType<Alchemist.Projectiles.AlchemistSmoke2>();
				int smokeProj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
				Main.projectile[smokeProj].localAI[0] = alchProj.glowColor.R;
				Main.projectile[smokeProj].localAI[1] = alchProj.glowColor.G;
				Main.projectile[smokeProj].ai[1] = alchProj.glowColor.B;
			}
			
			if (alchProj.waterFlask.type == ItemType<Alchemist.Weapons.Water.BloodMoonFlask>()) {
				for (int i = 0 ; i < nb ; i ++) {
					int dmg = getSecondaryDamage(modPlayer, alchProj.nbElements);
					Vector2 vel = (new Vector2(0f, -(float)((3 * alchProj.nbElements) + Main.rand.Next(3))).RotatedByRandom(MathHelper.ToRadians(10)));
					int spawnProj = ProjectileType<Alchemist.Projectiles.Water.BloodMoonFlaskProj>();
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, projectile.owner);
				}
			}
			
			int type = ProjectileType<Alchemist.Projectiles.Air.CloudInAVialProj>();
			int newProj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, type, 0, 0.5f, projectile.owner);
			Main.projectile[newProj].ai[1] = alchProj.nbElements;
			Main.projectile[newProj].netUpdate = true;
		}
	}
}
