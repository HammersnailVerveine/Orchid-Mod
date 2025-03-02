using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Alchemist.Recipes
{
	public class RecipeMushroomThread : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = -2;
			this.debuffDuration = 15;
			this.sound = SoundID.Item25;
			this.dust = 56;
			
			this.ingredients.Add(ItemType<Weapons.Nature.GlowingMushroomVial>());
			this.ingredients.Add(ItemType<Weapons.Fire.BlinkrootFlask>());
		}
		
		
		public override void Reaction(Player player, OrchidAlchemist modPlayer)
		{
			player.QuickSpawnItem(player.GetSource_Misc("Alchemist Hidden Reaction"), ItemType<Misc.MushroomThread>(), 1);
		}
	}
}
