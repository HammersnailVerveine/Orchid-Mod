using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Recipes
{
	public class RecipeLivingBeehive : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 2;
			this.name = "Living Beehive";
			this.description = "Every attack using 3 or more element will summon bees";
			this.debuffDuration = 25;
			this.sound = SoundID.Item97;
			this.dust = 153;
			this.buff = BuffType<Alchemist.Buffs.QueenBeeFlaskBuff>();
			this.buffDuration = 60;
			
			this.ingredients.Add(ItemType<Alchemist.Weapons.Air.QueenBeeFlask>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Fire.GunpowderFlask>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Nature.JungleLilyFlask>()); 
		}
		
		
		public override void Reaction(Player player, OrchidModPlayerAlchemist modPlayer)
		{
		}
	}
}
