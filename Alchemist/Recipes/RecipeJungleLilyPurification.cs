﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Recipes
{
	public class RecipeJungleLilyPurification : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = -4;
			this.name = "Lily Purification";
			this.description = "Cleanses most common early-game debuffs and blooms jungle lilies around the user";
			this.debuffDuration = 10;
			this.soundType = 2;
			this.soundID = 85;
			
			this.ingredients.Add(ItemType<Alchemist.Weapons.Nature.JungleLilyFlask>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Air.CorruptionFlask>());
		}
		
		
		public override void Reaction(Player player, OrchidModPlayer modPlayer)
		{
			int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.JungleLilyFlaskReaction>();
			Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, spawnProj, 0, 0f, player.whoAmI);
			OrchidModProjectile.spawnDustCircle(player.Center, 15, 10, 7, true, 1.5f, 1f, 3f);
			OrchidModProjectile.spawnDustCircle(player.Center, 15, 15, 10, true, 1.5f, 1f, 5f);
		}
	}
}