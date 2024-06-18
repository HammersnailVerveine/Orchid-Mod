using Microsoft.Xna.Framework;
using OrchidMod.Content.Alchemist.Projectiles;
using OrchidMod.Common.Global.NPCs;
using System;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using OrchidMod.Common.ModObjects;
using OrchidMod.Common.Global.Items;


namespace OrchidMod.Content.Alchemist.Weapons.Nature
{
	public class AttractiteFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 10;
			Item.width = 30;
			Item.height = 30;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.NATURE;
			this.rightClickDust = 60;
			this.colorR = 155;
			this.colorG = 21;
			this.colorB = 18;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Attractite Flask");
			/* Tooltip.SetDefault("Hit target will attract alchemic spores and lingering particles"
							+ "\nThe attractivity buff will jump to the nearest target on miss"
							+ "\nCounts as an extract when spawning spores"); */
		}

		public override void KillSecond(int timeLeft, Player player, OrchidAlchemist modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidGlobalItemPerEntity globalItem)
		{
			if (alchProj.nbElements == 1)
			{
				OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 20, true, 1.5f, 1f, 3f);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 20, true, 1.5f, 1f, 3f, true, false);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 20, true, 1.5f, 1f, 3f, false, true);
			}
			else if (alchProj.nbElements == 2)
			{
				OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 30, true, 1.5f, 1f, 8f);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 30, true, 1.5f, 1f, 8f, true, false);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 30, true, 1.5f, 1f, 8f, false, true);
			}
			else if (alchProj.nbElements > 2)
			{
				OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 40, true, 1.5f, 1f, 13f);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 40, true, 1.5f, 1f, 13f, true, false);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 60, 20, 40, true, 1.5f, 1f, 13f, false, true);
			}
			if (!alchProj.hitNPC)
			{
				float baseRange = 50f;
				int usedElements = alchProj.nbElements > 3 ? 3 : alchProj.nbElements;
				float distance = 20f + usedElements * baseRange;
				NPC attractiteTarget = null;
				for (int k = 0; k < 200; k++)
				{
					if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly)
					{
						Vector2 newMove = Main.npc[k].Center - projectile.Center;
						float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
						if (distanceTo < distance)
						{
							distance = distanceTo;
							attractiteTarget = Main.npc[k];
						}
					}
				}
				if (attractiteTarget != null)
				{
					attractiteTarget.AddBuff(BuffType<Debuffs.Attraction>(), 60 * (alchProj.nbElements * 3));
				}
			}
		}

		public override void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidAlchemist modPlayer,
		OrchidModAlchemistNPC modTarget, OrchidGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidGlobalItemPerEntity globalItem)
		{
			target.AddBuff(BuffType<Debuffs.Attraction>(), 60 * (alchProj.nbElements * 3));
		}

		public override void AddVariousEffects(Player player, OrchidAlchemist modPlayer, AlchemistProj alchProj, Projectile proj, OrchidGlobalItemPerEntity globalItem)
		{
			alchProj.nbElementsNoExtract--;
		}
	}
}
