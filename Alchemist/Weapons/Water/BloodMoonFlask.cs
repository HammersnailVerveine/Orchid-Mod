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
	public class BloodMoonFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 15;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 50, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.WATER;
			this.rightClickDust = 5;
			this.colorR = 349;
			this.colorG = 98;
			this.colorB = 64;
			this.secondaryDamage = 0;
			this.secondaryScaling = 4f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Mist Flask");
		    Tooltip.SetDefault("Creates a lingering cloud of damaging mist"
							+  "\nThe mist knockback heavily scales with the number of elements used"
							+  "\nUsing a fire element increases damage dealt, air increases spread"
							+  "\nUsing both negates both effects");
		}
		
		public override void KillSecond(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem) {
			int dmg = getSecondaryDamage(modPlayer, alchProj.nbElements) + ((alchProj.airFlask.type == 0 && alchProj.fireFlask.type != 0) ? 6 : 0);
			int rand =  2 + alchProj.nbElements + Main.rand.Next(2);
			float kb = 0.5f * alchProj.nbElements;
			for (int i = 0 ; i < rand ; i ++) {
				Vector2 vel = (new Vector2(0f, ((alchProj.airFlask.type != 0 && alchProj.fireFlask.type == 0) ? -4f : -2f)).RotatedByRandom(MathHelper.ToRadians(180)));
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Water.BloodMoonFlaskProj>(), dmg, kb, projectile.owner);
			}
		}
	}
}
