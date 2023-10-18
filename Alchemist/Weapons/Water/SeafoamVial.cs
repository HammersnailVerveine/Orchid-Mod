using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using OrchidMod.Common.Globals.NPCs;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Water
{
	public class SeafoamVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 11;
			Item.width = 30;
			Item.height = 30;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.WATER;
			this.rightClickDust = 29;
			this.colorR = 1;
			this.colorG = 139;
			this.colorB = 252;
			this.secondaryDamage = 10;
			this.secondaryScaling = 5f;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Seafoam Flask");
			/* Tooltip.SetDefault("Creates a lingering, damaging water bubble"
							+ "\nHas a chance to release a catalytic seafoam bubble"); */
		}

		public override void KillSecond(int timeLeft, Player player, OrchidAlchemist modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int nb = 2 + Main.rand.Next(2);
			for (int i = 0; i < nb; i++)
			{
				Vector2 vel = (new Vector2(0f, -(float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(90)));
				int spawnProj = alchProj.natureFlask.type == ItemType<Alchemist.Weapons.Nature.PoisonVial>() ? ProjectileType<Alchemist.Projectiles.Nature.PoisonVialProjAlt>() : ProjectileType<Alchemist.Projectiles.Water.SeafoamVialProjAlt>();
				SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, spawnProj, 0, 0f, projectile.owner);
			}
			int dmg = GetSecondaryDamage(player, alchProj.nbElements);
			int shoot = ProjectileType<Alchemist.Projectiles.Water.SeafoamVialProj>();
			if (alchProj.natureFlask.type == ItemType<Alchemist.Weapons.Nature.PoisonVial>())
			{
				dmg = modPlayer.GetSecondaryDamage(alchProj.natureFlask.type, alchProj.nbElements);
				shoot = ProjectileType<Alchemist.Projectiles.Nature.PoisonVialProj>();
			}
			nb = alchProj.hasCloud() ? 2 : 1;
			for (int i = 0; i < nb; i++)
			{
				Vector2 vel = (new Vector2(0f, -2.5f).RotatedByRandom(MathHelper.ToRadians(30)));
				vel *= (float)(1 - (Main.rand.Next(10) / 10));
				SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, shoot, dmg, 0.5f, projectile.owner);
			}
		}

		public override void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidAlchemist modPlayer,
		OrchidModAlchemistNPC modTarget, OrchidGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int rand = alchProj.nbElements;
			rand += alchProj.hasCloud() ? 2 : 0;
			if (Main.rand.Next(10) < rand && !alchProj.noCatalyticSpawn)
			{
				int dmg = GetSecondaryDamage(player, alchProj.nbElements);
				int proj = ProjectileType<Alchemist.Projectiles.Reactive.SeafoamBubble>();
				if (alchProj.natureFlask.type == ItemType<Alchemist.Weapons.Nature.PoisonVial>())
				{
					dmg = modPlayer.GetSecondaryDamage(alchProj.natureFlask.type, alchProj.nbElements);
					proj = ProjectileType<Alchemist.Projectiles.Reactive.PoisonBubble>();
				}
				Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(20));
				SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, perturbedSpeed, proj, dmg, 0f, projectile.owner);
			}
		}
	}
}
