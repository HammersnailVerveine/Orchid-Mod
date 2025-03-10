using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Alchemist.Recipes
{
	public class RecipePoisonousSlime : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 2;
			this.debuffDuration = 10;
			this.sound = SoundID.Item85;
			this.dust = 44;
			this.buff = BuffType<Buffs.KingSlimeFlaskBuff>();
			this.buffDuration = 60;
			
			this.ingredients.Add(ItemType<Weapons.Water.KingSlimeFlask>());
			this.ingredients.Add(ItemType<Weapons.Nature.PoisonVial>()); 
		}
		
		
		public override void Reaction(Player player, OrchidAlchemist modPlayer)
		{
		}
	}
}
