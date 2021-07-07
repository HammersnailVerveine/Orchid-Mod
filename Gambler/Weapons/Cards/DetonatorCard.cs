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
	public class DetonatorCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 50;
			item.crit = 4;
			item.knockBack = 10f;
			item.useAnimation = 60;
			item.useTime = 60;
			item.shootSpeed = 10f;
			this.cardRequirement = 0;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Detonator");
		    Tooltip.SetDefault("Throws explosives, detonating upon releasing left click"
							+  "\nHas a small delay before being able to detonate");
		}
		
		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false) {
			int projType = ProjectileType<Gambler.Projectiles.DetonatorCardProj>();
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
