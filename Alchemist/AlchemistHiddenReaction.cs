using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Weapons.Air;
using OrchidMod.Alchemist.Weapons.Fire;
using OrchidMod.Alchemist.Weapons.Nature;
using OrchidMod.Alchemist.Weapons.Water;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist
{
	public class AlchemistHiddenReaction
	{

		public static void MushroomThread(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 56);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
			player.QuickSpawnItem(ItemType<Alchemist.Misc.MushroomThread>(), 1);
		}

		public static void AttractiteShurikens(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 56);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
			player.QuickSpawnItem(ItemType<Alchemist.Weapons.Misc.AttractiteShuriken>(), 5);
		}

		public static void BeeSwarm(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 16);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}

			int itemType = ItemType<QueenBeeFlask>();
			int dmg = OrchidModAlchemistHelper.getSecondaryDamage(player, modPlayer, itemType, 4, true);
			for (int i = 0; i < 10; i++)
			{
				Vector2 vel = (new Vector2(0f, -(float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(80)));
				if (player.strongBees && Main.rand.Next(2) == 0)
					Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, 566, (int)(dmg * 1.15f), 0f, player.whoAmI);
				else
				{
					Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, 181, dmg, 0f, player.whoAmI);
				}
			}
			for (int i = 0; i < 10; i++)
			{
				Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(80)));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Air.QueenBeeFlaskProj>();
				Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
			}
		}

		public static void BubbleSpirited(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			for (int i = 0; i < 10; i++)
			{
				int alpha = 175;
				Color newColor = new Color(0, 80, (int)byte.MaxValue, 100);
				int dust = Dust.NewDust(player.Center, 10, 10, 4, 0.0f, 0.0f, alpha, newColor, 1.2f);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}

			int itemType = ItemType<DungeonFlask>();
			int dmg = OrchidModAlchemistHelper.getSecondaryDamage(player, modPlayer, itemType, 4, true);
			int spawnProj = ProjectileType<Alchemist.Projectiles.Reactive.SpiritedBubble>();
			Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -5f, spawnProj, dmg, 0f, player.whoAmI);
		}

		public static void BubblePoison(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			for (int i = 0; i < 10; i++)
			{
				int alpha = 175;
				Color newColor = new Color(0, 80, (int)byte.MaxValue, 100);
				int dust = Dust.NewDust(player.Center, 10, 10, 4, 0.0f, 0.0f, alpha, newColor, 1.2f);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}

			int itemType = ItemType<PoisonVial>();
			int dmg = OrchidModAlchemistHelper.getSecondaryDamage(player, modPlayer, itemType, 4, true);
			int spawnProj = ProjectileType<Alchemist.Projectiles.Reactive.PoisonBubble>();
			Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -5f, spawnProj, dmg, 0f, player.whoAmI);
		}

		public static void BubbleOil(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			for (int i = 0; i < 10; i++)
			{
				int alpha = 175;
				Color newColor = new Color(0, 80, (int)byte.MaxValue, 100);
				int dust = Dust.NewDust(player.Center, 10, 10, 4, 0.0f, 0.0f, alpha, newColor, 1.2f);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}

			int dmg = 0;
			int spawnProj = ProjectileType<Alchemist.Projectiles.Reactive.OilBubble>();
			Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -5f, spawnProj, dmg, 0f, player.whoAmI);
		}

		public static void BubbleSeafoam(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			for (int i = 0; i < 10; i++)
			{
				int alpha = 175;
				Color newColor = new Color(0, 80, (int)byte.MaxValue, 100);
				int dust = Dust.NewDust(player.Center, 10, 10, 4, 0.0f, 0.0f, alpha, newColor, 1.2f);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}

			int itemType = ItemType<SeafoamVial>();
			int dmg = OrchidModAlchemistHelper.getSecondaryDamage(player, modPlayer, itemType, 4, true);
			int spawnProj = ProjectileType<Alchemist.Projectiles.Reactive.SeafoamBubble>();
			Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -5f, spawnProj, dmg, 0f, player.whoAmI);
		}

		public static void BubbleSap(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			for (int i = 0; i < 10; i++)
			{
				int alpha = 175;
				Color newColor = new Color(0, 80, (int)byte.MaxValue, 100);
				int dust = Dust.NewDust(player.Center, 10, 10, 4, 0.0f, 0.0f, alpha, newColor, 1.2f);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}

			int itemType = ItemType<LivingSapVial>();
			int dmg = OrchidModAlchemistHelper.getSecondaryDamage(player, modPlayer, itemType, 25, false);
			int spawnProj = ProjectileType<Alchemist.Projectiles.Reactive.LivingSapBubble>();
			Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -5f, spawnProj, dmg, 0f, player.whoAmI);
		}

		public static void BubbleSlime(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			for (int i = 0; i < 10; i++)
			{
				int alpha = 175;
				Color newColor = new Color(0, 80, (int)byte.MaxValue, 100);
				int dust = Dust.NewDust(player.Center, 10, 10, 4, 0.0f, 0.0f, alpha, newColor, 1.2f);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}

			int itemType = ItemType<KingSlimeFlask>();
			int dmg = OrchidModAlchemistHelper.getSecondaryDamage(player, modPlayer, itemType, 4, true);
			int spawnProj = ProjectileType<Alchemist.Projectiles.Reactive.SlimeBubble>();
			Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -5f, spawnProj, dmg, 0f, player.whoAmI);
		}

		public static void BubbleSlimeLava(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			for (int i = 0; i < 10; i++)
			{
				int alpha = 175;
				Color newColor = new Color(0, 80, (int)byte.MaxValue, 100);
				int dust = Dust.NewDust(player.Center, 10, 10, 4, 0.0f, 0.0f, alpha, newColor, 1.2f);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}

			int itemType = ItemType<HellSlimeFlask>();
			int dmg = OrchidModAlchemistHelper.getSecondaryDamage(player, modPlayer, itemType, 4, true);
			int spawnProj = ProjectileType<Alchemist.Projectiles.Reactive.SlimeBubbleLava>();
			Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -5f, spawnProj, dmg, 0f, player.whoAmI);
		}

		public static void Propulsion(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			player.jump = 1;
			player.velocity.Y = -15f;
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 15);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
			for (int i = 0; i < 15; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 37);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.2f;
			}
		}

		public static void PotionBuilder(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			player.AddBuff(107, 60 * 30); // Builder
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 15);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
		}

		public static void PotionObsidian(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			player.AddBuff(1, 60 * 30); // Obsidian
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 15);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
		}

		public static void PotionNightOwl(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			player.AddBuff(12, 60 * 30); // Night Owl
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 15);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
		}

		public static void PotionInvisibility(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			player.AddBuff(10, 60 * 30); // Invisibility
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 15);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
		}

		public static void PotionFeatherFall(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			player.AddBuff(8, 60 * 30); // Featherfall
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 16);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
		}

		public static void PotionFlipper(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			player.AddBuff(109, 60 * 30); // Flipper
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 6);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
		}

		public static void AirSpores(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 16);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
			int itemType = ItemType<DeathweedFlask>();
			int itemType2 = ItemType<ShiverthornFlask>();
			itemType = OrchidModAlchemistHelper.containsAlchemistFlask(itemType, player, modPlayer) ? itemType : itemType2;
			int dmg = OrchidModAlchemistHelper.getSecondaryDamage(player, modPlayer, itemType, 4, true);
			for (int i = 0; i < 10; i++)
			{
				Vector2 vel = (new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Air.AirSporeProj>();
				int spawnProj2 = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, player.whoAmI);
				Main.projectile[spawnProj2].localAI[1] = 1f;
			}
			int nb = 4 + Main.rand.Next(3);
			for (int i = 0; i < nb; i++)
			{
				Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Air.AirSporeProjAlt>();
				Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
			}
			if (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<GlowingAttractiteFlask>(), player, modPlayer))
			{
				for (int i = 0; i < 5; i++)
				{
					Vector2 vel = (new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
					int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>();
					int spawnProj2 = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, player.whoAmI);
					Main.projectile[spawnProj2].localAI[1] = 1f;
				}
				for (int i = 0; i < 2; i++)
				{
					Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
					int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProjAlt>();
					Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
				}
			}
		}

		public static void WaterSpores(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 33);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
			int itemType = ItemType<WaterleafFlask>();
			int dmg = OrchidModAlchemistHelper.getSecondaryDamage(player, modPlayer, itemType, 4, true);
			for (int i = 0; i < 10; i++)
			{
				Vector2 vel = (new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Water.WaterSporeProj>();
				int spawnProj2 = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, player.whoAmI);
				Main.projectile[spawnProj2].localAI[1] = 1f;
			}
			int nb = 4 + Main.rand.Next(3);
			for (int i = 0; i < nb; i++)
			{
				Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Water.WaterSporeProjAlt>();
				Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
			}
			if (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<GlowingAttractiteFlask>(), player, modPlayer))
			{
				for (int i = 0; i < 5; i++)
				{
					Vector2 vel = (new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
					int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>();
					int spawnProj2 = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, player.whoAmI);
					Main.projectile[spawnProj2].localAI[1] = 1f;
				}
				for (int i = 0; i < 2; i++)
				{
					Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
					int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProjAlt>();
					Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
				}
			}
		}

		public static void FireSpores(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 6);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
			int itemType = ItemType<BlinkrootFlask>();
			int itemType2 = ItemType<FireblossomFlask>();
			itemType = OrchidModAlchemistHelper.containsAlchemistFlask(itemType, player, modPlayer) ? itemType : itemType2;
			int dmg = OrchidModAlchemistHelper.getSecondaryDamage(player, modPlayer, itemType, 4, true);
			for (int i = 0; i < 10; i++)
			{
				Vector2 vel = (new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Fire.FireSporeProj>();
				int spawnProj2 = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, player.whoAmI);
				Main.projectile[spawnProj2].localAI[1] = 1f;
			}
			int nb = 4 + Main.rand.Next(3);
			for (int i = 0; i < nb; i++)
			{
				Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Fire.FireSporeProjAlt>();
				Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
			}
			if (OrchidModAlchemistHelper.containsAlchemistFlask(ItemType<GlowingAttractiteFlask>(), player, modPlayer))
			{
				for (int i = 0; i < 5; i++)
				{
					Vector2 vel = (new Vector2(0f, -5f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360))));
					int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProj>();
					int spawnProj2 = Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, player.whoAmI);
					Main.projectile[spawnProj2].localAI[1] = 1f;
				}
				for (int i = 0; i < 2; i++)
				{
					Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
					int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.NatureSporeProjAlt>();
					Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
				}
			}
		}

		public static void GlowshroomHealing(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			if (Main.myPlayer == player.whoAmI)
				player.HealEffect(25, true);
			player.statLife += 25;

			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 56);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
		}

		public static void Bubbles(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
		}

		public static void SunflowerSeeds(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			int itemType = ItemType<SunflowerFlask>();
			int dmg = OrchidModAlchemistHelper.getSecondaryDamage(player, modPlayer, itemType, 4, true);
			int nb = 5 + Main.rand.Next(4);

			for (int i = 0; i < 5; i++)
			{
				Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(10)).RotatedBy(MathHelper.ToRadians(-40 + (20 * i))));
				Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Nature.SunflowerFlaskProj1>(), dmg, 0f, player.whoAmI);
			}

			for (int i = 0; i < nb; i++)
			{
				Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.SunflowerFlaskProj4>();
				Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
			}
		}

		public static void StellarTalcOrbit(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			player.AddBuff(BuffType<Alchemist.Buffs.StellarTalcBuff>(), 60 * 60);
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 15);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
		}

		public static void PermanentFreeze(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			player.AddBuff(BuffType<Alchemist.Buffs.IceChestFlaskBuff>(), 60 * 60);
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 261);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
		}

		public static void PoisonousSlime(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			player.AddBuff(BuffType<Alchemist.Buffs.KingSlimeFlaskBuff>(), 60 * 60);
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 44);
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
		}

		public static void LiliesPurification(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			player.AddBuff(BuffType<Alchemist.Buffs.JungleLilyExtractBuff>(), 60 * 60);
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 15);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
		}

		public static void BurningSamples(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			player.AddBuff(BuffType<Alchemist.Buffs.SlimeFlaskBuff>(), 60 * 60);
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 29);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
		}

		public static void LivingBeehive(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			player.AddBuff(BuffType<Alchemist.Buffs.QueenBeeFlaskBuff>(), 60 * 60);
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 153);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
		}

		public static void DemonReeks(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			player.AddBuff(BuffType<Alchemist.Buffs.DemonBreathFlaskBuff>(), 60 * 60);
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 21);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
		}

		public static void SpiritedDroplets(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			player.AddBuff(BuffType<Alchemist.Buffs.SpiritedWaterBuff>(), 60 * 60);
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 29);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
		}


		public static void MistySteps(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			player.AddBuff(BuffType<Alchemist.Buffs.BloodMistFlaskBuff>(), 60 * 60);
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(player.Center, 10, 10, 5);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 1.5f;
			}
		}

		public static void JungleLilyPurification(AlchemistHiddenReactionRecipe recipe, Player player, OrchidModPlayer modPlayer)
		{
			int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.JungleLilyFlaskReaction>();
			Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, spawnProj, 0, 0f, player.whoAmI);
			OrchidModProjectile.spawnDustCircle(player.Center, 15, 10, 7, true, 1.5f, 1f, 3f);
			OrchidModProjectile.spawnDustCircle(player.Center, 15, 15, 10, true, 1.5f, 1f, 5f);
		}
	}
}