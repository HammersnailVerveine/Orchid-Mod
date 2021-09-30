using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Alchemist.Weapons.Air;
using OrchidMod.Alchemist.Weapons.Fire;
using OrchidMod.Alchemist.Weapons.Water;
using Terraria;
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
			projectile.width = 14;
			projectile.height = 18;
			projectile.friendly = true;
			projectile.aiStyle = 2;
			projectile.timeLeft = 300;
			projectile.scale = 1f;
			projectile.penetrate = 2;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alchemical Solution");
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = ModContent.GetTexture("OrchidMod/Alchemist/Projectiles/AlchemistProj_Glow");
			OrchidModProjectile.DrawProjectileGlowmask(projectile, spriteBatch, texture, glowColor);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.penetrate--;
			return false;
		}

		public override void AI()
		{
			if (projectile.penetrate == 1)
			{
				Vector2 oldCenter = new Vector2(projectile.Center.X, projectile.Center.Y);
				//projectile.tileCollide = false;
				projectile.penetrate = -1;
				projectile.width = 48;
				projectile.height = 48;
				projectile.timeLeft = 1;
				projectile.alpha = 255;
				projectile.Center = oldCenter;
			}

			if (!this.initialized)
			{
				this.initializeAlchemistProjectile();
				this.initialized = true;
			}

			if (!this.hitNPC) OrchidModProjectile.resetIFrames(projectile);

			if (this.waterFlaskGlobal != null)
			{
				if (Main.rand.Next(8) > this.nbElements)
				{
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.waterFlaskGlobal.alchemistRightClickDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}

			if (this.fireFlaskGlobal != null)
			{
				if (Main.rand.Next(8) > this.nbElements)
				{
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.fireFlaskGlobal.alchemistRightClickDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}

			if (this.natureFlaskGlobal != null)
			{
				if (Main.rand.Next(8) > this.nbElements)
				{
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.natureFlaskGlobal.alchemistRightClickDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}

			if (this.airFlaskGlobal != null)
			{
				if (Main.rand.Next(8) > this.nbElements)
				{
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.airFlaskGlobal.alchemistRightClickDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}

			if (this.lightFlaskGlobal != null)
			{
				if (Main.rand.Next(8) > this.nbElements)
				{
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.lightFlaskGlobal.alchemistRightClickDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}

			if (this.darkFlaskGlobal != null)
			{
				if (Main.rand.Next(8) > this.nbElements)
				{
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.darkFlaskGlobal.alchemistRightClickDust);
					Main.dust[dust].velocity /= 3f;
					Main.dust[dust].scale *= 1.3f;
					Main.dust[dust].noGravity = true;
				}
			}
		}

		public void initializeAlchemistProjectile()
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			bool[] elements = modPlayer.alchemistElements;
			Item[] flasks = modPlayer.alchemistFlasks;

			this.projOwner = (projectile.owner == Main.myPlayer);

			this.nbElements = modPlayer.alchemistNbElements;
			projectile.damage = (int)(modPlayer.alchemistFlaskDamage * modPlayer.alchemistDamage + 5E-06f);

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
			OrchidModAlchemistHelper.clearAlchemistElements(player, modPlayer, mod);
			OrchidModAlchemistHelper.clearAlchemistFlasks(player, modPlayer, mod);
			OrchidModAlchemistHelper.clearAlchemistColors(player, modPlayer, mod);

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
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistLastAttackDelay = 0;

			int soundNb = this.nbElements == 1 ? 1 : this.nbElements == 2 ? 2 : 3;
			switch (soundNb)
			{
				case 1:
					Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 27);
					break;
				case 2:
					Main.PlaySound(13, (int)projectile.position.X, (int)projectile.position.Y, 0);
					break;
				default:
					Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 107);
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

		public override void SafeOnHitNPC(NPC target, OrchidModAlchemistNPC modTarget, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			if (this.projOwner)
			{
				this.hitNPC = true;
				OrchidModGlobalNPC modTargetGlobal = target.GetGlobalNPC<OrchidModGlobalNPC>();

				this.OnHitNPCFirst(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal);
				this.OnHitNPCSecond(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal);
				this.OnHitNPCThird(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal);
			}
		}

		public void KillFirst(int timeLeft, Player player, OrchidModPlayer modPlayer)
		{
			if (this.fireFlaskGlobal != null)
			{
				fireFlaskGlobal.killFirstDelegate(timeLeft, player, modPlayer, this, projectile, this.fireFlaskGlobal);
			}

			if (this.waterFlaskGlobal != null)
			{
				waterFlaskGlobal.killFirstDelegate(timeLeft, player, modPlayer, this, projectile, this.waterFlaskGlobal);
			}

			if (this.natureFlaskGlobal != null)
			{
				natureFlaskGlobal.killFirstDelegate(timeLeft, player, modPlayer, this, projectile, this.natureFlaskGlobal);
			}

			if (this.airFlaskGlobal != null)
			{
				airFlaskGlobal.killFirstDelegate(timeLeft, player, modPlayer, this, projectile, this.airFlaskGlobal);
			}

			if (this.lightFlaskGlobal != null)
			{
				lightFlaskGlobal.killFirstDelegate(timeLeft, player, modPlayer, this, projectile, this.lightFlaskGlobal);
			}

			if (this.darkFlaskGlobal != null)
			{
				darkFlaskGlobal.killFirstDelegate(timeLeft, player, modPlayer, this, projectile, this.darkFlaskGlobal);
			}

			if (this.nbElements > 1 && player.HasBuff(BuffType<Alchemist.Buffs.JungleLilyExtractBuff>()))
			{
				int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.JungleLilyFlaskReaction>();
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, spawnProj, 0, 0f, projectile.owner);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 15, 10, 7, true, 1.5f, 1f, 3f);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 15, 15, 10, true, 1.5f, 1f, 5f);
			}
		}

		public void KillSecond(int timeLeft, Player player, OrchidModPlayer modPlayer)
		{
			if (this.fireFlaskGlobal != null)
			{
				fireFlaskGlobal.killSecondDelegate(timeLeft, player, modPlayer, this, projectile, this.fireFlaskGlobal);
			}

			if (this.waterFlaskGlobal != null)
			{
				waterFlaskGlobal.killSecondDelegate(timeLeft, player, modPlayer, this, projectile, this.waterFlaskGlobal);
			}

			if (this.natureFlaskGlobal != null)
			{
				natureFlaskGlobal.killSecondDelegate(timeLeft, player, modPlayer, this, projectile, this.natureFlaskGlobal);
			}

			if (this.airFlaskGlobal != null)
			{
				airFlaskGlobal.killSecondDelegate(timeLeft, player, modPlayer, this, projectile, this.airFlaskGlobal);
			}

			if (this.lightFlaskGlobal != null)
			{
				lightFlaskGlobal.killSecondDelegate(timeLeft, player, modPlayer, this, projectile, this.lightFlaskGlobal);
			}

			if (this.darkFlaskGlobal != null)
			{
				darkFlaskGlobal.killSecondDelegate(timeLeft, player, modPlayer, this, projectile, this.darkFlaskGlobal);
			}

			if (this.nbElements > 2 && player.HasBuff(BuffType<Alchemist.Buffs.QueenBeeFlaskBuff>()))
			{
				int itemType = ItemType<QueenBeeFlask>();
				int dmg = OrchidModAlchemistHelper.getSecondaryDamage(player, modPlayer, itemType, this.nbElements, true);
				int rand = this.nbElements + Main.rand.Next(3);
				for (int i = 0; i < rand; i++)
				{
					Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(80)));
					if (player.strongBees && Main.rand.Next(2) == 0)
						Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, 566, (int)(dmg * 1.15f), 0f, player.whoAmI, 0f, 0f);
					else
					{
						Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, 181, dmg, 0f, player.whoAmI, 0f, 0f);
					}
				}
			}

			if (player.HasBuff(BuffType<Alchemist.Buffs.SpiritedWaterBuff>()))
			{
				int spawnProj = ProjectileType<Alchemist.Projectiles.Water.DungeonFlaskProj>();
				int itemType = ItemType<DungeonFlask>();
				int dmg = OrchidModAlchemistHelper.getSecondaryDamage(player, modPlayer, itemType, this.nbElements, true);
				for (int i = 0; i < this.nbElements; i++)
				{
					Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, dmg, 0f, projectile.owner);
				}
			}
		}

		public void KillThird(int timeLeft, Player player, OrchidModPlayer modPlayer)
		{
			if (this.fireFlaskGlobal != null)
			{
				fireFlaskGlobal.killThirdDelegate(timeLeft, player, modPlayer, this, projectile, this.fireFlaskGlobal);
			}

			if (this.waterFlaskGlobal != null)
			{
				waterFlaskGlobal.killThirdDelegate(timeLeft, player, modPlayer, this, projectile, this.waterFlaskGlobal);
			}

			if (this.natureFlaskGlobal != null)
			{
				natureFlaskGlobal.killThirdDelegate(timeLeft, player, modPlayer, this, projectile, this.natureFlaskGlobal);
			}

			if (this.airFlaskGlobal != null)
			{
				airFlaskGlobal.killThirdDelegate(timeLeft, player, modPlayer, this, projectile, this.airFlaskGlobal);
			}

			if (this.lightFlaskGlobal != null)
			{
				lightFlaskGlobal.killThirdDelegate(timeLeft, player, modPlayer, this, projectile, this.lightFlaskGlobal);
			}

			if (this.darkFlaskGlobal != null)
			{
				darkFlaskGlobal.killThirdDelegate(timeLeft, player, modPlayer, this, projectile, this.darkFlaskGlobal);
			}
		}

		public void OnHitNPCFirst(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer, OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal)
		{
			if (this.fireFlaskGlobal != null)
			{
				fireFlaskGlobal.onHitNPCFirstDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.fireFlaskGlobal);
			}

			if (this.waterFlaskGlobal != null)
			{
				waterFlaskGlobal.onHitNPCFirstDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.waterFlaskGlobal);
			}

			if (this.natureFlaskGlobal != null)
			{
				natureFlaskGlobal.onHitNPCFirstDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.natureFlaskGlobal);
			}

			if (this.airFlaskGlobal != null)
			{
				airFlaskGlobal.onHitNPCFirstDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.airFlaskGlobal);
			}

			if (this.lightFlaskGlobal != null)
			{
				lightFlaskGlobal.onHitNPCFirstDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.lightFlaskGlobal);
			}

			if (this.darkFlaskGlobal != null)
			{
				darkFlaskGlobal.onHitNPCFirstDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.darkFlaskGlobal);
			}
		}

		public void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer, OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal)
		{
			if (this.fireFlaskGlobal != null)
			{
				fireFlaskGlobal.onHitNPCSecondDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.fireFlaskGlobal);
			}

			if (this.waterFlaskGlobal != null)
			{
				waterFlaskGlobal.onHitNPCSecondDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.waterFlaskGlobal);
			}

			if (this.natureFlaskGlobal != null)
			{
				natureFlaskGlobal.onHitNPCSecondDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.natureFlaskGlobal);
			}

			if (this.airFlaskGlobal != null)
			{
				airFlaskGlobal.onHitNPCSecondDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.airFlaskGlobal);
			}

			if (this.lightFlaskGlobal != null)
			{
				lightFlaskGlobal.onHitNPCSecondDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.lightFlaskGlobal);
			}

			if (this.darkFlaskGlobal != null)
			{
				darkFlaskGlobal.onHitNPCSecondDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.darkFlaskGlobal);
			}
		}

		public void OnHitNPCThird(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer, OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal)
		{
			if (this.fireFlaskGlobal != null)
			{
				fireFlaskGlobal.onHitNPCThirdDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.fireFlaskGlobal);
			}

			if (this.waterFlaskGlobal != null)
			{
				waterFlaskGlobal.onHitNPCThirdDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.waterFlaskGlobal);
			}

			if (this.natureFlaskGlobal != null)
			{
				natureFlaskGlobal.onHitNPCThirdDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.natureFlaskGlobal);
			}

			if (this.airFlaskGlobal != null)
			{
				airFlaskGlobal.onHitNPCThirdDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.airFlaskGlobal);
			}

			if (this.lightFlaskGlobal != null)
			{
				lightFlaskGlobal.onHitNPCThirdDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.lightFlaskGlobal);
			}

			if (this.darkFlaskGlobal != null)
			{
				darkFlaskGlobal.onHitNPCThirdDelegate(target, damage, knockback, crit, player, modPlayer, modTarget, modTargetGlobal, this, projectile, this.darkFlaskGlobal);
			}
		}

		public void addVariousEffects()
		{
			Player player = Main.player[projectile.owner];
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			this.nbElementsNoExtract += this.nbElements;

			int buffType = BuffType<Alchemist.Buffs.ReactiveVialsBuff>();
			if (player.HasBuff(buffType))
			{
				projectile.damage = (int)(projectile.damage * 1.1f);
				player.ClearBuff(buffType);
			}

			if (this.fireFlaskGlobal != null)
			{
				fireFlaskGlobal.addVariousEffectsDelegate(player, modPlayer, this, projectile, this.fireFlaskGlobal);
			}

			if (this.waterFlaskGlobal != null)
			{
				waterFlaskGlobal.addVariousEffectsDelegate(player, modPlayer, this, projectile, this.fireFlaskGlobal);
			}

			if (this.natureFlaskGlobal != null)
			{
				natureFlaskGlobal.addVariousEffectsDelegate(player, modPlayer, this, projectile, this.fireFlaskGlobal);
			}

			if (this.airFlaskGlobal != null)
			{
				airFlaskGlobal.addVariousEffectsDelegate(player, modPlayer, this, projectile, this.fireFlaskGlobal);
			}

			if (this.lightFlaskGlobal != null)
			{
				lightFlaskGlobal.addVariousEffectsDelegate(player, modPlayer, this, projectile, this.fireFlaskGlobal);
			}

			if (this.darkFlaskGlobal != null)
			{
				darkFlaskGlobal.addVariousEffectsDelegate(player, modPlayer, this, projectile, this.fireFlaskGlobal);
			}
		}

		public void spawnKillDusts(int timeLeft)
		{
			int rand = this.nbElements + Main.rand.Next(this.nbElements * 2);
			for (int i = 0; i < rand; i++)
			{
				int proj = ProjectileType<Alchemist.Projectiles.AlchemistSmoke1>();
				Vector2 vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, proj, 0, 0f, projectile.owner);
				Main.projectile[smokeProj].localAI[0] = this.glowColor.R;
				Main.projectile[smokeProj].localAI[1] = this.glowColor.G;
				Main.projectile[smokeProj].ai[1] = this.glowColor.B;
			}
			rand = 1 + Main.rand.Next(this.nbElements);
			for (int i = 0; i < rand; i++)
			{
				int proj = ProjectileType<Alchemist.Projectiles.AlchemistSmoke2>();
				Vector2 vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, proj, 0, 0f, projectile.owner);
				Main.projectile[smokeProj].localAI[0] = this.glowColor.R;
				Main.projectile[smokeProj].localAI[1] = this.glowColor.G;
				Main.projectile[smokeProj].ai[1] = this.glowColor.B;
			}
			rand = Main.rand.Next(this.nbElements);
			for (int i = 0; i < rand; i++)
			{
				int proj = ProjectileType<Alchemist.Projectiles.AlchemistSmoke3>();
				Vector2 vel = (new Vector2(0f, -((float)(2 + Main.rand.Next(5)))).RotatedByRandom(MathHelper.ToRadians(180)));
				int smokeProj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, proj, 0, 0f, projectile.owner);
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
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.waterFlaskGlobal.alchemistRightClickDust);
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
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.fireFlaskGlobal.alchemistRightClickDust);
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
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.natureFlaskGlobal.alchemistRightClickDust);
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
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.airFlaskGlobal.alchemistRightClickDust);
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
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.lightFlaskGlobal.alchemistRightClickDust);
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
						int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, this.darkFlaskGlobal.alchemistRightClickDust);
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