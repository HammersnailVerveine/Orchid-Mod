using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Water
{
	public class DungeonFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 25;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 30, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.WATER;
			this.rightClickDust = 29;
			this.colorR = 6;
			this.colorG = 13;
			this.colorB = 144;
			this.secondaryDamage = 12;
			this.secondaryScaling = 7f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirited Water");
			Tooltip.SetDefault("Releases lingering water flames"
							+ "\nThe flames are considered as lingering particles"
							+ "\nHas a chance to release a catalytic spirited bubble");
		}

		public override void KillSecond(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int dmg = getSecondaryDamage(player, modPlayer, alchProj.nbElements);
			int rand = alchProj.nbElements + Main.rand.Next(2);
			for (int i = 0; i < rand; i++)
			{
				Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Water.DungeonFlaskProj>(), dmg, 0f, projectile.owner);
			}
		}

		public override void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer,
		OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int rand = alchProj.nbElements;
			rand += alchProj.hasCloud() ? 2 : 0;
			if (Main.rand.Next(10) < rand && !alchProj.noCatalyticSpawn)
			{
				int dmg = getSecondaryDamage(player, modPlayer, alchProj.nbElements);
				int proj = ProjectileType<Alchemist.Projectiles.Reactive.SpiritedBubble>();
				Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, proj, dmg, 0f, projectile.owner);
			}
		}
	}
}
