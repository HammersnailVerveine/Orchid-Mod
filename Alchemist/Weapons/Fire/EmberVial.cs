using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Fire
{
	public class EmberVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 7;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.FIRE;
			this.rightClickDust = 6;
			this.colorR = 253;
			this.colorG = 62;
			this.colorB = 3;
			this.secondaryDamage = 8;
			this.secondaryScaling = 1f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ember Flask");
		    Tooltip.SetDefault("Burns your target briefly"
							+ "\nReleases lingering embers");
		}
		
		public override void KillSecond(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem) {
			int nb = 2 + Main.rand.Next(3);
			for (int i = 0 ; i < nb ; i ++) {
				Vector2 vel = (new Vector2(0f, -(float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(80)));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Fire.EmberVialProjAlt>();
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
			}
			int dmg = this.getSecondaryDamage(modPlayer, 0);
			int rand = alchProj.nbElements + Main.rand.Next(2);
			for (int i = 0 ; i < rand ; i ++) {
				Vector2 vel = (new Vector2(0f, -3f).RotatedByRandom(MathHelper.ToRadians(60)));
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Fire.EmberVialProj>(), dmg, 0f, projectile.owner);
			}
		}
		
		public override void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer, 
		OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem) {
			target.AddBuff(BuffID.OnFire, (60 * 3 ) + (60 * (alchProj.nbElements)));
		}
	}
}
