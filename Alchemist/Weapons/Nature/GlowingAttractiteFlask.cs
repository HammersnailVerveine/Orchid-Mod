using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using OrchidMod.Common.Globals.NPCs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Nature
{
	public class GlowingAttractiteFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 15;
			Item.width = 30;
			Item.height = 30;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 0, 35, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.NATURE;
			this.rightClickDust = 15;
			this.colorR = 5;
			this.colorG = 149;
			this.colorB = 235;
			this.secondaryDamage = 22;
		}

		public override void AltSetStaticDefaults()
		{
			DisplayName.SetDefault("Glowing Attractite Flask");
			Tooltip.SetDefault("Hit target will attract alchemic spores and lingering particles"
							+ "\nThe attractivity buff will jump to the nearest target on miss"
							+ "\nReleases nature spores, the less other extracts used, the more"
							+ "\nSpores deals 10% increased damage against fire-coated enemies");
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(null, "MoonglowFlask", 1);
			recipe.AddIngredient(null, "AttractiteFlask", 1);
			recipe.AddIngredient(null, "AlchemicStabilizer", 1);
			recipe.Register();
		}

		public override void KillSecond(int timeLeft, Player player, OrchidAlchemist modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
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
			int nb = 2 + Main.rand.Next(2);
			for (int i = 0; i < nb; i++)
			{
				Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProjAlt>();
				SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, spawnProj, 0, 0f, projectile.owner);
			}
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active == true && proj.type == ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>() && proj.owner == projectile.owner && proj.localAI[1] != 1f)
				{
					proj.Kill();
				}
			}
			nb = alchProj.nbElements + alchProj.nbElementsNoExtract;
			nb += player.HasBuff(BuffType<Alchemist.Buffs.MushroomHeal>()) ? Main.rand.Next(3) : 0;
			for (int i = 0; i < nb; i++)
			{
				Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
				int dmg = GetSecondaryDamage(player, alchProj.nbElements);
				SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, Mod.Find<ModProjectile>("NatureSporeProj").Type, dmg, 0f, projectile.owner);
			}
			if (alchProj.nbElements == 1)
			{
				OrchidModProjectile.spawnDustCircle(projectile.Center, 15, 20, 20, true, 1.5f, 1f, 3f);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 15, 20, 20, true, 1.5f, 1f, 3f, true, false);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 15, 20, 20, true, 1.5f, 1f, 3f, false, true);
			}
			else if (alchProj.nbElements == 2)
			{
				OrchidModProjectile.spawnDustCircle(projectile.Center, 15, 20, 30, true, 1.5f, 1f, 8f);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 15, 20, 30, true, 1.5f, 1f, 8f, true, false);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 15, 20, 30, true, 1.5f, 1f, 8f, false, true);
			}
			else if (alchProj.nbElements > 2)
			{
				OrchidModProjectile.spawnDustCircle(projectile.Center, 15, 20, 40, true, 1.5f, 1f, 13f);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 15, 20, 40, true, 1.5f, 1f, 13f, true, false);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 15, 20, 40, true, 1.5f, 1f, 13f, false, true);
			}
		}

		public override void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidAlchemist modPlayer,
		OrchidModAlchemistNPC modTarget, OrchidGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			target.AddBuff(BuffType<Debuffs.Attraction>(), 60 * (alchProj.nbElements * 3));
		}

		public override void AddVariousEffects(Player player, OrchidAlchemist modPlayer, AlchemistProj alchProj, Projectile proj, OrchidModGlobalItem globalItem)
		{
			alchProj.nbElementsNoExtract--;
		}
	}
}
