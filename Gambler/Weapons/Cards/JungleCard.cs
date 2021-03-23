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
	public class JungleCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 9;
			item.crit = 4;
			item.knockBack = 1f;
			item.useAnimation = 30;
			item.useTime = 30;
			item.shootSpeed = 10f;
			this.cardRequirement = 2;
			this.gamblerCardSets.Add("Biome");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Jungle");
		    Tooltip.SetDefault("Releases homing spores, dealing more damage for each already active one"
							+  "\nChances to release a seed, replicating the attack");
		}
		
		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false) {
			Vector2 vel = new Vector2(speedX, speedY / 5f).RotatedByRandom(MathHelper.ToRadians(15));
			int projType = ProjectileType<Gambler.Projectiles.JungleCardProj>();
			OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, projType, damage, knockBack, player.whoAmI), dummy);
			Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
		}
	}
}
