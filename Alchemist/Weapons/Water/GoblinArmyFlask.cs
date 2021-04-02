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
	public class GoblinArmyFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 10;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 30, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.WATER;
			this.rightClickDust = 184;
			this.colorR = 22;
			this.colorG = 22;
			this.colorB = 22;
			this.secondaryDamage = 50;
			this.secondaryScaling = 15f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goblin Oil");
		    Tooltip.SetDefault("Using a fire element in the same attack will drastically increase damage"
							+  "\nThis will also damage and spread alchemical fire to all nearby water coated enemies"
							+  "\nHas a chance to release a catalytic oil bubble, coating nearby enemies in water on reaction");
		}
		
		public override void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer, 
		OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem) {
			if (alchProj.fireFlask.type != 0) {
				int dmg = getSecondaryDamage(modPlayer, alchProj.nbElements);
				modTarget.spreadOilFire(target.Center, dmg, Main.player[projectile.owner]);
				Main.PlaySound(2, (int)target.position.X, (int)target.position.Y, 45);
			}
				
			int rand = alchProj.nbElements;
			rand += alchProj.hasCloud() ? 2 : 0;
			if (Main.rand.Next(6) < rand && !alchProj.noCatalyticSpawn) {
				int dmg = 0;
				int proj = ProjectileType<Alchemist.Projectiles.Reactive.OilBubble>();
				Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, proj, dmg, 0f, projectile.owner);
			}
		}
		
		public override void AddVariousEffects(Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem) {
			if (alchProj.fireFlask.type != 0) {
				projectile.damage += (int)(30 * modPlayer.alchemistDamage);
			}
		}
	}
}
