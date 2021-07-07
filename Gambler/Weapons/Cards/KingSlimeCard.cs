using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class KingSlimeCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 20;
			item.crit = 4;
			item.knockBack = 1f;
			item.shootSpeed = 10f;
			item.useAnimation = 30;
			item.useTime = 30;
			this.cardRequirement = 3;
			this.gamblerCardSets.Add("Boss");
			this.gamblerCardSets.Add("Slime");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Blue King");
		    Tooltip.SetDefault("Summons a bouncy slime, following your cursor, and jumping up to it"
							+  "\nGains in damage with fall distance and enemy hits, touching on the ground resets it");
		}
		
		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false) {
			int projType = ProjectileType<Gambler.Projectiles.KingSlimeCardProj>();
			bool found = false;
			for (int l = 0; l < Main.projectile.Length; l++) {  
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == projType && proj.owner == player.whoAmI) {
					found = true;
					break;
				} 
			}
			
			if (!found) {
				OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy);
				Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
			} else {
				Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 7);
			}
		}
	}
}
