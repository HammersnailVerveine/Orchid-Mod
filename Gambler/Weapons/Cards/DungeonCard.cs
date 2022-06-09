using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class DungeonCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = 1;
			Item.damage = 20;
			Item.crit = 10;
			Item.knockBack = 1f;
			Item.useAnimation = 45;
			Item.useTime = 15;
			Item.reuseDelay = 65;
			Item.shootSpeed = 1f;
			this.cardRequirement = 0;
			this.gamblerCardSets.Add("Elemental");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Spirit Tear");
			Tooltip.SetDefault("Fires a burst of spirit bolts"
							+ "\nHitting the same target with all projectiles will rip a part of their soul"
							+ "\nPicking it up will deal a large amount of damage");
		}

		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false)
		{
			int projType = ProjectileType<Gambler.Projectiles.DungeonCardProj>();
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
				for (int k = 0; k < Main.npc.Length; k++)
				{
					if (Main.npc[k].active)
					{
						OrchidModGlobalNPC modTarget = Main.npc[k].GetGlobalNPC<OrchidModGlobalNPC>();
						modTarget.gamblerDungeonCardCount = 0;
					}
				}
			}

			int newProj = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy);
			Main.projectile[newProj].ai[1] = Main.rand.Next(4);
			Main.projectile[newProj].netUpdate = true;
			SoundEngine.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 8);
		}
	}
}
