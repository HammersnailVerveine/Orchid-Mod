using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class BubbleCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = 1;
			Item.damage = 13;
			Item.crit = 4;
			Item.knockBack = 1f;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.shootSpeed = 5f;
			this.cardRequirement = 2;
			this.gamblerCardSets.Add("Elemental");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Bubbles");
			Tooltip.SetDefault("Summons bubbles, floating upwards"
							+ "\nReleasing your left click causes existing bubbles to dash"
							+ "\nBubbles gain in damage over time");
		}

		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false)
		{
			int projType = ProjectileType<Gambler.Projectiles.BubbleCardProj>();
			float scale = 1f - (Main.rand.NextFloat() * .3f);
			Vector2 vel = new Vector2(0f, -3f).RotatedByRandom(MathHelper.ToRadians(30));
			vel = vel * scale;
			OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, projType, damage, knockBack, player.whoAmI), dummy);
			SoundEngine.PlaySound(2, (int)player.Center.X, (int)player.Center.Y - 200, 86);
		}
	}
}
