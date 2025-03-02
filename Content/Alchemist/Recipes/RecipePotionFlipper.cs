using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Alchemist.Recipes
{
	public class RecipePotionFlipper : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 1;
			this.debuffDuration = 30;
			this.sound = SoundID.Item25;
			this.dust = 15;
			this.buff = 109;
			this.buffDuration = 30;
			
			this.ingredients.Add(ItemType<Weapons.Air.ShiverthornFlask>());
			this.ingredients.Add(ItemType<Weapons.Water.WaterleafFlask>());
		}
		
		
		public override void Reaction(Player player, OrchidAlchemist modPlayer)
		{
		}
	}
}
