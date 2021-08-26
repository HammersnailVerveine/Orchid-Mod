using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class OceanCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 13;
			item.crit = 4;
			item.knockBack = 5f;
			item.useAnimation = 50;
			item.useTime = 50;
			item.shootSpeed = 5f;
			this.cardRequirement = 1;
			this.gamblerCardSets.Add("Biome");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Ocean");
			Tooltip.SetDefault("Throws rolling coconuts"
							+ "\nChances to summon a seed, replicating the attack");
		}

		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false)
		{
			int projType = ProjectileType<Gambler.Projectiles.OceanCardProj>();
			OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy);
			Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y - 200, 1);
		}
	}
}
