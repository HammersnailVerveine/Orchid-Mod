using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Recipes
{
	public class RecipePurifyingLilies : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 2;
			this.name = "Lily Purification";
			this.description = "Cleanses most common early-game debuffs and blooms jungle lilies around the user";
			this.debuffDuration = 15;
			this.soundType = 2;
			this.soundID = 85;
			this.dust = 16;
			this.buff = BuffType<Alchemist.Buffs.JungleLilyExtractBuff>();
			this.buffDuration = 60;
			
			this.ingredients.Add(ItemType<Alchemist.Weapons.Nature.JungleLilyFlask>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Air.CorruptionFlask>());
			this.ingredients.Add(ItemType<Alchemist.Weapons.Fire.GunpowderFlask>());
		}
		
		
		public override void Reaction(Player player, OrchidModPlayer modPlayer)
		{
		}
	}
}
