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
	public class BrainCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 35;
			item.crit = 4;
			item.knockBack = 1f;
			item.useAnimation = 30;
			item.useTime = 30;
			this.cardRequirement = 4;
			this.gamblerCardSets.Add("Boss");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : The Hivemind");
		    Tooltip.SetDefault("Summons 3 brains around you, one of them following your cursor"
							+  "\nOnly one of them is real, and deals contact damage"
							+  "\nHitting randomly changes the true brain, and increases damage a lot"
							+  "\nBrains cannot deal damage if they are too close to you");
		}
		
		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false) {
			int projType = ProjectileType<Gambler.Projectiles.BrainCardProj>();
			bool found = false;
			for (int l = 0; l < Main.projectile.Length; l++) {  
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == projType && proj.owner == player.whoAmI)
				{
					found = true;
					break;
				} 
			}
			if (!found) {
				for (int i = 0; i < 3 ; i ++) {	
					int newProj = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy);
					Main.projectile[newProj].ai[1] = (float)(i);
					Main.projectile[newProj].ai[0] = (float)(i == 0 ? 300 : 0);
					Main.projectile[newProj].friendly = i == 0;
					Main.projectile[newProj].netUpdate = true;
				}
				Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
			} else {
				Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 7);
			}
		}
	}
}
