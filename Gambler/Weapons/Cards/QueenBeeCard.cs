using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class QueenBeeCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 8;
			item.crit = 4;
			item.knockBack = 1f;
			item.useAnimation = 30;
			item.useTime = 30;
			this.cardRequirement = 4;
			this.gamblerCardSets.Add("Boss");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Poisonous Queen");
			Tooltip.SetDefault("Summons a bee hive, following your cursor"
							+ "\nShake it to summon bees");
		}

		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (Main.mouseLeft && Main.mouseLeftRelease || modPlayer.gamblerJustSwitched)
			{
				int projType = ProjectileType<Gambler.Projectiles.QueenBeeCardProj>();
				for (int l = 0; l < Main.projectile.Length; l++)
				{
					Projectile proj = Main.projectile[l];
					if (proj.active && proj.type == projType && proj.owner == player.whoAmI)
					{
						proj.Kill();
						break;
					}
				}
				modPlayer.gamblerJustSwitched = false;
				OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy);
				Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y - 200, 1);
			}
			else
			{
				Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y - 200, 7);
			}
		}
	}
}
