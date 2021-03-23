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
	public class SnowCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 10;
			item.crit = 4;
			item.knockBack = 2f;
			item.useAnimation = 40;
			item.useTime = 40;
			item.shootSpeed = 5f;
			this.cardRequirement = 1;
			this.gamblerCardSets.Add("Biome");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Snow");
		    Tooltip.SetDefault("Throws returning snowflakes backwards, gaining in damage over time"
							+  "\nThe snowflakes cannot be thrown diagonally"
							+  "\nChances to summon a pine cone, replicating the attack");
		}
		
		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false) {
			int projType = ProjectileType<Gambler.Projectiles.SnowCardProj>();
			Vector2 target = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
			Vector2 vel = new Vector2(0f, 0f);
			
			float absX = (float)Math.Sqrt((player.Center.X - target.X) * (player.Center.X - target.X));
			float absY = (float)Math.Sqrt((player.Center.Y - target.Y) * (player.Center.Y - target.Y));
			if (absX > absY) {
				vel.X = target.X < player.Center.X ? 1f : -1f;
			} else {
				vel.Y = target.Y < player.Center.Y ? 1f : -1f;
			}
			
			vel.Normalize();
			vel *= new Vector2(speedX, speedY).Length();
			OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, projType, damage, knockBack, player.whoAmI), dummy);
			Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
		}
	}
}
