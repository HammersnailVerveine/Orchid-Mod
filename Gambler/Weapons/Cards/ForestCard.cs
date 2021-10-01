using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class ForestCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 6;
			item.crit = 4;
			item.knockBack = 1f;
			item.useAnimation = 30;
			item.useTime = 30;
			item.shootSpeed = 10f;
			this.cardRequirement = 0;
			this.gamblerCardSets.Add("Biome");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Forest");
			Tooltip.SetDefault("Tosses a handful of acorns"
							+ "\nPeriodically summons a seed, replicating the attack");
		}

		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false)
		{
			int rand = Main.rand.Next(3) + 1;
			int projType = ProjectileType<Gambler.Projectiles.ForestCardProj>();
			float scale = 1f - (Main.rand.NextFloat() * .3f);
			for (int i = 0; i < rand; i++)
			{
				Vector2 vel = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(30));
				vel = vel * scale;
				OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, projType, damage, knockBack, player.whoAmI), dummy);
			}
			Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y - 200, 1);
		}
	}
}
