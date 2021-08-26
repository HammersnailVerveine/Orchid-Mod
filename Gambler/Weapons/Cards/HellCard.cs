using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class HellCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 40;
			item.crit = 4;
			item.knockBack = 3f;
			item.useAnimation = 30;
			item.useTime = 30;
			item.shootSpeed = 13f;
			this.cardRequirement = 3;
			this.gamblerCardSets.Add("Biome");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Hell");
			Tooltip.SetDefault("Launches fiery mortar"
							+ "\nChances to summon a pepper, replicating the attack");
		}

		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false)
		{
			int projType = ProjectileType<Gambler.Projectiles.HellCardProj>();
			OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy);
			Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y - 200, 1);
		}
	}
}
