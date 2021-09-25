using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class IvyChestCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 11;
			item.crit = 4;
			item.knockBack = 0.8f;
			item.useAnimation = 75;
			item.useTime = 75;
			item.shootSpeed = 5f;
			this.cardRequirement = 1;
			this.gamblerCardSets.Add("Elemental");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Deep Forest");
			Tooltip.SetDefault("Releases bursts of leaves, able to be pushed with the cursor"
							+ "\nThe further they are pushed, the more damage they deal"
							+ "\nOnly 3 sets of leaves can exist at once");
		}

		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false)
		{
			int projType = ProjectileType<Gambler.Projectiles.IvyChestCardProj>();

			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == projType && proj.owner == player.whoAmI)
				{
					proj.ai[1]++;
					if (proj.ai[1] >= 3f)
					{
						proj.Kill();
					}
					proj.netUpdate = true;
				}
			}

			for (int i = 0; i < 5 + Main.rand.Next(4); i++)
			{
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				Vector2 vel = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
				vel = vel * scale;
				OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, projType, damage, knockBack, player.whoAmI), dummy);
			}

			Main.PlaySound(6, (int)player.Center.X, (int)player.Center.Y, 0);
		}
	}
}
