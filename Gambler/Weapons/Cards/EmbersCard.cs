using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class EmbersCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = 1;
			Item.damage = 7;
			Item.crit = 4;
			Item.knockBack = 1f;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.shootSpeed = 5f;
			this.cardRequirement = 0;
			this.gamblerCardSets.Add("Elemental");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Embers");
			Tooltip.SetDefault("Releases homing embers");
		}

		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false)
		{
			Vector2 vel = new Vector2(speedX, speedY / 5f).RotatedByRandom(MathHelper.ToRadians(15));
			int projType = ProjectileType<Gambler.Projectiles.EmbersCardProj>();
			OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, projType, damage, knockBack, player.whoAmI), dummy);
			SoundEngine.PlaySound(2, (int)player.Center.X, (int)player.Center.Y - 200, 1);
		}
	}
}
