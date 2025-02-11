using OrchidMod.Content.Gambler;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Common
{
	public static class OrchidConditions
	{
		public static Condition DisabledThorium = new Condition("Mods.OrchidMod.Conditions.DisabledThorium", () => OrchidMod.ThoriumMod == null);
		public static Condition SlimeRain = new Condition("Mods.OrchidMod.Conditions.SlimeRain", () => Main.slimeRain);
		public static Condition GamblerSetSlime = new Condition("Mods.OrchidMod.Conditions.GamblerSetSlime", () => Main.LocalPlayer.GetModPlayer<OrchidGambler>().CheckSetCardsInDeck(GamblerCardSet.Slime) > 2);
		public static Condition GamblerSetBoss = new Condition("Mods.OrchidMod.Conditions.GamblerSetBoss", () => Main.LocalPlayer.GetModPlayer<OrchidGambler>().CheckSetCardsInDeck(GamblerCardSet.Boss) > 2);
		public static Condition GamblerSetBiome = new Condition("Mods.OrchidMod.Conditions.GamblerSetBiome", () => Main.LocalPlayer.GetModPlayer<OrchidGambler>().CheckSetCardsInDeck(GamblerCardSet.Biome) > 2);
		public static Condition GamblerSetElemental = new Condition("Mods.OrchidMod.Conditions.GamblerSetElemental", () => Main.LocalPlayer.GetModPlayer<OrchidGambler>().CheckSetCardsInDeck(GamblerCardSet.Elemental) > 2);
		public static Condition AlchemistKnownReactions4 = new Condition("Mods.OrchidMod.Conditions.AlchemistKnownReactions4", () => Main.LocalPlayer.GetModPlayer<OrchidAlchemist>().alchemistKnownReactions.Count > 4);
	}
}
