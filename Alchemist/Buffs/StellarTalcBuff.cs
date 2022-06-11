using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Buffs
{
	public class StellarTalcBuff : ModBuff
	{
		public override string Texture => OrchidAssets.AlchemistBuffsPath + Name;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Orbit");
			Description.SetDefault("Stellar Talc projectiles will orbit around you");
			Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = true;
		}
	}
}