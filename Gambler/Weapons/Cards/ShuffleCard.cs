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
	public class ShuffleCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 1;
			item.damage = 5;
			item.crit = 4;
			item.knockBack = 3f;
			item.useAnimation = 25;
			item.useTime = 25;
			item.shootSpeed = 10f;
			this.cardRequirement = 0;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Shuffle");
		    Tooltip.SetDefault("Randomly shoots a selection of clubs, spades, diamonds and hearts"
							+  "\nEach projectile has its own properties and behaviour"
							+  "\nDamage increases with the number of cards in your deck");
		}
		
		public override void GamblerShoot(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int projType = 0;
			int rand = Main.rand.Next(4);
			switch (rand) {
				case 0:
					projType = ProjectileType<Gambler.Projectiles.ShuffleCardProj1>();
					break;
				case 1:
					projType = ProjectileType<Gambler.Projectiles.ShuffleCardProj2>();
					break;
				case 2:
					projType = ProjectileType<Gambler.Projectiles.ShuffleCardProj3>();
					break;
				default:
					projType = ProjectileType<Gambler.Projectiles.ShuffleCardProj4>();
					break;
			}
			float scale = 1f - (Main.rand.NextFloat() * .3f);
			Vector2 vel = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
			vel = vel * scale; 
			int newProj = OrchidModGamblerHelper.DummyProjectile(Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, projType, damage, knockBack, player.whoAmI), dummy);
			Main.projectile[newProj].damage += OrchidModGamblerHelper.getNbGamblerCards(player, modPlayer) * 2;
			Main.projectile[newProj].damage += rand == 3 ? 2 : 0;
			Main.projectile[newProj].netUpdate = true;
			OrchidModProjectile.spawnDustCircle(position + vel * 2f, rand < 2 ? 60 : 63, 5, 10, true, 1.5f, 0.5f, 3f);
			OrchidModProjectile.spawnDustCircle(position + vel * 5f, rand < 2 ? 60 : 63, 3, 5, true, 1.5f, 0.5f, 3f);
			Main.PlaySound(2, (int)player.Center.X ,(int)player.Center.Y - 200, 1);
		}
	}
}
