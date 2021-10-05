using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Recipes
{
	public class RecipePoisonousSlime : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 2;
			this.name = "Poisonous Slime";
			this.description = "Increases the likelyhood of spawning slime bubbles, creating spiked jungle slimes";
			this.debuffDuration = 10;
			this.soundType = 2;
			this.soundID = 85;
			this.dust = 44;
			this.buff = BuffType<Alchemist.Buffs.KingSlimeFlaskBuff>();
			this.buffDuration = 60;
			
			this.ingredients.Add(ItemType<Alchemist.Weapons.Water.KingSlimeFlask>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Nature.PoisonVial>()); 
		}
		
		
		public override void Reaction(Player player, OrchidModPlayer modPlayer)
		{
		}
	}
}
