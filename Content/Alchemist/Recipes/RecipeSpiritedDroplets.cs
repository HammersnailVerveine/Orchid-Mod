using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Alchemist.Recipes
{
	public class RecipeSpiritedDroplets : AlchemistHiddenReactionRecipe
	{
		public override void SetDefaults()
		{
			this.level = 3;
			this.name = "Spirited Droplets";
			this.description = "Chemical attacks will release spirited water flames";
			this.debuffDuration = 20;
			this.sound = SoundID.Item85;
			this.dust = 29;
			this.buff = BuffType<Buffs.SpiritedWaterBuff>();
			this.buffDuration = 60;
			
			this.ingredients.Add(ItemType<Weapons.Water.DungeonFlask>());
			this.ingredients.Add(ItemType<Weapons.Fire.GunpowderFlask>());
			this.ingredients.Add(ItemType<Weapons.Nature.AttractiteFlask>());
		}
		
		
		public override void Reaction(Player player, OrchidAlchemist modPlayer)
		{
		}
	}
}
