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
	public class EmbersCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 6;
			item.crit = 4;
			item.knockBack = 1f;
			item.useAnimation = 15;
			item.useTime = 15;
			item.shootSpeed = 5f;
			this.cardRequirement = 0;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Embers");
		    Tooltip.SetDefault("Releases homing embers");
		}
		
		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false) {
			Vector2 vel = new Vector2(speedX, speedY / 5f).RotatedByRandom(MathHelper.ToRadians(15));
			int projType = ProjectileType<Gambler.Projectiles.EmbersCardProj>();
			OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, projType, damage, knockBack, player.whoAmI), dummy);
			Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
		}
	}
}
