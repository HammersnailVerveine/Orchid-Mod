using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Alchemist.Weapons.Air;
using OrchidMod.Alchemist.Weapons.Fire;
using OrchidMod.Alchemist.Weapons.Water;
using OrchidMod.Common.Globals.NPCs;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles
{
	public class AlchemistProj : OrchidModAlchemistProjectile
	{
		public int nbElements = 0;
		public Item waterFlask = new Item();
		public Item fireFlask = new Item();
		public Item natureFlask = new Item();
		public Item airFlask = new Item();
		public Item lightFlask = new Item();
		public Item darkFlask = new Item();
		public OrchidModGlobalItem waterFlaskGlobal = null;
		public OrchidModGlobalItem fireFlaskGlobal = null;
		public OrchidModGlobalItem natureFlaskGlobal = null;
		public OrchidModGlobalItem airFlaskGlobal = null;
		public OrchidModGlobalItem lightFlaskGlobal = null;
		public OrchidModGlobalItem darkFlaskGlobal = null;
		public Color glowColor = new Color(0, 0, 0);
		public bool hitNPC = false;

		public bool noCatalyticSpawn = false;
		public int nbElementsNoExtract = 0;

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.aiStyle = 2;
			Projectile.timeLeft = 300;
			Projectile.scale = 1f;
			Projectile.penetrate = 2;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alchemical Solution");
		}

		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = ModContent.Request<Texture2D>("OrchidMod/Alchemist/Projectiles/AlchemistProj_Glow").Value;
			OrchidModProjectile.DrawProjectileGlowmask(Projectile, Main.spriteBatch, texture, glowColor);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.penetrate--;
			return false;
		}

		public override void AI()
		{
			if (Projectile.penetrate == 1)
			{
				Vector2 oldCenter = new Vector2(Projectile.Center.X, Projectile.Center.Y);
				//projectile.tileCollide = false;
				Projectile.penetrate = -1;
				Projectile.width = 48;
				Projectile.height = 48;
				Projectile.timeLeft = 1;
				Projectile.alpha = 255;
				Projectile.Center = oldCenter;
			}

			if (!this.initialized)
			{
				this.initializeAlchemistProjectile();
				this.initialized = true;
			}

			if (!this.hitNPC) OrchidModProjectile.resetIFrames(Projectile);

			if (this.waterFlaskGlobal != null)
			{
				if (Main.rand.Next(8) > this.nbElements)
				{
					int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, this.waterFlaskGlobal.alchemistRightClickDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}

			if (this.fireFlaskGlobal != null)
			{
				if (Main.rand.Next(8) > this.nbElements)
				{
					int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, this.fireFlaskGlobal.alchemistRightClickDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}

			if (this.natureFlaskGlobal != null)
			{
				if (Main.rand.Next(8) > this.nbElements)
				{
					int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, this.natureFlaskGlobal.alchemistRightClickDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}

			if (this.airFlaskGlobal != null)
			{
				if (Main.rand.Next(8) > this.nbElements)
				{
					int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, this.airFlaskGlobal.alchemistRightClickDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}

			if (this.lightFlaskGlobal != null)
			{
				if (Main.rand.Next(8) > this.nbElements)
				{
					int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, this.lightFlaskGlobal.alchemistRightClickDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}

			if (this.darkFlaskGlobal != null)
			{
				if (Main.rand.Next(8) > this.nbElements)
				{
					int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, this.darkFlaskGlobal.alchemistRightClickDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}
		}

		public void initializeAlchemistProjectile()
		{
			Player player = Main.player[Projectile.owner];
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			bool[] elements = modPlayer.alchemistElements;
			Item[] flasks = modPlayer.alchemistFlasks;

			this.projOwner = (Projectile.owner == Main.myPlayer);

			this.nbElements = modPlayer.alchemistNbElements;
			Projectile.damage = (int)(modPlayer.alchemistFlaskDamage * modPlayer.alchemistDamage + 5E-06f);

			if (elements[0])
			{
				this.waterFlask = flasks[0];
				this.waterFlaskGlobal = this.waterFlask.GetGlobalItem<OrchidModGlobalItem>();
			}

			if (elements[1])
			{
				this.fireFlask = flasks[1];
				this.fireFlaskGlobal = this.fireFlask.GetGlobalItem<OrchidModGlobalItem>();
			}

			if (elements[2])
			{
				this.natureFlask = flasks[2];
				this.natureFlaskGlobal = this.natureFlask.GetGlobalItem<OrchidModGlobalItem>();
			}

			if (elements[3])
			{
				this.airFlask = flasks[3];
				this.airFlaskGlobal = this.airFlask.GetGlobalItem<OrchidModGlobalItem>();
			}

			if (elements[4])
			{
				this.lightFlask = flasks[4];
				this.lightFlaskGlobal = this.lightFlask.GetGlobalItem<OrchidModGlobalItem>();
			}

			if (elements[5])
			{
				this.darkFlask = flasks[5];
				this.darkFlaskGlobal = this.darkFlask.GetGlobalItem<OrchidModGlobalItem>();
			}

			this.addVariousEffects();
			this.glowColor = new Color(modPlayer.alchemistColorR, modPlayer.alchemistColorG, modPlayer.alchemistColorB);

			modPlayer.alchemistFlaskDamage = 0;
			modPlayer.alchemistNbElements = 0;
			modPlayer.ClearAlchemistElements();
			modPlayer.ClearAlchemistFlasks();
			modPlayer.ClearAlchemistColors();

			if (this.fireFlask.type == ItemType<GunpowderFlask>())
			{
				this.noCatalyticSpawn = true;
			}

			if (this.nbElements > 2)
			{
				if (modPlayer.alchemistMeteor)
				{
					player.AddBuff((BuffType<Alchemist.Buffs.MeteorSpeed>()), 60 * 10);
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();
			modPlayer.alchemistLastAttackDelay = 0;

			int soundNb = this.nbElements == 1 ? 1 : this.nbElements == 2 ? 2 : 3;
			switch (soundNb)
			{
				case 1:
					SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);
					break;
				default:
					SoundEngine.PlaySound(SoundID.Item107, Projectile.Center);
					break;
			}

			this.spawnKillDusts(timeLeft);

			if (this.projOwner)
			{
				this.KillFirst(timeLeft, player, modPlayer);
				this.KillSecond(timeLeft, player, modPlayer);
				this.KillThird(timeLeft, player, modPlayer);
			}
		}

		public override void SafeOnHitNPC(NPC target, OrchidModAlchemistNPC modTarget, int damage, float knockback, bool crit, Player player, OrchidAlchemist modPlayer)
		{
			if (this.projOwner)
			{
				if (modPlayer.alchemistCovent && this.nbElements > 2 && this.airFlaskGlobal == null) { // Keystone of the Convent
					float distance = 500f;
					bool keystoneCrit = (Main.rand.Next(101) <= modPlayer.alchemistCrit + 4);
					int keystoneDamage = (int)(22 * (modPlayer.alchemistDamage - 1f));
					//int keystoneDamage = (int)(22 * (modPlayer.alchemistDamage + player.allDamage - 1f));
					int absorbedCount = 0;
					int spawnProj = ProjectileType<Alchemist.Projectiles.Air.KeystoneOfTheConventProj>();
					for (int k = 0; k < Main.maxNPCs ; k++)
					{
						if (Main.npc[k].active)
						{
							Vector2 newMove = Main.npc[k].Center - target.Center;
							float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
							if (distanceTo < distance)
							{
								OrchidModAlchemistNPC modTargetSecondary = Main.npc[k].GetGlobalNPC<OrchidModAlchemistNPC>();
								if (modTargetSecondary.alchemistAir > 0)
								{
									modTargetSecondary.alchemistAir = 0;
									absorbedCount ++;
									newMove /= -10f;
									Projectile.NewProjectile(target.GetSource_OnHit(target), Main.npc[k].Center.X, Main.npc[k].Center.Y, newMove.X, newMove.Y, spawnProj, 0, 0f, Projectile.owner);
								}
							}
						}
					}
					player.ApplyDamageToNPC(target, Main.DamageVar(keystoneDamage * absorbedCount), 0f, player.direction, keystoneCrit);
				}
				
				this.hitNPC = true;
				OrchidGlobalNPC modTargetGlobal = target.GetGlobalNPC<OrchidGlobalNPC>();

				this.OnHitNPCFirst(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal);
				this.OnHitNPCSecond(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal);
				this.OnHitNPCThird(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal);
			}
		}

		public void KillFirst(int timeLeft, Player player, OrchidAlchemist modPlayer)
		{
			if (this.fireFlaskGlobal != null)
			{
				fireFlaskGlobal.killFirstDelegate(timeLeft, player, modPlayer, this, Projectile, this.fireFlaskGlobal);
			}

			if (this.waterFlaskGlobal != null)
			{
				waterFlaskGlobal.killFirstDelegate(timeLeft, player, modPlayer, this, Projectile, this.waterFlaskGlobal);
			}

			if (this.natureFlaskGlobal != null)
			{
				natureFlaskGlobal.killFirstDelegate(timeLeft, player, modPlayer, this, Projectile, this.natureFlaskGlobal);
			}

			if (this.airFlaskGlobal != null)
			{
				airFlaskGlobal.killFirstDelegate(timeLeft, player, modPlayer, this, Projectile, this.airFlaskGlobal);
			}

			if (this.lightFlaskGlobal != null)
			{
				lightFlaskGlobal.killFirstDelegate(timeLeft, player, modPlayer, this, Projectile, this.lightFlaskGlobal);
			}

			if (this.darkFlaskGlobal != null)
			{
				darkFlaskGlobal.killFirstDelegate(timeLeft, player, modPlayer, this, Projectile, this.darkFlaskGlobal);
			}

			if (this.nbElements > 1 && player.HasBuff(BuffType<Alchemist.Buffs.JungleLilyExtractBuff>()))
			{
				int spawnProj = ProjectileType<Nature.JungleLilyFlaskReaction>();
				Projectile.NewProjectile(null, Projectile.Center.X, Projectile.Center.Y, 0f, 0f, spawnProj, 0, 0f, Projectile.owner);
				OrchidModProjectile.spawnDustCircle(Projectile.Center, 15, 10, 7, true, 1.5f, 1f, 3f);
				OrchidModProjectile.spawnDustCircle(Projectile.Center, 15, 15, 10, true, 1.5f, 1f, 5f);
			}
		}

		public void KillSecond(int timeLeft, Player player, OrchidAlchemist modPlayer)
		{
			if (this.fireFlaskGlobal != null)
			{
				fireFlaskGlobal.killSecondDelegate(timeLeft, player, modPlayer, this, Projectile, this.fireFlaskGlobal);
			}

			if (this.waterFlaskGlobal != null)
			{
				waterFlaskGlobal.killSecondDelegate(timeLeft, player, modPlayer, this, Projectile, this.waterFlaskGlobal);
			}

			if (this.natureFlaskGlobal != null)
			{
				natureFlaskGlobal.killSecondDelegate(timeLeft, player, modPlayer, this, Projectile, this.natureFlaskGlobal);
			}

			if (this.airFlaskGlobal != null)
			{
				airFlaskGlobal.killSecondDelegate(timeLeft, player, modPlayer, this, Projectile, this.airFlaskGlobal);
			}

			if (this.lightFlaskGlobal != null)
			{
				lightFlaskGlobal.killSecondDelegate(timeLeft, player, modPlayer, this, Projectile, this.lightFlaskGlobal);
			}

			if (this.darkFlaskGlobal != null)
			{
				darkFlaskGlobal.killSecondDelegate(timeLeft, player, modPlayer, this, Projectile, this.darkFlaskGlobal);
			}

			if (this.nbElements > 2 && player.HasBuff(BuffType<Alchemist.Buffs.QueenBeeFlaskBuff>()))
			{
				int itemType = ItemType<QueenBeeFlask>();
				int dmg = modPlayer.GetSecondaryDamage(itemType, this.nbElements, true);
				int rand = this.nbElements + Main.rand.Next(3);
				for (int i = 0; i < rand; i++)
				{
					Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(80)));
					if (player.strongBees && Main.rand.NextBool(2))
						Projectile.NewProjectile(null, player.Center.X, player.Center.Y, vel.X, vel.Y, 566, (int)(dmg * 1.15f), 0f, player.whoAmI, 0f, 0f);
					else
					{
						Projectile.NewProjectile(null, player.Center.X, player.Center.Y, vel.X, vel.Y, 181, dmg, 0f, player.whoAmI, 0f, 0f);
					}
				}
			}

			if (player.HasBuff(BuffType<Alchemist.Buffs.SpiritedWaterBuff>()))
			{
				int spawnProj = ProjectileType<Alchemist.Projectiles.Water.DungeonFlaskProj>();
				int itemType = ItemType<DungeonFlask>();
				int dmg = modPlayer.GetSecondaryDamage(itemType, this.nbElements, true);
				for (int i = 0; i < this.nbElements; i++)
				{
					Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
					Projectile.NewProjectile(null, Projectile.Center.X, Projectile.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, Projectile.owner);
				}
			}
		}

		public void KillThird(int timeLeft, Player player, OrchidAlchemist modPlayer)
		{
			if (this.fireFlaskGlobal != null)
			{
				fireFlaskGlobal.killThirdDelegate(timeLeft, player, modPlayer, this, Projectile, this.fireFlaskGlobal);
			}

			if (this.waterFlaskGlobal != null)
			{
				waterFlaskGlobal.killThirdDelegate(timeLeft, player, modPlayer, this, Projectile, this.waterFlaskGlobal);
			}

			if (this.natureFlaskGlobal != null)
			{
				natureFlaskGlobal.killThirdDelegate(timeLeft, player, modPlayer, this, Projectile, this.natureFlaskGlobal);
			}

			if (this.airFlaskGlobal != null)
			{
				airFlaskGlobal.killThirdDelegate(timeLeft, player, modPlayer, this, Projectile, this.airFlaskGlobal);
			}

			if (this.lightFlaskGlobal != null)
			{
				lightFlaskGlobal.killThirdDelegate(timeLeft, player, modPlayer, this, Projectile, this.lightFlaskGlobal);
			}

			if (this.darkFlaskGlobal != null)
			{
				darkFlaskGlobal.killThirdDelegate(timeLeft, player, modPlayer, this, Projectile, this.darkFlaskGlobal);
			}
		}

		public void OnHitNPCFirst(NPC target, int damage, float knockback, bool crit, Player player, OrchidAlchemist modPlayer, OrchidModAlchemistNPC modTarget, OrchidGlobalNPC modTargetGlobal)
		{
			if (this.fireFlaskGlobal != null)
			{
				fireFlaskGlobal.onHitNPCFirstDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, Projectile, this.fireFlaskGlobal);
			}

			if (this.waterFlaskGlobal != null)
			{
				waterFlaskGlobal.onHitNPCFirstDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, Projectile, this.waterFlaskGlobal);
			}

			if (this.natureFlaskGlobal != null)
			{
				natureFlaskGlobal.onHitNPCFirstDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, Projectile, this.natureFlaskGlobal);
			}

			if (this.airFlaskGlobal != null)
			{
				airFlaskGlobal.onHitNPCFirstDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, Projectile, this.airFlaskGlobal);
			}

			if (this.lightFlaskGlobal != null)
			{
				lightFlaskGlobal.onHitNPCFirstDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, Projectile, this.lightFlaskGlobal);
			}

			if (this.darkFlaskGlobal != null)
			{
				darkFlaskGlobal.onHitNPCFirstDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, Projectile, this.darkFlaskGlobal);
			}
		}

		public void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidAlchemist modPlayer, OrchidModAlchemistNPC modTarget, OrchidGlobalNPC modTargetGlobal)
		{
			if (this.fireFlaskGlobal != null)
			{
				fireFlaskGlobal.onHitNPCSecondDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, Projectile, this.fireFlaskGlobal);
			}

			if (this.waterFlaskGlobal != null)
			{
				waterFlaskGlobal.onHitNPCSecondDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, Projectile, this.waterFlaskGlobal);
			}

			if (this.natureFlaskGlobal != null)
			{
				natureFlaskGlobal.onHitNPCSecondDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, Projectile, this.natureFlaskGlobal);
			}

			if (this.airFlaskGlobal != null)
			{
				airFlaskGlobal.onHitNPCSecondDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, Projectile, this.airFlaskGlobal);
			}

			if (this.lightFlaskGlobal != null)
			{
				lightFlaskGlobal.onHitNPCSecondDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, Projectile, this.lightFlaskGlobal);
			}

			if (this.darkFlaskGlobal != null)
			{
				darkFlaskGlobal.onHitNPCSecondDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, Projectile, this.darkFlaskGlobal);
			}
		}

		public void OnHitNPCThird(NPC target, int damage, float knockback, bool crit, Player player, OrchidAlchemist modPlayer, OrchidModAlchemistNPC modTarget, OrchidGlobalNPC modTargetGlobal)
		{
			if (this.fireFlaskGlobal != null)
			{
				fireFlaskGlobal.onHitNPCThirdDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, Projectile, this.fireFlaskGlobal);
			}

			if (this.waterFlaskGlobal != null)
			{
				waterFlaskGlobal.onHitNPCThirdDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, Projectile, this.waterFlaskGlobal);
			}

			if (this.natureFlaskGlobal != null)
			{
				natureFlaskGlobal.onHitNPCThirdDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, Projectile, this.natureFlaskGlobal);
			}

			if (this.airFlaskGlobal != null)
			{
				airFlaskGlobal.onHitNPCThirdDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, Projectile, this.airFlaskGlobal);
			}

			if (this.lightFlaskGlobal != null)
			{
				lightFlaskGlobal.onHitNPCThirdDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, Projectile, this.lightFlaskGlobal);
			}

			if (this.darkFlaskGlobal != null)
			{
				darkFlaskGlobal.onHitNPCThirdDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, Projectile, this.darkFlaskGlobal);
			}
		}

		public void addVariousEffects()
		{
			Player player = Main.player[Projectile.owner];
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();

			this.nbElementsNoExtract += this.nbElements;

			int buffType = BuffType<Alchemist.Buffs.ReactiveVialsBuff>();
			if (player.HasBuff(buffType))
			{
				Projectile.damage = (int)(Projectile.damage * 1.1f);
				player.ClearBuff(buffType);
			}

			if (this.fireFlaskGlobal != null)
			{
				fireFlaskGlobal.addVariousEffectsDelegate(player, modPlayer, this, Projectile, this.fireFlaskGlobal);
			}

			if (this.waterFlaskGlobal != null)
			{
				waterFlaskGlobal.addVariousEffectsDelegate(player, modPlayer, this, Projectile, this.fireFlaskGlobal);
			}

			if (this.natureFlaskGlobal != null)
			{
				natureFlaskGlobal.addVariousEffectsDelegate(player, modPlayer, this, Projectile, this.fireFlaskGlobal);
			}

			if (this.airFlaskGlobal != null)
			{
				airFlaskGlobal.addVariousEffectsDelegate(player, modPlayer, this, Projectile, this.fireFlaskGlobal);
			}

			if (this.lightFlaskGlobal != null)
			{
				lightFlaskGlobal.addVariousEffectsDelegate(player, modPlayer, this, Projectile, this.fireFlaskGlobal);
			}

			if (this.darkFlaskGlobal != null)
			{
				darkFlaskGlobal.addVariousEffectsDelegate(player, modPlayer, this, Projectile, this.fireFlaskGlobal);
			}
		}

		public void spawnKillDusts(int timeLeft)
		{
			int rand = this.nbElements + Main.rand.Next(this.nbElements * 2);
			for (int i = 0; i < rand; i++)
			{
				int proj = ProjectileType<Alchemist.Projectiles.AlchemistSmoke1>();
				Vector2 vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj = Projectile.NewProjectile(null, Projectile.Center.X, Projectile.Center.Y, vel.X, vel.Y, proj, 0, 0f, Projectile.owner);
				Main.projectile[smokeProj].localAI[0] = this.glowColor.R;
				Main.projectile[smokeProj].localAI[1] = this.glowColor.G;
				Main.projectile[smokeProj].ai[1] = this.glowColor.B;
			}
			rand = 1 + Main.rand.Next(this.nbElements);
			for (int i = 0; i < rand; i++)
			{
				int proj = ProjectileType<Alchemist.Projectiles.AlchemistSmoke2>();
				Vector2 vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj = Projectile.NewProjectile(null, Projectile.Center.X, Projectile.Center.Y, vel.X, vel.Y, proj, 0, 0f, Projectile.owner);
				Main.projectile[smokeProj].localAI[0] = this.glowColor.R;
				Main.projectile[smokeProj].localAI[1] = this.glowColor.G;
				Main.projectile[smokeProj].ai[1] = this.glowColor.B;
			}
			rand = Main.rand.Next(this.nbElements);
			for (int i = 0; i < rand; i++)
			{
				int proj = ProjectileType<Alchemist.Projectiles.AlchemistSmoke3>();
				Vector2 vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj = Projectile.NewProjectile(null, Projectile.Center.X, Projectile.Center.Y, vel.X, vel.Y, proj, 0, 0f, Projectile.owner);
				Main.projectile[smokeProj].localAI[0] = this.glowColor.R;
				Main.projectile[smokeProj].localAI[1] = this.glowColor.G;
				Main.projectile[smokeProj].ai[1] = this.glowColor.B;
			}

			if (this.waterFlaskGlobal != null)
			{
				for (int i = 0; i < 8 - this.nbElements; i++)
				{
					if (Main.rand.Next(3) < 2)
					{
						int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, this.waterFlaskGlobal.alchemistRightClickDust);
						Main.dust[dust].velocity *= 3f;
						Main.dust[dust].scale *= 1.2f;
					}
				}
			}

			if (this.fireFlaskGlobal != null)
			{
				for (int i = 0; i < 8 - this.nbElements; i++)
				{
					if (Main.rand.Next(3) < 2)
					{
						int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, this.fireFlaskGlobal.alchemistRightClickDust);
						Main.dust[dust].velocity *= 3f;
						Main.dust[dust].scale *= 1.2f;
					}
				}
			}

			if (this.natureFlaskGlobal != null)
			{
				for (int i = 0; i < 8 - this.nbElements; i++)
				{
					if (Main.rand.Next(3) < 2)
					{
						int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, this.natureFlaskGlobal.alchemistRightClickDust);
						Main.dust[dust].velocity *= 3f;
						Main.dust[dust].scale *= 1.2f;
					}
				}
			}

			if (this.airFlaskGlobal != null)
			{
				for (int i = 0; i < 8 - this.nbElements; i++)
				{
					if (Main.rand.Next(3) < 2)
					{
						int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, this.airFlaskGlobal.alchemistRightClickDust);
						Main.dust[dust].velocity *= 3f;
						Main.dust[dust].scale *= 1.2f;
					}
				}
			}

			if (this.lightFlaskGlobal != null)
			{
				for (int i = 0; i < 8 - this.nbElements; i++)
				{
					if (Main.rand.Next(3) < 2)
					{
						int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, this.lightFlaskGlobal.alchemistRightClickDust);
						Main.dust[dust].velocity *= 3f;
						Main.dust[dust].scale *= 1.2f;
					}
				}
			}

			if (this.darkFlaskGlobal != null)
			{
				for (int i = 0; i < 8 - this.nbElements; i++)
				{
					if (Main.rand.Next(3) < 2)
					{
						int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, this.darkFlaskGlobal.alchemistRightClickDust);
						Main.dust[dust].velocity *= 3f;
						Main.dust[dust].scale *= 1.2f;
					}
				}
			}
		}

		public bool hasCloud()
		{
			return (this.airFlask.type == ItemType<CloudInAVial>() || this.airFlask.type == ItemType<FartInAVial>() || this.airFlask.type == ItemType<BlizzardInAVial>());
		}
	}
}