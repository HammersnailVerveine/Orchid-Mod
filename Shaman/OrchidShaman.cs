using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Alchemist;
using OrchidMod.Dancer;
using OrchidMod.Gambler;
using OrchidMod.Shaman;
using OrchidMod.Guardian;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;
using OrchidMod.Common;
using OrchidMod.Shaman.Buffs;
using OrchidMod.Buffs;
using OrchidMod.Shaman.Projectiles.Equipment;
using OrchidMod.Shaman.Buffs.Thorium;
using OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Big;
using OrchidMod.Shaman.Projectiles;
using OrchidMod.Shaman.Buffs.Debuffs;

namespace OrchidMod
{
	public class OrchidShaman : ModPlayer
	{
		public OrchidPlayer modPlayer;

		public int shamanBuffTimer = 6;
		public float shamanExhaustionRate = 1.0f;
		public int UIDisplayTimer = 0;
		public int UIDisplayDelay = 60 * 3; // 3 Seconds

		public int shamanHitDelay = 0;
		public int shamanSelectedItem = 0;
		public int shamanCatalystIndex = 0;

		public ShamanOrbSmall shamanOrbSmall = ShamanOrbSmall.NULL;
		public ShamanOrbBig shamanOrbBig = ShamanOrbBig.NULL;
		public ShamanOrbLarge shamanOrbLarge = ShamanOrbLarge.NULL;
		public ShamanOrbUnique shamanOrbUnique = ShamanOrbUnique.NULL;
		public ShamanOrbCircle shamanOrbCircle = ShamanOrbCircle.NULL;

		public int orbCountSmall = 0;
		public int orbCountBig = 0;
		public int orbCountLarge = 0;
		public int orbCountUnique = 0;
		public int orbCountCircle = 0;

		public int shamanFireTimer = 0;
		public int shamanWaterTimer = 0;
		public int shamanAirTimer = 0;
		public int shamanEarthTimer = 0;
		public int shamanSpiritTimer = 0;

		public int shamanFireBondLoading = 0;
		public int shamanWaterBondLoading = 0;
		public int shamanAirBondLoading = 0;
		public int shamanEarthBondLoading = 0;
		public int shamanSpiritBondLoading = 0;

		public bool shamanPollFireMax = false;
		public bool shamanPollWaterMax = false;
		public bool shamanPollAirMax = false;
		public bool shamanPollEarthMax = false;
		public bool shamanPollSpiritMax = false;

		public int shamanPollFire = 0;
		public int shamanPollWater = 0;
		public int shamanPollAir = 0;
		public int shamanPollEarth = 0;
		public int shamanPollSpirit = 0;

		public bool shamanFire = false;
		public bool shamanIce = false;
		public bool shamanPoison = false;
		public bool shamanVenom = false;
		public bool shamanHoney = false;
		public bool shamanFeather = false;
		public bool shamanDripping = false;
		public bool shamanAmber = false;
		public bool shamanDryad = false;
		public bool shamanForest = false;
		public bool shamanHeavy = false;
		public bool shamanSkull = false;
		public bool shamanWaterHoney = false;
		public bool shamanSmite = false;
		public bool shamanCrimtane = false;
		public bool shamanDemonite = false;
		public bool shamanDownpour = false;
		public bool shamanHell = false;
		public bool shamanHarpyAnklet = false;
		public bool abyssSet = false;
		public bool shamanShadowEmpowerment = false;
		public bool shamanMourningTorch = false;
		public bool shamanSunBelt = false;
		public bool shamanVampire = false;
		public bool shamanDestroyer = false;
		public bool shamanDiabolist = false;
		public bool shamanWyvern = false;
		public bool shamanRage = false;
		public bool shamanAmethyst = false;
		public bool shamanTopaz = false;
		public bool shamanSapphire = false;
		public bool shamanEmerald = false;
		public bool shamanRuby = false;

		public bool harpyAnkletLock = true;
		public int shamanTimerCrimson = 0;
		public int shamanTimerViscount = 0;
		public int shamanTimerHellDamage = 600;
		public int shamanTimerHellDefense = 300;
		public int shamanTimerDestroyer = 0;
		public int shamanDestroyerCount = 0;
		public int shamanTimerDiabolist = 0;
		public int shamanDiabolistCount = 0;
		public int timerVial = 0;

		public bool doubleJumpHarpy = false;
		public bool harpySpaceKeyReleased = false;
		public int jumpHeightCheck = -1;
		public bool abyssalWings = false;

		public bool HasAnyBondLoaded() =>
			(shamanFireBondLoading +
			shamanWaterBondLoading +
			shamanAirBondLoading +
			shamanEarthBondLoading +
			shamanSpiritBondLoading) > 0;

		public int GetDamage(int damage) => (int)Player.GetDamage<ShamanDamageClass>().ApplyTo(damage);

		public int GetNbShamanicBonds()
		{
			int val = 0;

			if (shamanFireTimer > 0)
				val++;

			if (shamanWaterTimer > 0)
				val++;

			if (shamanAirTimer > 0)
				val++;

			if (shamanEarthTimer > 0)
				val++;

			if (shamanSpiritTimer > 0)
				val++;

			return val;
		}

		public Vector2? ShamanCatalystPosition
		{
			get
			{
				var proj = Main.projectile[this.shamanCatalystIndex];
				if (proj == null || !proj.active) return null;

				return proj.Center;
			}
		}

		public void Reset()
		{
			shamanCatalystIndex = -1;

			orbCountSmall = 0;
			orbCountBig = 0;
			orbCountLarge = 0;
			orbCountUnique = 0;
			orbCountCircle = 0;

			shamanOrbBig = ShamanOrbBig.NULL;
			shamanOrbSmall = ShamanOrbSmall.NULL;
			shamanOrbLarge = ShamanOrbLarge.NULL;
			shamanOrbUnique = ShamanOrbUnique.NULL;
			shamanOrbCircle = ShamanOrbCircle.NULL;

			shamanFireTimer = 0;
			shamanWaterTimer = 0;
			shamanAirTimer = 0;
			shamanEarthTimer = 0;
			shamanSpiritTimer = 0;
			UIDisplayTimer = 0;
		}

		public override void Initialize()
		{
			modPlayer = Player.GetModPlayer<OrchidPlayer>();

			Reset();
		}

		public override void PostUpdateEquips()
		{
			/*
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				object result = thoriumMod.Call("GetAllCrit", Player);
				if (result is int thoriumCrit && thoriumCrit > 0)
				{
					this.shamanCrit += thoriumCrit;
				}
			}
			*/

			if (UIDisplayTimer == 0)
			{
				shamanFireBondLoading = 0;
				shamanWaterBondLoading = 0;
				shamanAirBondLoading = 0;
				shamanEarthBondLoading = 0;
				shamanSpiritBondLoading = 0;
			}

			if (shamanPollFire > -300)
			{
				shamanPollFire--;
				shamanFireBondLoading += shamanPollFire > 0 && shamanFireBondLoading < 100 ? 1 : 0;
				shamanPollFireMax = shamanPollFireMax || shamanFireBondLoading == 100;
			}
			else
			{
				shamanPollFire = shamanPollFireMax ? -295 : -290;
				shamanFireBondLoading -= shamanFireBondLoading > 0 ? 1 : 0;
				shamanPollFireMax = shamanPollFireMax && shamanFireBondLoading > 0;
			}

			if (shamanPollWater > -300)
			{
				shamanPollWater--;
				shamanWaterBondLoading += shamanPollWater > 0 && shamanWaterBondLoading < 100 ? 1 : 0;
				shamanPollWaterMax = shamanPollWaterMax || shamanWaterBondLoading == 100;
			}
			else
			{
				shamanPollWater = shamanPollWaterMax ? -295 : -290;
				shamanWaterBondLoading -= shamanWaterBondLoading > 0 ? 1 : 0;
				shamanPollWaterMax = shamanPollWaterMax && shamanWaterBondLoading > 0;
			}

			if (shamanPollAir > -300)
			{
				shamanPollAir--;
				shamanAirBondLoading += shamanPollAir > 0 && shamanAirBondLoading < 100 ? 1 : 0;
				shamanPollAirMax = shamanPollAirMax || shamanAirBondLoading == 100;
			}
			else
			{
				shamanPollAir = shamanPollAirMax ? -295 : -290;
				shamanAirBondLoading -= shamanAirBondLoading > 0 ? 1 : 0;
				shamanPollAirMax = shamanPollAirMax && shamanAirBondLoading > 0;
			}

			if (shamanPollEarth > -300)
			{
				shamanPollEarth--;
				shamanEarthBondLoading += shamanPollEarth > 0 && shamanEarthBondLoading < 100 ? 1 : 0;
				shamanPollEarthMax = shamanPollEarthMax || shamanEarthBondLoading == 100;
			}
			else
			{
				shamanPollEarth = shamanPollEarthMax ? -295 : -290;
				shamanEarthBondLoading -= shamanEarthBondLoading > 0 ? 1 : 0;
				shamanPollEarthMax = shamanPollEarthMax && shamanEarthBondLoading > 0;
			}

			if (shamanPollSpirit > -300)
			{
				shamanPollSpirit--;
				shamanSpiritBondLoading += shamanPollSpirit > 0 && shamanSpiritBondLoading < 100 ? 1 : 0;
				shamanPollSpiritMax = shamanPollSpiritMax || shamanSpiritBondLoading == 100;
			}
			else
			{
				shamanPollSpirit = shamanPollSpiritMax ? -295 : -290;
				shamanSpiritBondLoading -= shamanSpiritBondLoading > 0 ? 1 : 0;
				shamanPollSpiritMax = shamanPollSpiritMax && shamanSpiritBondLoading > 0;
			}

			if (HasAnyBondLoaded())
			{
				UIDisplayTimer = UIDisplayDelay;
			}

			if (shamanFireTimer > 0)
			{

				if (shamanRuby)
				{
					Player.lifeRegen += 2;
				}

				if (shamanSmite && modPlayer.timer120 % 60 == 0)
				{
					int dmg = (int)Player.GetDamage<ShamanDamageClass>().ApplyTo(100);

					int randX = Main.rand.Next(50);
					int randY = Main.rand.Next(50);

					for (int i = 0; i < 3; i++)
					{
						int dust = Dust.NewDust(new Vector2((int)(Player.Center.X + 25 - randX), (int)(Player.Center.Y + 15 - randY)), 0, 0, 162, (float)(1 - Main.rand.Next(2)), (float)(1 - Main.rand.Next(2)), 0, default(Color), 2f);
						Main.dust[dust].noGravity = true;
					}

					Projectile.NewProjectile(null, (int)(Player.Center.X + 25 - randX), (int)(Player.Center.Y + 15 - randY), 0f, 0f, ModContent.ProjectileType<Smite>(), dmg, 0f, Player.whoAmI);
				}

			}

			if (shamanWaterTimer > 0)
			{

				if (shamanSapphire)
				{
					shamanExhaustionRate -= 0.1f;
				}

				if (shamanHeavy)
				{
					Player.statDefense += 10;
					Player.moveSpeed -= 0.2f;
				}

			}

			if (shamanAirTimer > 0)
			{
				float vel = Math.Abs(Player.velocity.X) + Math.Abs(Player.velocity.Y);

				if (shamanDripping)
				{
					if (modPlayer.timer120 % 10 == 0)
					{
						int dmg = (int)Player.GetDamage<ShamanDamageClass>().ApplyTo(30);
						Projectile.NewProjectile(null, Player.Center.X - 10 + (Main.rand.Next(20)), Player.Center.Y + 16, 0f, -0.001f, ModContent.ProjectileType<Shaman.Projectiles.Equipment.LavaDroplet>(), dmg, 0f, Player.whoAmI);
					}
				}

				if (shamanEmerald)
				{
					Player.moveSpeed += 0.1f;
				}

				if (shamanFeather)
				{
					if (!Player.controlDown) Player.gravity /= 3;
				}

				if (shamanHarpyAnklet)
				{
					doubleJumpHarpy = true;
				}
			}

			if (shamanEarthTimer > 0)
			{
				if (shamanHoney)
				{
					Player.AddBuff((48), 1); // Honey
					if (shamanEarthTimer % 90 == 0)
					{

						int randX = Main.rand.Next(150);
						int randY = Main.rand.Next(100);

						for (int i = 0; i < 3; i++)
						{
							int dust = Dust.NewDust(new Vector2((int)(Player.Center.X + 75 - randX), (int)(Player.Center.Y + 15 - randY)), 0, 0, 152, 0f, 0f, 0, default(Color), 1.3f);
							Main.dust[dust].noGravity = true;
						}

						if (Main.player[Main.myPlayer].strongBees && Main.rand.NextBool(2))
							Projectile.NewProjectile(null, (int)(Player.Center.X + 75 - randX), (int)(Player.Center.Y + 15 - randY), (float)(Main.rand.Next(3) - 1.5), (float)(Main.rand.Next(3) - 1.5), 566, (int)(12), 0f, Player.whoAmI, 0f, 0f);
						else
							Projectile.NewProjectile(null, (int)(Player.Center.X + 75 - randX), (int)(Player.Center.Y + 15 - randY), (float)(Main.rand.Next(3) - 1.5), (float)(Main.rand.Next(3) - 1.5), 181, (int)(10), 0f, Player.whoAmI, 0f, 0f);
					}
				}

				if (shamanTopaz)
				{
					Player.statDefense += 5;
				}

				if (shamanForest)
				{
					Player.AddBuff(ModContent.BuffType<DeepForestAura>(), 2);
				}

				if (shamanAmber)
					Player.statLifeMax2 += 50;

			}

			if (shamanSpiritTimer > 0)
			{

				if (shamanAmethyst)
				{
					Player.GetDamage<ShamanDamageClass>() += 0.1f;
				}
			}

			if (doubleJumpHarpy)  // Vanilla double jump code is insanely weird.
			{
				if (!Player.controlJump) harpySpaceKeyReleased = true;

				if (!(Player.hasJumpOption_Cloud || Player.hasJumpOption_Sail || Player.hasJumpOption_Sandstorm
				|| Player.hasJumpOption_Blizzard || Player.hasJumpOption_Fart || Player.hasJumpOption_Unicorn))
					Player.hasJumpOption_Cloud = true;

				if (Player.velocity.Y == 0 || Player.grappling[0] >= 0 || (shamanHarpyAnklet && shamanSpiritTimer > 0 && !Player.controlJump))
				{
					if (Player.canJumpAgain_Cloud)
					{
						jumpHeightCheck = (int)((double)Player.jumpHeight * 0.75);
					}
					if (Player.canJumpAgain_Sail)
					{
						jumpHeightCheck = (int)((double)Player.jumpHeight * 1.25);
					}
					if (Player.canJumpAgain_Fart)
					{
						jumpHeightCheck = Player.jumpHeight * 2;
					}
					if (Player.canJumpAgain_Blizzard)
					{
						jumpHeightCheck = (int)((double)Player.jumpHeight * 1.5);
					}
					if (Player.canJumpAgain_Sandstorm)
					{
						jumpHeightCheck = Player.jumpHeight * 3;
					}
					if (Player.canJumpAgain_Unicorn)
					{
						jumpHeightCheck = Player.jumpHeight * 2;
					}
				}

				if (Player.canJumpAgain_Cloud && Player.jump == (int)((double)Player.jumpHeight * 0.75))
					Player.jump--;

				if ((Player.jump == jumpHeightCheck && harpySpaceKeyReleased == true))
				{
					harpySpaceKeyReleased = false;
					int dmg = 10;
					if (shamanHarpyAnklet && shamanSpiritTimer > 0)
					{
						dmg += (int)Player.GetDamage<ShamanDamageClass>().ApplyTo(12);
						if (Player.FindBuffIndex(ModContent.BuffType<HarpyAgility>()) > -1)
							dmg += (int)Player.GetDamage<ShamanDamageClass>().ApplyTo(12);
					}

					for (float dirX = -1; dirX < 2; dirX++)
					{
						for (float dirY = -1; dirY < 2; dirY++)
						{
							bool ankletCanShoot = !(dirX == 0 && dirY == 0 && dirX == dirY);
							float ankletSpeed = 10f;
							if (dirX != 0 && dirY != 0) ankletSpeed = 7.5f;
							if (ankletCanShoot)
							{
								Projectile.NewProjectile(null, Player.Center.X, Player.Center.Y, (dirX * ankletSpeed), (dirY * ankletSpeed), ModContent.ProjectileType<HarpyAnkletProj>(), dmg, 0.0f, Player.whoAmI, 0.0f, 0.0f);
							}
						}
					}
				}
			}

			if (shamanFireTimer + shamanWaterTimer + shamanAirTimer + shamanEarthTimer + shamanSpiritTimer == 0)
			{
				UIDisplayTimer -= UIDisplayTimer > 0 ? 1 : 0;
			}
			else
			{
				UIDisplayTimer = UIDisplayDelay;
			}

			if (modPlayer.doubleTap > 0) modPlayer.doubleTap--;
			else modPlayer.doubleTapLock = false;
			if (modPlayer.doubleTapCooldown > 0) modPlayer.doubleTapCooldown--;

			bool adequateControl = Main.ReversedUpDownArmorSetBonuses ? Player.controlUp : Player.controlDown;

			if (adequateControl && modPlayer.doubleTap == 0 && modPlayer.doubleTapCooldown == 0)
			{
				modPlayer.doubleTap += 30;
				modPlayer.doubleTapCooldown = 60;
				modPlayer.doubleTapLock = true;
			}

			if (!adequateControl && modPlayer.doubleTap > 0 && modPlayer.doubleTapLock)
			{
				modPlayer.doubleTapLock = false;
			}

			if (adequateControl && modPlayer.doubleTap > 0 && !modPlayer.doubleTapLock)
			{
				if (abyssSet)
				{
					Projectile.NewProjectile(null, Main.MouseWorld.X, Main.MouseWorld.Y, 0f, 0f, ModContent.ProjectileType<Shaman.Projectiles.Equipment.Abyss.AbyssPortal>(), 0, 5, Player.whoAmI);
					SoundEngine.PlaySound(SoundID.Item122, Player.Center);
				}
				modPlayer.doubleTap = 0;
				modPlayer.doubleTapCooldown += 1000;
			}
		}

		public override void PostUpdate()
		{
			if (shamanFireTimer <= 0
			&& shamanWaterTimer <= 0
			&& shamanAirTimer <= 0
			&& shamanEarthTimer <= 0
			&& shamanSpiritTimer <= 0)
			{

				shamanOrbBig = ShamanOrbBig.NULL;
				shamanOrbSmall = ShamanOrbSmall.NULL;
				shamanOrbLarge = ShamanOrbLarge.NULL;
				shamanOrbUnique = ShamanOrbUnique.NULL;

				orbCountSmall = 0;
				orbCountBig = 0;
				orbCountLarge = 0;
				orbCountUnique = 0;
				orbCountCircle = 0;
			}

			if (!(Player.FindBuffIndex(ModContent.BuffType<SpiritualBurst>()) > -1) && orbCountCircle > 39 && shamanOrbCircle == ShamanOrbCircle.REVIVER)
				orbCountCircle = 0;

			if (orbCountBig < 0)
				orbCountBig = 0;

			if (shamanTimerCrimson < 30)
				shamanTimerCrimson++;

			if (shamanTimerViscount < 180)
				shamanTimerViscount++;

			if (shamanTimerHellDamage < 600)
				shamanTimerHellDamage++;

			if (shamanTimerHellDefense < 300)
				shamanTimerHellDefense++;


			if (timerVial < 30)
				timerVial++;
		}

		public override void OnHitNPCWithProj(Projectile projectile, NPC target, int damage, float knockback, bool crit)
		{
			OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			if (modProjectile.shamanProjectile)
			{
				if (shamanCrimtane && shamanEarthTimer > 0 && shamanTimerCrimson == 30)
				{
					shamanTimerCrimson = 0;
					if (Main.myPlayer == Player.whoAmI)
						Player.HealEffect(2, true);
					Player.statLife += 2;
				}

				if (shamanVampire && Main.rand.NextBool(5) && shamanTimerViscount == 180)
				{
					shamanTimerViscount = 0;
					Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(40));
					if (Main.rand.NextBool(2))
					{
						int type = ProjectileType<Shaman.Projectiles.Thorium.Equipment.Viscount.ViscountBlood>();
						Projectile.NewProjectile(Player.GetSource_OnHit(target), target.Center.X, target.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, type, 0, 0.0f, projectile.owner, 0.0f, 0.0f);
					}
					else
					{
						int type = ProjectileType<Shaman.Projectiles.Thorium.Equipment.Viscount.ViscountSound>();
						Projectile.NewProjectile(Player.GetSource_OnHit(target), target.Center.X, target.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, type, 0, 0.0f, projectile.owner, 0.0f, 0.0f);
					}
				}

				if (shamanHell && shamanTimerHellDamage == 600 && shamanFireTimer > 0)
				{
					shamanTimerHellDamage = 0;
					for (int i = 0; i < 10; i++)
					{
						Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(360));
						int dmg = GetDamage(50);
						int type = ProjectileType<Shaman.Projectiles.Equipment.Hell.ShamanHellHoming>();
						Projectile.NewProjectile(Player.GetSource_OnHit(target), target.Center.X, target.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, type, dmg, 0.0f, projectile.owner, 0.0f, 0.0f);
					}
				}
			}
		}

		public override void ModifyHitNPCWithProj(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			if (modProjectile.shamanProjectile)
			{
				/*
				if (Main.rand.Next(101) <= this.shamanCrit + modProjectile.baseCritChance)
					crit = true;
				else crit = false;
				*/

				if (crit)
				{
					if (Player.FindBuffIndex(ModContent.BuffType<OpalEmpowerment>()) > -1)
					{
						damage += 5;
					}

					if (Player.FindBuffIndex(ModContent.BuffType <DestroyerFrenzy>()) > -1)
					{
						damage = (int)(damage * 1.15f);
					}

					if (projectile.type == ModContent.ProjectileType<TitanicScepterProj>())
					{
						damage = (int)(damage * 1.5f);
					}

					if (Player.FindBuffIndex(ModContent.BuffType<TitanicBuff>()) > -1)
					{
						damage = (int)(damage * 1.2f);
					}

					if (shamanDestroyer && shamanWaterTimer > 0)
					{
						shamanTimerDestroyer = 60;
						shamanDestroyerCount++;
					}
				}

				if (target.type != NPCID.TargetDummy)
				{
					if (projectile.type != ModContent.ProjectileType<LavaDroplet>()
						&& projectile.type != ModContent.ProjectileType<LostSoul>()
						&& projectile.type != ModContent.ProjectileType<HarpyAnkletProj>()
						&& projectile.type != ModContent.ProjectileType<DeepForestCharmProj>()
						&& projectile.type != ModContent.ProjectileType<Smite>()
						)
					{

						if (shamanFireTimer > 0)
						{
							if (shamanPoison) target.AddBuff((20), 5 * 60);
							if (shamanVenom) target.AddBuff((70), 5 * 60);
							if (shamanFire) target.AddBuff((24), 5 * 60);
							if (shamanIce) target.AddBuff((44), 5 * 60);
							if (shamanDemonite) target.AddBuff(153, 20); // Shadowflame
						}

						if (crit == true && shamanSkull && shamanWaterTimer > 0)
						{
							int dmg = GetDamage(80);
							Vector2 mouseTarget = Main.MouseWorld;
							Vector2 heading = mouseTarget - Main.player[projectile.owner].position;
							heading.Normalize();
							heading *= new Vector2(5f, 5f).Length();
							Vector2 projectileVelocity = (new Vector2(heading.X, heading.Y).RotatedByRandom(MathHelper.ToRadians(10)));
							Projectile.NewProjectile(null, Main.player[projectile.owner].Center.X, Main.player[projectile.owner].Center.Y, projectileVelocity.X, projectileVelocity.Y, ModContent.ProjectileType<LostSoul>(), dmg, 0f, projectile.owner, 0f, 0f);
						}

						if (crit == true && shamanWaterHoney && shamanWaterTimer > 0 && timerVial == 30)
						{
							timerVial = 0;
							Player.HealEffect(3, true);
							Player.statLife += 3;
						}

						if (Main.rand.NextBool(15) && shamanDownpour && GetNbShamanicBonds() > 2)
						{
							int dmg = (int)Player.GetDamage<ShamanDamageClass>().ApplyTo(50);
							target.StrikeNPCNoInteraction(dmg, 0f, 0);
							SoundEngine.PlaySound(SoundID.Item93, target.Center);

							for (int i = 0; i < 15; i++)
							{
								int dust = Dust.NewDust(target.position, target.width, target.height, 226);
								Main.dust[dust].noGravity = true;
								Main.dust[dust].velocity *= 2f;
							}
						}
					}
				}
			}
		}

		public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			if (shamanShadowEmpowerment)
			{
				if (Main.rand.NextBool(4) && drawInfo.shadow == 0f)
				{
					int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f, 2f), Player.width + 4, Player.height + 4, 27, Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default(Color), 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.8f;
					Main.dust[dust].velocity.Y -= 0.5f;
					drawInfo.DustCache.Add(dust);
				}
			}

			if (abyssalWings && Player.controlJump) // Don't works if it is in the vanity slot
			{
				if (Main.rand.NextBool(6) && drawInfo.shadow == 0f && Player.wingTime > 0)
				{
					int dust = Dust.NewDust(drawInfo.Position - new Vector2(15f, 2f), Player.width + 30, Player.height + 4, DustType<Content.Dusts.AbyssalDust>());
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity.Y -= 0.5f;
					Main.dust[dust].scale = 1.75f;
					//Main.playerDrawDust.Add(dust);
					drawInfo.DustCache.Add(dust);
				}
				if (Main.rand.NextBool(6) && drawInfo.shadow == 0f && Player.wingTime > 0)
				{
					int dust = Dust.NewDust(drawInfo.Position - new Vector2(15f, 2f), Player.width + 30, Player.height + 4, DustType<Content.Dusts.AbyssalBrightDust>());
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity.Y -= 0.5f;
					Main.dust[dust].scale = 1.75f;
					//Main.playerDrawDust.Add(dust);
					drawInfo.DustCache.Add(dust);
				}
			}
		}

		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit,
		ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (shamanMourningTorch)
			{
				shamanFireTimer -= 5 * 60;
				shamanFireTimer = shamanFireTimer > 0 ? shamanFireTimer : 0;
				shamanWaterTimer -= 5 * 60;
				shamanWaterTimer = shamanWaterTimer > 0 ? shamanWaterTimer : 0;
				shamanAirTimer -= 5 * 60;
				shamanAirTimer = shamanAirTimer > 0 ? shamanAirTimer : 0;
				shamanEarthTimer -= 5 * 60;
				shamanEarthTimer = shamanEarthTimer > 0 ? shamanEarthTimer : 0;
				shamanSpiritTimer -= 5 * 60;
				shamanSpiritTimer = shamanSpiritTimer > 0 ? shamanSpiritTimer : 0;
			}

			if (shamanSunBelt)
			{
				Player.AddBuff(ModContent.BuffType<BrokenPower>(), 60 * 15);
			}

			if (shamanHell && shamanTimerHellDefense == 300 && shamanEarthTimer > 0)
			{
				shamanTimerHellDefense = 0;
				int dmg = (int)Player.GetDamage<ShamanDamageClass>().ApplyTo(50);
				Projectile.NewProjectile(null, Player.Center.X, Player.Center.Y, 0f, 0f, ProjectileType<Shaman.Projectiles.Equipment.Hell.ShamanHellExplosion>(), dmg, 0.0f, Player.whoAmI, 0.0f, 0.0f);
				SoundEngine.PlaySound(SoundID.Item14);
				OrchidModProjectile.spawnDustCircle(Player.Center, 6, 10, 15, true, 1f, 1f, 8f, true, true, false, 0, 0, true);
				OrchidModProjectile.spawnDustCircle(Player.Center, 6, 10, 15, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
				OrchidModProjectile.spawnDustCircle(Player.Center, 6, 10, 15, true, 2f, 1f, 3f, true, true, false, 0, 0, true);
			}

			if (shamanDiabolist && shamanEarthTimer > 0)
			{
				shamanTimerDiabolist = 60;
				shamanDiabolistCount += damage;
			}

			return true;
		}

		public override void ResetEffects()
		{
			shamanExhaustionRate = 1.0f;
			shamanBuffTimer = 6;
			doubleJumpHarpy = false;
			abyssalWings = false;
			abyssSet = false;

			shamanFire = false;
			shamanIce = false;
			shamanPoison = false;
			shamanVenom = false;
			shamanHoney = false;
			shamanFeather = false;
			shamanVampire = false;
			shamanDestroyer = false;
			shamanDiabolist = false;
			shamanDripping = false;
			shamanAmber = false;
			shamanDryad = false;
			shamanForest = false;
			shamanHeavy = false;
			shamanSkull = false;
			shamanWaterHoney = false;
			shamanSmite = false;
			shamanCrimtane = false;
			shamanDemonite = false;
			shamanDownpour = false;
			shamanHell = false;
			shamanHarpyAnklet = false;
			shamanWyvern = false;
			shamanRage = false;
			shamanAmethyst = false;
			shamanTopaz = false;
			shamanSapphire = false;
			shamanEmerald = false;
			shamanRuby = false;

			shamanShadowEmpowerment = false;
			shamanMourningTorch = false;
			shamanSunBelt = false;

			shamanHitDelay -= shamanHitDelay > 0 && modPlayer.timer120 % 5 == 0 ? 1 : 0;

			shamanFireTimer -= shamanFireTimer > 0 ? 1 : 0;
			shamanWaterTimer -= shamanWaterTimer > 0 ? 1 : 0;
			shamanAirTimer -= shamanAirTimer > 0 ? 1 : 0;
			shamanEarthTimer -= shamanEarthTimer > 0 ? 1 : 0;
			shamanSpiritTimer -= shamanSpiritTimer > 0 ? 1 : 0;
		}

		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
			Reset();
		}

		public override void OnRespawn(Player player)
		{
			Reset();
		}

		public override void clientClone(ModPlayer clientClone)
		{
			OrchidShaman clone = clientClone as OrchidShaman;

			clone.shamanOrbSmall = this.shamanOrbSmall;
			clone.shamanOrbBig = this.shamanOrbBig;
			clone.shamanOrbLarge = this.shamanOrbLarge;
			clone.shamanOrbUnique = this.shamanOrbUnique;
			clone.shamanOrbCircle = this.shamanOrbCircle;

			clone.orbCountSmall = this.orbCountSmall;
			clone.orbCountBig = this.orbCountBig;
			clone.orbCountLarge = this.orbCountLarge;
			clone.orbCountUnique = this.orbCountUnique;
			clone.orbCountCircle = this.orbCountCircle;
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)OrchidModMessageType.ORCHIDPLAYERSYNCPLAYER);
			packet.Write((byte)Player.whoAmI);

			packet.Write((byte)shamanOrbSmall);
			packet.Write((byte)shamanOrbBig);
			packet.Write((byte)shamanOrbLarge);
			packet.Write((byte)shamanOrbUnique);
			packet.Write((byte)shamanOrbCircle);

			packet.Write(orbCountSmall);
			packet.Write(orbCountBig);
			packet.Write(orbCountLarge);
			packet.Write(orbCountUnique);
			packet.Write(orbCountCircle);

			packet.Write(shamanFireTimer);
			packet.Write(shamanWaterTimer);
			packet.Write(shamanAirTimer);
			packet.Write(shamanEarthTimer);
			packet.Write(shamanSpiritTimer);

			packet.Send(toWho, fromWho);
		}

		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			OrchidShaman clone = clientPlayer as OrchidShaman;

			//Orb Types
			if (clone.shamanOrbSmall != shamanOrbSmall)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDSMALL);
				packet.Write((byte)Player.whoAmI);
				packet.Write((byte)shamanOrbSmall);
				packet.Send();
			}

			if (clone.shamanOrbBig != shamanOrbBig)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDBIG);
				packet.Write((byte)Player.whoAmI);
				packet.Write((byte)shamanOrbBig);
				packet.Send();
			}

			if (clone.shamanOrbLarge != shamanOrbLarge)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDLARGE);
				packet.Write((byte)Player.whoAmI);
				packet.Write((byte)shamanOrbLarge);
				packet.Send();
			}

			if (clone.shamanOrbUnique != shamanOrbUnique)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDUNIQUE);
				packet.Write((byte)Player.whoAmI);
				packet.Write((byte)shamanOrbUnique);
				packet.Send();
			}

			if (clone.shamanOrbCircle != shamanOrbCircle)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBTYPECHANGEDCIRCLE);
				packet.Write((byte)Player.whoAmI);
				packet.Write((byte)shamanOrbCircle);
				packet.Send();
			}

			// Orb Counts
			if (clone.orbCountSmall != orbCountSmall)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDSMALL);
				packet.Write((byte)Player.whoAmI);
				packet.Write(orbCountSmall);
				packet.Send();
			}

			if (clone.orbCountBig != orbCountBig)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDBIG);
				packet.Write((byte)Player.whoAmI);
				packet.Write(orbCountBig);
				packet.Send();
			}

			if (clone.orbCountLarge != orbCountLarge)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDLARGE);
				packet.Write((byte)Player.whoAmI);
				packet.Write(orbCountLarge);
				packet.Send();
			}

			if (clone.orbCountUnique != orbCountUnique)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDUNIQUE);
				packet.Write((byte)Player.whoAmI);
				packet.Write(orbCountUnique);
				packet.Send();
			}

			if (clone.orbCountCircle != orbCountCircle)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANORBCOUNTCHANGEDCIRCLE);
				packet.Write((byte)Player.whoAmI);
				packet.Write(orbCountCircle);
				packet.Send();
			}

			//Empowerment Timers
			if (clone.shamanFireTimer != shamanFireTimer)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDATTACK);
				packet.Write((byte)Player.whoAmI);
				packet.Write(shamanFireTimer);
				packet.Send();
			}

			if (clone.shamanWaterTimer != shamanWaterTimer)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDARMOR);
				packet.Write((byte)Player.whoAmI);
				packet.Write(shamanWaterTimer);
				packet.Send();
			}

			if (clone.shamanAirTimer != shamanAirTimer)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDCRITICAL);
				packet.Write((byte)Player.whoAmI);
				packet.Write(shamanAirTimer);
				packet.Send();
			}

			if (clone.shamanEarthTimer != shamanEarthTimer)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDREGENERATION);
				packet.Write((byte)Player.whoAmI);
				packet.Write(shamanEarthTimer);
				packet.Send();
			}

			if (clone.shamanSpiritTimer != shamanSpiritTimer)
			{
				var packet = Mod.GetPacket();
				packet.Write((byte)OrchidModMessageType.SHAMANBUFFTIMERCHANGEDSPEED);
				packet.Write((byte)Player.whoAmI);
				packet.Write(shamanSpiritTimer);
				packet.Send();
			}
		}

		public void AddShamanicEmpowerment(int type)
		{
			if (type == 0)
			{
				return;
			}

			if (shamanForest && type == 4 && shamanEarthTimer == 0)
			{
				Player.AddBuff(BuffType<Shaman.Buffs.DeepForestAura>(), 1);
				int projType = ProjectileType<Shaman.Projectiles.Equipment.DeepForestCharmProj>();
				Projectile.NewProjectile(null, Player.Center.X, Player.position.Y, 0f, 0f, projType, 1, 0, Player.whoAmI, 0f, 0f);
				Projectile.NewProjectile(null, Player.Center.X, Player.position.Y, 0f, 0f, projType, 2, 0, Player.whoAmI, 0f, 0f);
			}

			bool newEmpowerment = false;
			int currentTimer = 0;
			int lowestDuration = 60 * shamanBuffTimer + 1;

			lowestDuration = (shamanFireTimer < lowestDuration) ? shamanFireTimer : lowestDuration;
			lowestDuration = (shamanWaterTimer < lowestDuration) ? shamanWaterTimer : lowestDuration;
			lowestDuration = (shamanAirTimer < lowestDuration) ? shamanAirTimer : lowestDuration;
			lowestDuration = (shamanEarthTimer < lowestDuration) ? shamanEarthTimer : lowestDuration;
			lowestDuration = (shamanSpiritTimer < lowestDuration) ? shamanSpiritTimer : lowestDuration;

			switch (type)
			{
				case 1:
					currentTimer = shamanFireTimer;
					break;
				case 2:
					currentTimer = shamanWaterTimer;
					break;
				case 3:
					currentTimer = shamanAirTimer;
					break;
				case 4:
					currentTimer = shamanEarthTimer;
					break;
				case 5:
					currentTimer = shamanSpiritTimer;
					break;
				default:
					return;
			}

			newEmpowerment = currentTimer == 0;

			if (newEmpowerment)
			{
				if (shamanDryad && GetNbShamanicBonds() > 0)
				{
					int lowestNoZeroDuation = 60 * shamanBuffTimer + 1;
					lowestNoZeroDuation = (shamanFireTimer != 0 && shamanFireTimer < lowestDuration) ? 0 + shamanFireTimer : lowestDuration;
					lowestNoZeroDuation = (shamanWaterTimer != 0 && shamanWaterTimer < lowestDuration) ? 0 + shamanWaterTimer : lowestDuration;
					lowestNoZeroDuation = (shamanAirTimer != 0 && shamanAirTimer < lowestDuration) ? 0 + shamanAirTimer : lowestDuration;
					lowestNoZeroDuation = (shamanEarthTimer != 0 && shamanEarthTimer < lowestDuration) ? 0 + shamanEarthTimer : lowestDuration;
					lowestNoZeroDuation = (shamanSpiritTimer != 0 && shamanSpiritTimer < lowestDuration) ? 0 + shamanSpiritTimer : lowestDuration;

					shamanFireTimer = (shamanFireTimer == lowestDuration) ? shamanFireTimer + 60 * 3 : shamanFireTimer;
					shamanWaterTimer = (shamanWaterTimer == lowestDuration) ? shamanWaterTimer + 60 * 3 : shamanWaterTimer;
					shamanAirTimer = (shamanAirTimer == lowestDuration) ? shamanAirTimer + 60 * 3 : shamanAirTimer;
					shamanEarthTimer = (shamanEarthTimer == lowestDuration) ? shamanEarthTimer + 60 * 3 : shamanEarthTimer;
					shamanSpiritTimer = (shamanSpiritTimer == lowestDuration) ? shamanSpiritTimer + 60 * 3 : shamanSpiritTimer;
				}
			}

			int maxBufftimer = 60 * shamanBuffTimer;
			int toAdd = (int)(maxBufftimer / (2 + shamanHitDelay));
			switch (type)
			{
				case 1:
					shamanFireTimer = shamanFireTimer + toAdd > maxBufftimer ? maxBufftimer : shamanFireTimer + toAdd;
					break;
				case 2:
					shamanWaterTimer = shamanWaterTimer + toAdd > maxBufftimer ? maxBufftimer : shamanWaterTimer + toAdd;
					break;
				case 3:
					shamanAirTimer = shamanAirTimer + toAdd > maxBufftimer ? maxBufftimer : shamanAirTimer + toAdd;
					break;
				case 4:
					shamanEarthTimer = shamanEarthTimer + toAdd > maxBufftimer ? maxBufftimer : shamanEarthTimer + toAdd;
					break;
				case 5:
					shamanSpiritTimer = shamanSpiritTimer + toAdd > maxBufftimer ? maxBufftimer : shamanSpiritTimer + toAdd;
					break;
				default:
					return;
			}
			shamanHitDelay = 8;
		}
	}
}
