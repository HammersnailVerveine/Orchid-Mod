using Microsoft.Xna.Framework;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class ShuffleCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 8;
			item.crit = 4;
			item.knockBack = 3f;
			item.useAnimation = 25;
			item.useTime = 25;
			item.shootSpeed = 10f;
			this.cardRequirement = 0;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Shuffle");
			Tooltip.SetDefault("Randomly shoots a selection of clubs, spades, diamonds and hearts"
							+ "\nEach projectile has its own properties and behaviour"
							+ "\nHold the attack button to create more projectiles and enhance their effects"
							+ "\nDamage increases with the number of cards in your deck");
		}

		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int projType = ProjectileType<Gambler.Projectiles.ShuffleCardProj>();
			float aiType = Main.rand.Next(4);
			int count = 0;
			int damageCount = damage + (int)(OrchidModGamblerHelper.getNbGamblerCards(player, modPlayer) * 1.2f);
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (player.whoAmI == proj.owner && proj.active && proj.ai[1] != 6f && proj.type == projType)
				{
					aiType = proj.ai[0];
					count++;
					damageCount = damage * (count + 1);
					proj.damage = damageCount;
					proj.netUpdate = true;
				}
			}
			if (count < 5)
			{
				int newProjInt = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, 0f, 0f, projType, damageCount, knockBack, player.whoAmI), dummy);
				Projectile newProj = Main.projectile[newProjInt];
				newProj.ai[1] = (float)(count + 1);
				newProj.ai[0] = (float)aiType;
				newProj.netUpdate = true;
				Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y - 200, count == 4 ? 35 : 1);
			}
			else
			{
				Main.PlaySound(2, (int)player.Center.X, (int)player.Center.Y - 200, 7);
			}
		}
	}
}
