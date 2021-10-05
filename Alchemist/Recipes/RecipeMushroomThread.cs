using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Recipes
{
	public class RecipeMushroomThread : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = -2;
			this.name = "Mushroom Thread";
			this.description = "Creates a sample of Mushroom Thread, which can be used to create armor";
			this.debuffDuration = 15;
			this.soundType = 2;
			this.soundID = 25;
			this.dust = 56;
			
			this.ingredients.Add(ItemType<Alchemist.Weapons.Nature.GlowingMushroomVial>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Fire.BlinkrootFlask>());
		}
		
		
		public override void Reaction(Player player, OrchidModPlayer modPlayer)
		{
			player.QuickSpawnItem(ItemType<Alchemist.Misc.MushroomThread>(), 1);
		}
	}
}
