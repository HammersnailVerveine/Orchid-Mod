using OrchidMod.Alchemist.Projectiles;
using OrchidMod.Alchemist.Projectiles.Air;
using OrchidMod.Alchemist.Projectiles.Fire;
using OrchidMod.Alchemist.Projectiles.Nature;
using OrchidMod.Alchemist.Projectiles.Reactive;
using OrchidMod.Alchemist.Projectiles.Reactive.ReactiveSpawn;
using OrchidMod.Alchemist.Projectiles.Water;
using System;
using System.Collections.Generic;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Water
{
	public class IceChestFlask : OrchidModAlchemistItem
	{
		public static List<int> smallProjectiles = setSmallProjectiles();
		public static List<int> bigProjectiles = setBigProjectiles();

		public static List<int> setSmallProjectiles()
		{
			List<int> smallProjectiles = new List<int>();
			smallProjectiles.Add(ProjectileType<AlchemistSlime>());
			smallProjectiles.Add(ProjectileType<BloomingPetal>());
			smallProjectiles.Add(ProjectileType<AirSporeProj>());
			smallProjectiles.Add(ProjectileType<CrimsonFlaskProj>());
			smallProjectiles.Add(ProjectileType<SunplateFlaskProj>());
			smallProjectiles.Add(ProjectileType<EmberVialProj>());
			smallProjectiles.Add(ProjectileType<FireSporeProj>());
			smallProjectiles.Add(ProjectileType<LivingSapVialProj>());
			smallProjectiles.Add(ProjectileType<NatureSporeProj>());
			smallProjectiles.Add(ProjectileType<PoisonVialProj>());
			smallProjectiles.Add(ProjectileType<DungeonFlaskProj>());
			smallProjectiles.Add(ProjectileType<SeafoamVialProj>());
			smallProjectiles.Add(ProjectileType<WaterSporeProj>());
			return smallProjectiles;
		}

		public static List<int> setBigProjectiles()
		{
			List<int> smallProjectiles = new List<int>();
			smallProjectiles.Add(ProjectileType<LivingSapBubble>());
			smallProjectiles.Add(ProjectileType<OilBubble>());
			smallProjectiles.Add(ProjectileType<PoisonBubble>());
			smallProjectiles.Add(ProjectileType<SeafoamBubble>());
			smallProjectiles.Add(ProjectileType<SlimeBubble>());
			smallProjectiles.Add(ProjectileType<SpiritedBubble>());
			smallProjectiles.Add(ProjectileType<AlchemistHive>());
			smallProjectiles.Add(ProjectileType<BloomingReactiveAlt>());
			smallProjectiles.Add(ProjectileType<CorruptionFlaskProj>());
			smallProjectiles.Add(ProjectileType<SunflowerFlaskProj3>());
			return smallProjectiles;
		}

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

		public override void KillFirst(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			if (alchProj.fireFlask.type == 0)
			{
				int range = 135 * alchProj.nbElements;
				int nb = 20 * alchProj.nbElements;
				OrchidModProjectile.spawnDustCircle(projectile.Center, this.rightClickDust, (int)(range * 0.75), nb, true, 1.5f, 1f, 8f);
				OrchidModProjectile.spawnDustCircle(projectile.Center, this.rightClickDust, (int)(range * 0.5), (int)(nb / 3), true, 1.5f, 1f, 16f, true, true, false, 0, 0, true);

				int damage = getSecondaryDamage(modPlayer, alchProj.nbElements);
				/*
				int projType = ProjectileType<Alchemist.Projectiles.Water.IceChestFlaskProj>();
				int newProjectileInt = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, projType, damage, 0f, projectile.owner);
				Projectile newProjectile = Main.projectile[newProjectileInt];
				newProjectile.width = range * 2;
				newProjectile.height = range * 2;
				newProjectile.position.X = projectile.Center.X - (newProjectile.width / 2);
				newProjectile.position.Y = projectile.Center.Y - (newProjectile.width / 2);
				newProjectile.netUpdate = true;
				*/

				for (int i = 0; i < 20; i++)
				{
					int dust = Dust.NewDust(projectile.position, range * 2, range * 2, 261);
					Main.dust[dust].scale = 1.2f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity.X /= 3;
					Main.dust[dust].velocity.Y /= 3;
				}

				for (int l = 0; l < Main.npc.Length; l++)
				{
					NPC target = Main.npc[l];
					float offsetX = target.Center.X - projectile.Center.X;
					float offsetY = target.Center.Y - projectile.Center.Y;
					float distance = (float)Math.Sqrt(offsetX * offsetX + offsetY * offsetY);
					if ((range + (target.width / 2)) > distance)
					{
						OrchidModAlchemistNPC modTarget = target.GetGlobalNPC<OrchidModAlchemistNPC>();
						target.AddBuff(BuffType<Alchemist.Buffs.Debuffs.FlashFreeze>(), modTarget.alchemistWater > 0 ? 60 * 30 : 60 * 3);
					}
				}

				for (int l = 0; l < Main.projectile.Length; l++)
				{
					Projectile proj = Main.projectile[l];
					float offsetX = proj.Center.X - projectile.Center.X;
					float offsetY = proj.Center.Y - projectile.Center.Y;
					float distance = (float)Math.Sqrt(offsetX * offsetX + offsetY * offsetY);
					if (player.whoAmI == proj.owner && proj.active && (range + (proj.width / 2)) > distance)
					{
						if (smallProjectiles.Contains(proj.type))
						{
							int projType = ProjectileType<IceChestFlaskProjSmall>();
							Projectile.NewProjectile(proj.Center.X, proj.Center.Y, 0f, 1f, projType, damage, 1f, player.whoAmI);
							proj.active = false;
							proj.netUpdate = true;
						}

						if (bigProjectiles.Contains(proj.type))
						{
							int projType = ProjectileType<IceChestFlaskProjBig>();
							Projectile.NewProjectile(proj.Center.X, proj.Center.Y, 0f, 1f, projType, damage * 5, 5f, player.whoAmI);
							proj.active = false;
							proj.netUpdate = true;
						}
					}
				}
			}
		}

		public override void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer,
		OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			if (alchProj.fireFlask.type != 0)
			{
				modTarget.alchemistWater = 60 * 10;
			}
		}
	}
}
