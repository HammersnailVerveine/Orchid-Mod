using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Alchemist.Recipes
{
	public class RecipeJungleLilyPurification : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = -4;
			this.name = "Lily Purification";
			this.description = "Cleanses most common early-game debuffs and blooms jungle lilies around the user";
			this.debuffDuration = 10;
			this.sound = SoundID.Item85;


			this.ingredients.Add(ItemType<Weapons.Nature.JungleLilyFlask>());
			this.ingredients.Add(ItemType<Weapons.Air.CorruptionFlask>());
		}
		
		
		public override void Reaction(Player player, OrchidAlchemist modPlayer)
		{
			int spawnProj = ProjectileType<Projectiles.Nature.JungleLilyFlaskReaction>();
			Projectile.NewProjectile(player.GetSource_Misc("Alchemist Hidden Reaction"), player.Center, Vector2.Zero, spawnProj, 0, 0f, player.whoAmI);
			OrchidModProjectile.spawnDustCircle(player.Center, 15, 10, 7, true, 1.5f, 1f, 3f);
			OrchidModProjectile.spawnDustCircle(player.Center, 15, 15, 10, true, 1.5f, 1f, 5f);
		}
	}
}
