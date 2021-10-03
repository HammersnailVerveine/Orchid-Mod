using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Recipes
{
	public class RecipeSunflowerSeeds : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = -1;
			this.name = "Sunflower Seeds";
			this.description = "Releases damaging sunflower seeds around the player";
			this.debuffDuration = 15;
			this.soundType = 2;
			this.soundID = 85;
			
			this.ingredients.Add(ItemType<Alchemist.Weapons.Nature.SunflowerFlask>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Water.SlimeFlask>());
		}
		
		
		public override void Reaction(Player player, OrchidModPlayer modPlayer)
		{
			int itemType = ItemType<Alchemist.Weapons.Nature.SunflowerFlask>();
			int dmg = OrchidModAlchemistHelper.getSecondaryDamage(player, modPlayer, itemType, 4, true);
			int nb = 5 + Main.rand.Next(4);

			for (int i = 0; i < 5; i++)
			{
				Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(10)).RotatedBy(MathHelper.ToRadians(-40 + (20 * i))));
				Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Nature.SunflowerFlaskProj1>(), dmg, 0f, player.whoAmI);
			}

			for (int i = 0; i < nb; i++)
			{
				Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.SunflowerFlaskProj4>();
				Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
			}
		}
	}
}
