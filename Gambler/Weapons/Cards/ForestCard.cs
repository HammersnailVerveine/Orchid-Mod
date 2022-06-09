using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class ForestCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = 1;
			Item.damage = 44;
			Item.crit = 4;
			Item.knockBack = 6f;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.shootSpeed = 10f;
			this.cardRequirement = 0;
			this.gamblerCardSets.Add("Biome");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Forest");
			Tooltip.SetDefault("Summons a forest bush above your head"
							+ "\nDrag and release a fruit to launch it");
		}

		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false)
		{	
			SoundEngine.PlaySound(2, (int)player.Center.X, (int)player.Center.Y - 200, 1);
			int projType = ProjectileType<Gambler.Projectiles.ForestCardBase>();
			bool found = false;
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == projType && proj.owner == player.whoAmI && proj.ai[1] != 1f)
				{
					found = true;
					break;
				}
			}
			if (!found) {
				OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, 0f, 0f, projType, damage, knockBack, player.whoAmI), dummy);
			}
		}
	}
}
