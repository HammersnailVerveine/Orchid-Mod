using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class DesertCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 8;
			item.crit = 4;
			item.knockBack = 0.5f;
			item.useAnimation = 10;
			item.useTime = 10;
			item.shootSpeed = 8f;
			this.cardRequirement = 3;
			this.gamblerCardSets.Add("Biome");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Desert");
			Tooltip.SetDefault("Rapidly fires thorns"
							+ "\nPeriodically summons a cactus, replicating the attack");
		}

		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false)
		{
			int projType = ProjectileType<Gambler.Projectiles.DesertCardProj>();
			float scale = 1f - (Main.rand.NextFloat() * .3f);
			Vector2 vel = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20));
			vel = vel * scale;
			OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, projType, damage, knockBack, player.whoAmI), dummy);
			Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y - 200, 7);
		}
	}
}
