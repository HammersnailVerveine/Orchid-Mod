﻿using Microsoft.Xna.Framework;
using OrchidMod.Common.Global.Projectiles;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Gambler.Projectiles
{
	public class QueenBeeCardProj : OrchidModGamblerProjectile
	{
		private bool positiveX = false;
		private bool positiveY = false;
		private int bounceDelay = 0;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hive");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 30;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			this.projectileTrail = true;
		}

		public override void SafeAI()
		{
			Player player = Main.player[Projectile.owner];
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			int cardType = Projectile.GetGlobalProjectile<OrchidGlobalProjectile>().gamblerDummyProj ? modPlayer.gamblerCardDummy.type : modPlayer.gamblerCardCurrent.type;

			this.bounceDelay -= this.bounceDelay > 0 ? 1 : 0;

			bool spawnBees = false;
			if (positiveX == true)
			{
				if (Projectile.velocity.X < 0f)
				{
					this.positiveX = false;
					spawnBees = true;
				}
			}
			else
			{
				if (Projectile.velocity.X > 0f)
				{
					this.positiveX = true;
					spawnBees = true;
				}
			}

			if (positiveY == true)
			{
				if (Projectile.velocity.Y < 0f)
				{
					this.positiveY = false;
					spawnBees = true;
				}
			}
			else
			{
				if (Projectile.velocity.Y > 0f)
				{
					this.positiveY = true;
					spawnBees = true;
				}
			}

			if (Main.rand.Next(3) == 0 && ((spawnBees && Main.rand.Next(2) == 0) || Main.rand.Next(50) == 0))
			{
				int rand = Main.rand.Next(2) + 1;
				for (int i = 0; i < rand; i++)
				{
					Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
					if (player.strongBees && Main.rand.Next(2) == 0)
					{
						bool dummy = Projectile.GetGlobalProjectile<OrchidGlobalProjectile>().gamblerDummyProj;
						DummyProjectile(Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, vel.X, vel.Y, 566, (int)(Projectile.damage * 1.15f), 0f, Projectile.owner, 0f, 0f), dummy);
					}
					else
					{
						bool dummy = Projectile.GetGlobalProjectile<OrchidGlobalProjectile>().gamblerDummyProj;
						int newProj = DummyProjectile(Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, vel.X, vel.Y, 181, Projectile.damage, 0f, Projectile.owner, 0f, 0f), dummy);
						Main.projectile[newProj].CritChance = Projectile.CritChance;
						OrchidGlobalProjectile modProjectile = Main.projectile[newProj].GetGlobalProjectile<OrchidGlobalProjectile>();
						modProjectile.gamblerProjectile = true;
					}
				}
			}

			if (Main.myPlayer == Projectile.owner && modPlayer.GamblerDeckInHand)
			{
				if (Main.mouseLeft && cardType == ItemType<Gambler.Weapons.Cards.QueenBeeCard>() && this.bounceDelay <= 0)
				{
					Vector2 newMove = Main.MouseWorld - Projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo > 5f)
					{
						newMove *= 10f / distanceTo;
						Projectile.velocity = newMove;
						Projectile.netUpdate = true;
					}
					else
					{
						if (Projectile.velocity.Length() > 0f)
						{
							Projectile.velocity *= 0f;
							Projectile.netUpdate = true;
						}
					}
				}
				else
				{
					Projectile.velocity *= 0f;
					Projectile.timeLeft = this.Initialized ? Projectile.timeLeft : 600;
					this.Initialized = true;
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
			SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
			this.bounceDelay = 15;
			return false;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 4; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 153);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}
		}
	}
}