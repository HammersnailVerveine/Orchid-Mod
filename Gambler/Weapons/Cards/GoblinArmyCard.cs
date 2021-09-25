using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class GoblinArmyCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 55;
			item.crit = 4;
			item.knockBack = 10f;
			item.useAnimation = 60;
			item.useTime = 60;
			item.shootSpeed = 0.75f;
			this.cardRequirement = 4;
			this.gamblerCardSets.Add("Elemental");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Erratic Magic");
			Tooltip.SetDefault("Throws a portal, firing shadow bolts at an increasing rate"
							+ "\nThe portal will disappear upon releasing left click or getting too far from it");
		}

		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false)
		{
			int projType = ProjectileType<Gambler.Projectiles.GoblinArmyCardProj>();
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
				for (int i = 0; i < 2; i++)
				{
					int newProjInt = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy);
					Projectile newProj = Main.projectile[newProjInt];
					newProj.ai[1] = i + 1;
					newProj.netUpdate = true;
				}
				Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y - 200, 1);
			}
			else
			{
				Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y - 200, 7);
			}
		}
	}
}
