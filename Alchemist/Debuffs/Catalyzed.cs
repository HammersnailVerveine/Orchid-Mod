using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Debuffs
{
	public class Catalyzed : ModBuff
	{
		public override string Texture => OrchidAssets.AlchemistBuffsPath + Name;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Catalyzed");
			Description.SetDefault("Triggers various alchemical properties");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}
}
