using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;


namespace OrchidMod.Alchemist.Weapons.Nature
{
	public class AttractiteFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 10;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 20, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.NATURE;
			this.rightClickDust = 60;
			this.colorR = 155;
			this.colorG = 21;
			this.colorB = 18;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Attractite Flask");
		    Tooltip.SetDefault("Hit target will attract most nearby alchemical lingering projectiles"
							+  "\nThe attractivity buff will jump to the nearest target on miss"
							+  "\nCounts as an extract when spawning spores");
		}
		
		public override void KillSecond(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem) {
			if (alchProj.nbElements == 1) {
				OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 20, true, 1.5f, 1f, 3f);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 20, true, 1.5f, 1f, 3f, true, false);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 20, true, 1.5f, 1f, 3f, false, true);
			} else if (alchProj.nbElements == 2) {
				OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 30, true, 1.5f, 1f, 8f);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 30, true, 1.5f, 1f, 8f, true, false);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 30, true, 1.5f, 1f, 8f, false, true);
			} else if (alchProj.nbElements > 2) {
				OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 40, true, 1.5f, 1f, 13f);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 40, true, 1.5f, 1f, 13f, true, false);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 40, true, 1.5f, 1f, 13f, false, true);
			}
			if (!alchProj.hitNPC) {
				float baseRange = 50f;
				int usedElements = alchProj.nbElements > 3 ? 3 : alchProj.nbElements;
				float distance = 20f + usedElements * baseRange;
				NPC attractiteTarget = null;
				for (int k = 0; k < 200; k++)
				{
					if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly){
						Vector2 newMove = Main.npc[k].Center - projectile.Center;
						float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
						if (distanceTo < distance) {
							distance = distanceTo;
							attractiteTarget = Main.npc[k];
						}
					}
				}
				if (attractiteTarget != null) {
					attractiteTarget.AddBuff(BuffType<Alchemist.Buffs.Debuffs.Attraction>(), 60 * (alchProj.nbElements * 3));
				}
			}
		}
		
		public override void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer, 
		OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem) {
			target.AddBuff(BuffType<Alchemist.Buffs.Debuffs.Attraction>(), 60 * (alchProj.nbElements * 3));
		}
	}
}
