using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Alchemist.Recipes
{
	public class RecipePermanentFreeze : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 1;
			this.debuffDuration = 15;
			this.sound = SoundID.Item25;
			this.dust = 261;
			this.buff = BuffType<Alchemist.Buffs.IceChestFlaskBuff>();
			this.buffDuration = 60;
			
			this.ingredients.Add(ItemType<Weapons.Water.IceChestFlask>());
			this.ingredients.Add(ItemType<Weapons.Fire.GunpowderFlask>());
			this.ingredients.Add(ItemType<Weapons.Air.CloudInAVial>());
		}
		
		
		public override void Reaction(Player player, OrchidAlchemist modPlayer)
		{
		}
	}
}
