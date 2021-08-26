using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class SkyCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 18;
			item.crit = 4;
			item.knockBack = 2f;
			item.useAnimation = 20;
			item.useTime = 20;
			item.shootSpeed = 8f;
			this.cardRequirement = 3;
			this.gamblerCardSets.Add("Biome");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Sky");
			Tooltip.SetDefault("Calls stars from the sky"
							+ "\nThe stars will sharply turn upon reaching cursor height"
							+ "\nChances to summon a skyware banana, replicating the attack");
		}

		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false)
		{
			int projType = ProjectileType<Gambler.Projectiles.SkyCardProj>();
			Vector2 vel = new Vector2(0f, 8f).RotatedByRandom(MathHelper.ToRadians(20));
			int newProjInt = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y - Main.screenHeight / 2 - 20, vel.X, vel.Y, projType, damage, knockBack, player.whoAmI), dummy);
			Projectile newProj = Main.projectile[newProjInt];
			newProj.ai[0] = Main.screenPosition.Y + (float)Main.mouseY - 10;

			Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 9);
		}
	}
}
