using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
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
			this.sound = SoundID.Item25;
			this.ingredients.Add(ItemType<Weapons.Nature.SunflowerFlask>());
			this.ingredients.Add(ItemType<Weapons.Water.SlimeFlask>());
		}
		
		
		public override void Reaction(Player player, OrchidModPlayer modPlayer)
		{
			int itemType = ItemType<Alchemist.Weapons.Nature.SunflowerFlask>();
			int dmg = OrchidModAlchemistHelper.getSecondaryDamage(player, modPlayer, itemType, 4, true);
			int nb = 5 + Main.rand.Next(4);

			for (int i = 0; i < 5; i++)
			{
				Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(10)).RotatedBy(MathHelper.ToRadians(-40 + (20 * i))));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.SunflowerFlaskProj1>();
				Projectile.NewProjectile(player.GetSource_Misc("Alchemist Hidden Reaction"), player.Center, vel, spawnProj, dmg, 0f, player.whoAmI);
			}

			for (int i = 0; i < nb; i++)
			{
				Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.SunflowerFlaskProj4>();
				Projectile.NewProjectile(player.GetSource_Misc("Alchemist Hidden Reaction"), player.Center, vel, spawnProj, 0, 0f, player.whoAmI);
			}
		}
	}
}
