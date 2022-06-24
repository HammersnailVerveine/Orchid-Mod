using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using OrchidMod.Common.Globals.NPCs;
using Terraria;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace OrchidMod.Alchemist.Weapons.Air
{
	public class QueenBeeFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 15;
			Item.width = 30;
			Item.height = 30;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			this.potencyCost = 3;
			this.element = AlchemistElement.AIR;
			this.rightClickDust = 153;
			this.colorR = 255;
			this.colorG = 156;
			this.colorB = 12;
			this.secondaryDamage = 15;
			this.secondaryScaling = 2f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Royal Jelly");
			Tooltip.SetDefault("If no fire element is used, summons bees on impact"
							+ "\nHas a chance to release a catalytic beehive");
		}

		public override void KillSecond(int timeLeft, Player player, OrchidModPlayerAlchemist modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int nb = 2 + Main.rand.Next(2);
			if (alchProj.fireFlask.type == ItemID.None)
			{
				for (int i = 0; i < nb; i++)
				{
					Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
					int spawnProj = ProjectileType<Alchemist.Projectiles.Air.QueenBeeFlaskProj>();
					Projectile.NewProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, spawnProj, 0, 0f, projectile.owner);
				}
				int dmg = GetSecondaryDamage(player, alchProj.nbElements);
				int rand = alchProj.nbElements + Main.rand.Next(3) + 1;
				for (int i = 0; i < rand; i++)
				{
					Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(80)));
					if (player.strongBees && Main.rand.NextBool(2))
						Projectile.NewProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, ProjectileID.GiantBee, (int)(dmg * 1.15f), 0f, projectile.owner, 0f, 0f);
					else
					{
						Projectile.NewProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, ProjectileID.Bee, dmg, 0f, projectile.owner, 0f, 0f);
					}
				}
			}
		}

		public override void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayerAlchemist modPlayer,
		OrchidModAlchemistNPC modTarget, OrchidGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int rand = alchProj.nbElements;
			if (Main.rand.Next(10) < rand)
			{
				int dmg = GetSecondaryDamage(player, alchProj.nbElements);
				int proj = ProjectileType<Alchemist.Projectiles.Reactive.AlchemistHive>();
				Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, perturbedSpeed, proj, dmg, 0f, projectile.owner);
			}
		}
	}
}
