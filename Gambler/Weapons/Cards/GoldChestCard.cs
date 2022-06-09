using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class GoldChestCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = 1;
			Item.damage = 16;
			Item.crit = 4;
			Item.knockBack = 1f;
			Item.useAnimation = 10;
			Item.useTime = 10;
			Item.shootSpeed = 10f;
			this.cardRequirement = 3;
			this.gamblerCardSets.Add("Elemental");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Enchantment");
			Tooltip.SetDefault("Releases damaging sparkles");
		}

		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false)
		{
			int projType = ProjectileType<Gambler.Projectiles.GoldChestCardProj>();
			float scale = 1f - (Main.rand.NextFloat() * .3f);
			Vector2 vel = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
			vel = vel * scale;
			int newProj = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, projType, damage, knockBack, player.whoAmI), dummy);
			Main.projectile[newProj].ai[1] = Main.rand.Next(4);
			Main.projectile[newProj].netUpdate = true;
			SoundEngine.PlaySound(2, (int)player.Center.X, (int)player.Center.Y - 200, 9);
		}
	}
}
