using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class OceanCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 25;
			item.crit = 4;
			item.knockBack = 5f;
			item.useAnimation = 50;
			item.useTime = 50;
			item.shootSpeed = 5f;
			this.cardRequirement = 1;
			this.gamblerCardSets.Add("Biome");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Ocean");
			Tooltip.SetDefault("Throws rolling coconuts"
							+ "\nPeriodically summons a seed, replicating the attack");
		}

		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false)
		{
			Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y - 200, 1);
			int projType = ProjectileType<Gambler.Projectiles.OceanCardProjAlt>();
			
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == projType && proj.owner == player.whoAmI && proj.ai[1] == 0f)
				{
					float distance = (position - proj.Center).Length();
					if (distance < 500f) {
						return;
					}
				}
			}
			
			Vector2 vel = (new Vector2(0f, -1f).RotatedBy(MathHelper.ToRadians(10)));
			int newProjectile = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, projType, damage, knockBack, player.whoAmI), dummy); 
			Main.projectile[newProjectile].ai[1] = 0f;
			Main.projectile[newProjectile].netUpdate = true;
			for (int i = 0; i < 5; i++)
			{
				int dustType = 31;
				Main.dust[Dust.NewDust(player.Center, 10, 10, dustType)].velocity *= 0.25f;
			}
		}
	}
}
