using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class SnowCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = 1;
			Item.damage = 22;
			Item.crit = 4;
			Item.knockBack = 1f;
			Item.useAnimation = 40;
			Item.useTime = 40;
			Item.shootSpeed = 5f;
			this.cardRequirement = 1;
			this.gamblerCardSets.Add("Biome");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Snow");
			Tooltip.SetDefault("Summons a snow bush above your head"
							+ "\nDrag and release a fruit to launch it");
		}

		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false)
		{
			SoundEngine.PlaySound(2, (int)player.Center.X, (int)player.Center.Y - 200, 1);
			int projType = ProjectileType<Gambler.Projectiles.SnowCardBase>();
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
