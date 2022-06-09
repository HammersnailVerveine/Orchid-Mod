using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class EaterCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = 1;
			Item.damage = 12;
			Item.crit = 4;
			Item.knockBack = 1f;
			Item.useAnimation = 30;
			Item.useTime = 30;
			this.cardRequirement = 4;
			this.gamblerCardSets.Add("Boss");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Corrupted Nightmare");
			Tooltip.SetDefault("Summon a corrupted worm, following your cursor"
							+ "\nRandomly releases rotten meat chunks"
							+ "\nCollecting them increases the worm damage and length");
		}

		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false)
		{
			int projType = ProjectileType<Gambler.Projectiles.EaterCardProj1>();
			bool found = false;
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == projType && proj.owner == player.whoAmI)
				{
					found = true;
					break;
				}
			}
			if (!found)
			{
				int newProjectile2 = (OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X - speedX * 3f, position.Y - speedY * 3f, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy));
				Main.projectile[newProjectile2].ai[0] = 1f;
				int newProjectile = (OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy));
				Main.projectile[newProjectile].ai[0] = 0f;
				Main.projectile[newProjectile2].ai[1] = newProjectile;
				Main.projectile[newProjectile].localAI[0] = newProjectile2;
				Main.projectile[newProjectile].netUpdate = true;
				Main.projectile[newProjectile2].netUpdate = true;
				SoundEngine.PlaySound(2, (int)player.Center.X, (int)player.Center.Y - 200, 1);
			}
			else
			{
				SoundEngine.PlaySound(2, (int)player.Center.X, (int)player.Center.Y - 200, 7);
			}
		}
	}
}
