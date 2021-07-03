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
	public class LavaSlimeCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 34;
			item.crit = 4;
			item.knockBack = 0.5f;
			item.shootSpeed = 10f;
			item.useAnimation = 30;
			item.useTime = 30;
			this.cardRequirement = 3;
			this.gamblerCardSets.Add("Slime");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Living Lava");
		    Tooltip.SetDefault("Summons a bouncy lava slime, following your cursor"
							+  "\nEach successful hit increases damage, touching the ground resets it"
							+  "\nEvery 3 consecutive hits, the slime will release a damaging explosion");
		}
		
		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false) {
			int projType = ProjectileType<Gambler.Projectiles.LavaSlimeCardProj>();
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
				OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy);
				Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
			} else {
				Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 7);
			}
		}
	}
}
