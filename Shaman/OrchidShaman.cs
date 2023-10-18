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
using OrchidMod.Content.Items.Consumables;

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

		public float ShamanFireBond = 0;
		public float ShamanWaterBond = 0;
		public float ShamanAirBond = 0;
		public float ShamanEarthBond = 0;
		public float ShamanSpiritBond = 0;
		public int ShamanFireBondPoll = 0;
		public int ShamanWaterBondPoll = 0;
		public int ShamanAirBondPoll = 0;
		public int ShamanEarthBondPoll = 0;
		public int ShamanSpiritBondPoll = 0;
		public bool ShamanFireBondReleased = false;
		public bool ShamanWaterBondReleased = false;
		public bool ShamanAirBondReleased = false;
		public bool ShamanEarthBondReleased = false;
		public bool ShamanSpiritBondReleased = false;

		public int ShamanBondUnloadDelay = 300; // Non-combat delay after which shaman elements bars start unloading
		public float ShamanBondUnloadRate = 1f; // Shaman bond deplete speed multiplier (after the above delay ends)
		public float ShamanBondLoadRate = 1f; // Shaman bond loading multiplier when hitting

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
		public bool shamanHorus = false;

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
			(ShamanFireBond +
			ShamanWaterBond +
			ShamanAirBond +
			ShamanEarthBond +
			ShamanSpiritBond) > 0;

		public int GetDamage(int damage) => (int)Player.GetDamage<ShamanDamageClass>().ApplyTo(damage);

		public int GetNbShamanicBonds()
		{
			int val = 0;
			if (ShamanFireBondReleased) val++;
			if (ShamanWaterBondReleased) val++;
			if (ShamanAirBondReleased) val++;
			if (ShamanEarthBondReleased) val++;
			if (ShamanSpiritBondReleased) val++;
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

			ShamanFireBond = 0;
			ShamanFireBondPoll = 0;
			ShamanFireBondReleased = false;

			ShamanWaterBond = 0;
			ShamanWaterBondPoll = 0;
			ShamanWaterBondReleased = false;

			ShamanAirBond = 0;
			ShamanAirBondPoll = 0;
			ShamanAirBondReleased = false;

			ShamanEarthBond = 0;
			ShamanEarthBondPoll = 0;
			ShamanEarthBondReleased = false;

			ShamanSpiritBond = 0;
			ShamanSpiritBondPoll = 0;
			ShamanSpiritBondReleased = false;

			UIDisplayTimer = 0;
		}

		public override void Initialize()
		{
			modPlayer = Player.GetModPlayer<OrchidPlayer>();

			Reset();
		}

		public override void PostUpdateEquips()
		{
			if (HasAnyBondLoaded())
			{
				UIDisplayTimer = UIDisplayDelay;
			}

			if (ShamanFireBondReleased)
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

			if (ShamanWaterBondReleased)
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

			if (ShamanAirBondReleased)
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

			if (ShamanEarthBondReleased)
			{
				if (shamanHoney)
				{
					Player.AddBuff((48), 1); // Honey
					if (modPlayer.timer120 % 90 == 0) //temp
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

			if (ShamanSpiritBondReleased)
			{

				if (shamanAmethyst)
				{
					Player.GetDamage<ShamanDamageClass>() += 0.1f;
				}
			}

			if (doubleJumpHarpy)  // Vanilla double jump code is insanely weird.
			{
				if (!Player.controlJump) harpySpaceKeyReleased = true;

				if (!(Player.GetJumpState(ExtraJump.CloudInABottle).Enabled || Player.GetJumpState(ExtraJump.TsunamiInABottle).Enabled || Player.GetJumpState(ExtraJump.SandstormInABottle).Enabled
				|| Player.GetJumpState(ExtraJump.BlizzardInABottle).Enabled || Player.GetJumpState(ExtraJump.FartInAJar).Enabled || Player.GetJumpState(ExtraJump.UnicornMount).Enabled))
					Player.GetJumpState(ExtraJump.CloudInABottle).Enable();

				if (Player.velocity.Y == 0 || Player.grappling[0] >= 0 || (shamanHarpyAnklet && ShamanAirBondReleased && !Player.controlJump))
				{
					if (Player.GetJumpState(ExtraJump.CloudInABottle).Available)
					{
						jumpHeightCheck = (int)((double)Player.jumpHeight * 0.75);
					}
					if (Player.GetJumpState(ExtraJump.TsunamiInABottle).Available)
					{
						jumpHeightCheck = (int)((double)Player.jumpHeight * 1.25);
					}
					if (Player.GetJumpState(ExtraJump.FartInAJar).Available)
					{
						jumpHeightCheck = Player.jumpHeight * 2;
					}
					if (Player.GetJumpState(ExtraJump.BlizzardInABottle).Available)
					{
						jumpHeightCheck = (int)((double)Player.jumpHeight * 1.5);
					}
					if (Player.GetJumpState(ExtraJump.SandstormInABottle).Available)
					{
						jumpHeightCheck = Player.jumpHeight * 3;
					}
					if (Player.GetJumpState(ExtraJump.UnicornMount).Available)
					{
						jumpHeightCheck = Player.jumpHeight * 2;
					}
				}

				if (Player.GetJumpState(ExtraJump.CloudInABottle).Available && Player.jump == (int)((double)Player.jumpHeight * 0.75))
					Player.jump--;

				if ((Player.jump == jumpHeightCheck && harpySpaceKeyReleased == true))
				{
					harpySpaceKeyReleased = false;
					int dmg = 10;
					if (shamanHarpyAnklet && ShamanAirBondReleased)
					{
						dmg += (int)Player.GetDamage<ShamanDamageClass>().ApplyTo(12);
						if (Player.FindBuffIndex(ModContent.BuffType<HarpyPotionBuff>()) > -1)
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
			if (GetNbShamanicBonds() == 0)
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

		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
		{
			OrchidModGlobalProjectile modProjectile = proj.GetGlobalProjectile<OrchidModGlobalProjectile>();
			if (modProjectile.shamanProjectile)
			{
				if (shamanCrimtane && ShamanEarthBondReleased && shamanTimerCrimson == 30)
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
						Projectile.NewProjectile(Player.GetSource_OnHit(target), target.Center.X, target.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, type, 0, 0.0f, proj.owner, 0.0f, 0.0f);
					}
					else
					{
						int type = ProjectileType<Shaman.Projectiles.Thorium.Equipment.Viscount.ViscountSound>();
						Projectile.NewProjectile(Player.GetSource_OnHit(target), target.Center.X, target.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, type, 0, 0.0f, proj.owner, 0.0f, 0.0f);
					}
				}

				if (shamanHell && shamanTimerHellDamage == 600 && ShamanFireBondReleased)
				{
					shamanTimerHellDamage = 0;
					for (int i = 0; i < 10; i++)
					{
						Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(360));
						int dmg = GetDamage(50);
						int type = ProjectileType<Shaman.Projectiles.Equipment.Hell.ShamanHellHoming>();
						Projectile.NewProjectile(Player.GetSource_OnHit(target), target.Center.X, target.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, type, dmg, 0.0f, proj.owner, 0.0f, 0.0f);
					}
				}

				if (target.type != NPCID.TargetDummy)
				{
					if (proj.type != ModContent.ProjectileType<LavaDroplet>()
						&& proj.type != ModContent.ProjectileType<LostSoul>()
						&& proj.type != ModContent.ProjectileType<HarpyAnkletProj>()
						&& proj.type != ModContent.ProjectileType<DeepForestCharmProj>()
						&& proj.type != ModContent.ProjectileType<Smite>()
						)
					{
						if (ShamanFireBondReleased)
						{
							if (shamanPoison) target.AddBuff((20), 5 * 60);
							if (shamanVenom) target.AddBuff((70), 5 * 60);
							if (shamanFire) target.AddBuff((24), 5 * 60);
							if (shamanIce) target.AddBuff((44), 5 * 60);
							if (shamanDemonite) target.AddBuff(153, 20); // Shadowflame
						}

						if (hit.Crit == true && shamanSkull && ShamanWaterBondReleased)
						{
							int dmg = GetDamage(80);
							Vector2 mouseTarget = Main.MouseWorld;
							Vector2 heading = mouseTarget - Main.player[proj.owner].position;
							heading.Normalize();
							heading *= new Vector2(5f, 5f).Length();
							Vector2 projectileVelocity = (new Vector2(heading.X, heading.Y).RotatedByRandom(MathHelper.ToRadians(10)));
							Projectile.NewProjectile(null, Main.player[proj.owner].Center.X, Main.player[proj.owner].Center.Y, projectileVelocity.X, projectileVelocity.Y, ModContent.ProjectileType<LostSoul>(), dmg, 0f, proj.owner, 0f, 0f);
						}

						if (hit.Crit == true && shamanWaterHoney && ShamanWaterBondReleased && timerVial == 30)
						{
							timerVial = 0;
							Player.HealEffect(3, true);
							Player.statLife += 3;
						}

						if (Main.rand.NextBool(15) && shamanDownpour && GetNbShamanicBonds() > 2)
						{
							int dmg = (int)Player.GetDamage<ShamanDamageClass>().ApplyTo(50);
							Player.ApplyDamageToNPC(target, dmg, 0f, target.direction);
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

		public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)/* tModPorter If you don't need the Projectile, consider using ModifyHitNPC instead */
		{
			OrchidModGlobalProjectile modProjectile = proj.GetGlobalProjectile<OrchidModGlobalProjectile>();
			if (modProjectile.shamanProjectile)
			{
				if (Player.FindBuffIndex(ModContent.BuffType<OpalEmpowerment>()) > -1)
				{
					modifiers.CritDamage += 1.15f;
				}

				if (Player.FindBuffIndex(ModContent.BuffType<DestroyerFrenzy>()) > -1)
				{
					modifiers.CritDamage += 1.15f;
				}

				if (proj.type == ModContent.ProjectileType<TitanicScepterProj>())
				{
					modifiers.CritDamage += 1.5f;
				}

				if (Player.FindBuffIndex(ModContent.BuffType<TitanicBuff>()) > -1)
				{
					modifiers.CritDamage += 1.2f;
				}

				if (shamanDestroyer && ShamanWaterBondReleased)
				{
					shamanTimerDestroyer = 60;
					shamanDestroyerCount++;
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

		public override void ModifyHurt(ref Player.HurtModifiers modifiers)/* tModPorter Override ImmuneTo, FreeDodge or ConsumableDodge instead to prevent taking damage */
		{
			/*
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
			*/

			if (shamanSunBelt)
			{
				Player.AddBuff(ModContent.BuffType<BrokenPower>(), 60 * 15);
			}

			if (shamanHell && shamanTimerHellDefense == 300 && ShamanEarthBondReleased)
			{
				shamanTimerHellDefense = 0;
				int dmg = (int)Player.GetDamage<ShamanDamageClass>().ApplyTo(50);
				Projectile.NewProjectile(null, Player.Center.X, Player.Center.Y, 0f, 0f, ProjectileType<Shaman.Projectiles.Equipment.Hell.ShamanHellExplosion>(), dmg, 0.0f, Player.whoAmI, 0.0f, 0.0f);
				SoundEngine.PlaySound(SoundID.Item14);
				OrchidModProjectile.spawnDustCircle(Player.Center, 6, 10, 15, true, 1f, 1f, 8f, true, true, false, 0, 0, true);
				OrchidModProjectile.spawnDustCircle(Player.Center, 6, 10, 15, true, 1.5f, 1f, 5f, true, true, false, 0, 0, true);
				OrchidModProjectile.spawnDustCircle(Player.Center, 6, 10, 15, true, 2f, 1f, 3f, true, true, false, 0, 0, true);
			}
		}

		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
		{
			if (shamanDiabolist && ShamanEarthBondReleased)
			{
				shamanTimerDiabolist = 60;
				shamanDiabolistCount += hurtInfo.Damage;
			}
		}

		public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
		{
			if (shamanDiabolist && ShamanEarthBondReleased)
			{
				shamanTimerDiabolist = 60;
				shamanDiabolistCount += hurtInfo.Damage;
			}
		}


		public override void ResetEffects()
		{
			if (GetNbShamanicBonds() == 0 && ShamanFireBond + ShamanWaterBond + ShamanAirBond + ShamanEarthBond + ShamanSpiritBond == 0)
			{
				UIDisplayTimer -= UIDisplayTimer > 0 ? 1 : 0;
			}
			else
			{
				UIDisplayTimer = UIDisplayDelay;
			}

			if (UIDisplayTimer == 0)
			{
				ShamanFireBond = 0;
				ShamanWaterBond = 0;
				ShamanAirBond = 0;
				ShamanEarthBond = 0;
				ShamanSpiritBond = 0;
			}

			ShamanFireBond += ShamanFireBondPoll > 0 && ShamanFireBond < 100 ? 1f * ShamanBondLoadRate : ShamanFireBondPoll < -ShamanBondUnloadDelay && ShamanFireBond > 0 && modPlayer.timer120 % 6 == 0 ? -1f * ShamanBondUnloadRate : 0;
			ShamanWaterBond += ShamanWaterBondPoll > 0 && ShamanWaterBond < 100 ? 1f * ShamanBondLoadRate : ShamanWaterBondPoll < -ShamanBondUnloadDelay && ShamanWaterBond > 0 && modPlayer.timer120 % 6 == 0 ? -1f * ShamanBondUnloadRate : 0;
			ShamanAirBond += ShamanAirBondPoll > 0 && ShamanAirBond < 100 ? 1f * ShamanBondLoadRate : ShamanAirBondPoll < -ShamanBondUnloadDelay && ShamanAirBond > 0 && modPlayer.timer120 % 6 == 0 ? -1f * ShamanBondUnloadRate : 0;
			ShamanEarthBond += ShamanEarthBondPoll > 0 && ShamanEarthBond < 100 ? 1f * ShamanBondLoadRate : ShamanEarthBondPoll < -ShamanBondUnloadDelay && ShamanEarthBond > 0 && modPlayer.timer120 % 6 == 0 ? -1f * ShamanBondUnloadRate : 0;
			ShamanSpiritBond += ShamanSpiritBondPoll > 0 && ShamanSpiritBond < 100 ? 1f * ShamanBondLoadRate : ShamanSpiritBondPoll < -ShamanBondUnloadDelay && ShamanSpiritBond > 0 && modPlayer.timer120 % 6 == 0 ? -1f * ShamanBondUnloadRate : 0;
			if (ShamanFireBond == 0) ShamanFireBondReleased = false;
			if (ShamanWaterBond == 0) ShamanWaterBondReleased = false;
			if (ShamanAirBond == 0) ShamanAirBondReleased = false;
			if (ShamanEarthBond == 0) ShamanEarthBondReleased = false;
			if (ShamanSpiritBond == 0) ShamanSpiritBondReleased = false;
			ShamanFireBondPoll--;
			ShamanWaterBondPoll--;
			ShamanAirBondPoll--;
			ShamanEarthBondPoll--;
			ShamanSpiritBondPoll--;
			ShamanBondUnloadDelay = 300;
			ShamanBondUnloadRate = 1f;
			ShamanBondLoadRate = 1f;

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
			shamanHorus = false;

			shamanShadowEmpowerment = false;
			shamanMourningTorch = false;
			shamanSunBelt = false;

			shamanHitDelay += shamanHitDelay < 900 ? 1 : 0;
		}

		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
			Reset();
		}

		public override void OnRespawn()
		{
			Reset();
		}

		public float GetShamanicBondValue(ShamanElement element)
		{
			switch (element)
			{
				case ShamanElement.FIRE:
					return ShamanFireBond;
				case ShamanElement.WATER:
					return ShamanWaterBond;
				case ShamanElement.AIR:
					return ShamanAirBond;
				case ShamanElement.EARTH:
					return ShamanEarthBond;
				case ShamanElement.SPIRIT:
					return ShamanSpiritBond;
				default:
					return 0;
			}
		}

		public bool IsShamanicBondReleased(ShamanElement element)
		{
			switch (element)
			{
				case ShamanElement.FIRE:
					return ShamanFireBondReleased;
				case ShamanElement.WATER:
					return ShamanWaterBondReleased;
				case ShamanElement.AIR:
					return ShamanAirBondReleased;
				case ShamanElement.EARTH:
					return ShamanEarthBondReleased;
				case ShamanElement.SPIRIT:
					return ShamanAirBondReleased;
				default:
					return false;
			}
		}

		public void ReleaseShamanicBond(OrchidModShamanItem item)
		{
			ShamanElement element = (ShamanElement)item.Element;
			switch (element)
			{
				case ShamanElement.FIRE:
					ShamanFireBondReleased = true;
					ShamanFireBondPoll = 0;
					break;
				case ShamanElement.WATER:
					ShamanWaterBondReleased = true;
					ShamanWaterBondPoll = 0;
					break;
				case ShamanElement.AIR:
					ShamanAirBondReleased = true;
					ShamanAirBondPoll = 0;
					break;
				case ShamanElement.EARTH:
					ShamanEarthBondReleased = true;
					ShamanEarthBondPoll = 0;
					break;
				case ShamanElement.SPIRIT:
					ShamanSpiritBondReleased = true;
					ShamanSpiritBondPoll = 0;
					break;
				default:
					break;
			}
		}

		public void AddShamanicEmpowerment(int type)
		{
			if (type == 0)
			{
				return;
			}

			if (shamanForest && type == 4)
			{
				Player.AddBuff(BuffType<DeepForestAura>(), 1);
				int projType = ProjectileType<DeepForestCharmProj>();
				Projectile.NewProjectile(null, Player.Center.X, Player.position.Y, 0f, 0f, projType, 1, 0, Player.whoAmI, 0f, 0f);
				Projectile.NewProjectile(null, Player.Center.X, Player.position.Y, 0f, 0f, projType, 2, 0, Player.whoAmI, 0f, 0f);
			}

			int toAdd = shamanHitDelay > 36 ? (int)(shamanHitDelay / 18f) : 2;
			switch (type)
			{
				case 1:
					if (!ShamanFireBondReleased)
					{
						ShamanFireBondPoll = ShamanFireBondPoll < 0 ? 0 : ShamanFireBondPoll;
						ShamanFireBondPoll += toAdd;
					}
					break;
				case 2:
					if (!ShamanWaterBondReleased)
					{
						ShamanWaterBondPoll = ShamanWaterBondPoll < 0 ? 0 : ShamanWaterBondPoll;
						ShamanWaterBondPoll += toAdd;
					}
					break;
				case 3:
					if (!ShamanAirBondReleased)
					{
						ShamanAirBondPoll = ShamanAirBondPoll < 0 ? 0 : ShamanAirBondPoll;
						ShamanAirBondPoll += toAdd;
					}
					break;
				case 4:
					if (!ShamanEarthBondReleased)
					{
						ShamanEarthBondPoll = ShamanEarthBondPoll < 0 ? 0 : ShamanEarthBondPoll;
						ShamanEarthBondPoll += toAdd;
					}
					break;
				case 5:
					if (!ShamanSpiritBondReleased)
					{
						ShamanSpiritBondPoll = ShamanSpiritBondPoll < 0 ? 0 : ShamanSpiritBondPoll;
						ShamanSpiritBondPoll += toAdd;
					}
					break;
				default:
					return;
			}

			shamanHitDelay = 0;
		}
	}
}
