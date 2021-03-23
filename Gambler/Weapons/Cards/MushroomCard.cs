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
	public class MushroomCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 24;
			item.crit = 4;
			item.knockBack = 2f;
			item.useAnimation = 30;
			item.useTime = 30;
			item.shootSpeed = 10f;
			this.cardRequirement = 2;
			this.gamblerCardSets.Add("Biome");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Glowing Mushroom");
		    Tooltip.SetDefault("Throws bouncy mushrooms, exploding on second contact"
							+  "\nThe projectile cannot deal damage before bouncing once"
							+  "\nChances to summon a fungi, replicating the attack");
		}
		
		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false) {
			int projType = ProjectileType<Gambler.Projectiles.MushroomCardProj>();
			OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, speedX, speedY, projType, damage, knockBack, player.whoAmI), dummy);
			Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
		}
	}
}
