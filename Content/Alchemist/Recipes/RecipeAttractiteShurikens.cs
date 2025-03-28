﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Alchemist.Recipes
{
	public class RecipeAttractiteShurikens : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 1;
			this.debuffDuration = 15;
			this.sound = SoundID.Item25;

			this.ingredients.Add(ItemType<Alchemist.Weapons.Nature.AttractiteFlask>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Water.KingSlimeFlask>());
		}
		
		
		public override void Reaction(Player player, OrchidAlchemist modPlayer)
		{
			player.QuickSpawnItem(player.GetSource_GiftOrReward(), ItemType<Weapons.Misc.AttractiteShuriken>(), 5);
		}
	}
}
