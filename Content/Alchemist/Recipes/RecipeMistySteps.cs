using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Alchemist.Recipes
{
	public class RecipeMistySteps : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 1;
			this.debuffDuration = 20;
			this.sound = SoundID.Item85;
			this.dust = 5;
			this.buff = BuffType<Alchemist.Buffs.BloodMistFlaskBuff>();
			this.buffDuration = 60;
			
			this.ingredients.Add(ItemType<Alchemist.Weapons.Water.BloodMoonFlask>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Air.CloudInAVial>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Nature.GlowingMushroomVial>());
		}
		
		
		public override void Reaction(Player player, OrchidAlchemist modPlayer)
		{
		}
	}
}
