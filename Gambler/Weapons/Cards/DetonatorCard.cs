using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class DetonatorCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = 1;
			Item.damage = 50;
			Item.crit = 4;
			Item.knockBack = 10f;
			Item.useAnimation = 60;
			Item.useTime = 60;
			Item.shootSpeed = 10f;
			this.cardRequirement = 0;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Detonator");
			Tooltip.SetDefault("Throws explosives, detonating upon releasing left click"
							+ "\nHas a small delay before being able to detonate");
		}

		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false)
		{
			int projType = ProjectileType<Gambler.Projectiles.DetonatorCardProj>();
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
				OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy);
				SoundEngine.PlaySound(2, (int)player.Center.X, (int)player.Center.Y - 200, 1);
			}
			else
			{
				SoundEngine.PlaySound(2, (int)player.Center.X, (int)player.Center.Y - 200, 7);
			}
		}
	}
}
