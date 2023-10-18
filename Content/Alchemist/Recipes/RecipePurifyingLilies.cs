using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Alchemist.Recipes
{
	public class RecipePurifyingLilies : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 2;
			this.name = "Purifying Lilies";
			this.description = "Each alchemical attack using two or more elements releases a purifying aura";
			this.debuffDuration = 15;
			this.sound = SoundID.Item85;
			this.dust = 16;
			this.buff = BuffType<Alchemist.Buffs.JungleLilyExtractBuff>();
			this.buffDuration = 60;
			
			this.ingredients.Add(ItemType<Alchemist.Weapons.Nature.JungleLilyFlask>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Air.CorruptionFlask>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Fire.GunpowderFlask>());
		}
		
		
		public override void Reaction(Player player, OrchidAlchemist modPlayer)
		{
		}
	}
}
