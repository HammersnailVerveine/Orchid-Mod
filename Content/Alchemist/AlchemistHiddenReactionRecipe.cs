using System.Collections.Generic;
using OrchidMod.Common.ModObjects;
using Terraria;
using Terraria.Localization;
using Terraria.Audio;
using Terraria.ID;

namespace OrchidMod.Content.Alchemist
{
	public abstract class AlchemistHiddenReactionRecipe
	{
		public string typeName = "blankName";
		public LocalizedText name => Language.GetOrRegister($"Mods.OrchidMod.AlchemistRecipe.{GetType().Name}.DisplayName", () => "blank");
		public LocalizedText description => Language.GetOrRegister($"Mods.OrchidMod.AlchemistRecipe.{GetType().Name}.Tooltip", () => "blank");
		public List<int> ingredients = new List<int>();
		public int level = 0;
		public int debuffDuration = 0;
		public SoundStyle sound = SoundID.Item1;
		public int buff = 0;
		public int buffDuration = 0;
		public int dust = 0;

		public delegate void RecipeEffect(Player player, OrchidAlchemist modPlayer);
		public RecipeEffect recipeEffect;
		
		public static void BlankEffect(AlchemistHiddenReactionRecipe recipe, Player player, OrchidPlayer modPlayer) {}
		
		public virtual void SetDefaults() {}
		public virtual void Reaction(Player player, OrchidAlchemist modPlayer) {}

		public AlchemistHiddenReactionRecipe()
		{
			this.typeName = this.GetType().Name;
			this.level = 0;
			this.debuffDuration = 0;
			this.sound = SoundID.Item1;
			this.recipeEffect = this.Reaction;
			this.ingredients = new List<int>();
			this.buff = 0;
			this.dust = 0;
			this.buffDuration = 0;
			
			this.SetDefaults();
		}
	}
}
