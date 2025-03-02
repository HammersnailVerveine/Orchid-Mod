using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Alchemist.Recipes
{
	public class RecipeBubbles : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 2;
			this.debuffDuration = 15;
			this.sound = SoundID.Item85;
			this.ingredients.Add(ItemType<Alchemist.Weapons.Nature.PoisonVial>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Water.SeafoamVial>());
		}
		
		
		public override void Reaction(Player player, OrchidAlchemist modPlayer)
		{
			int itemType = ItemType<Alchemist.Weapons.Nature.PoisonVial>();
			int dmg = modPlayer.GetSecondaryDamage(itemType, 4, true);

			for (int i = 0; i < 7; i++)
			{
				int spawnProj = Main.rand.Next(2) == 0 ? ProjectileType<Content.Alchemist.Projectiles.Water.SeafoamVialProj>() : ProjectileType<Content.Alchemist.Projectiles.Nature.PoisonVialProj>();
				Projectile.NewProjectile(player.GetSource_Misc("Alchemist Hidden Reaction"), player.Center.X - 120 + i * 40, player.Center.Y, 0f, -(float)(3 + Main.rand.Next(4)) * 0.5f, spawnProj, dmg, 0f, player.whoAmI);
			}

			for (int i = 0; i < 11; i++)
			{
				Vector2 vel = (new Vector2(0f, -(float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(10)));
				int spawnProj = Main.rand.Next(2) == 0 ? ProjectileType<Content.Alchemist.Projectiles.Water.SeafoamVialProjAlt>() : ProjectileType<Content.Alchemist.Projectiles.Nature.PoisonVialProjAlt>();
				Projectile.NewProjectile(player.GetSource_Misc("Alchemist Hidden Reaction"), player.Center.X - 150 + i * 30, player.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, player.whoAmI);
			}
		}
	}
}
