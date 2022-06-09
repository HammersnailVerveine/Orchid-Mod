using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs
{
	public class SpiritualBurst : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Spiritual Burst");
			Description.SetDefault("Empowers the Reviver of Souls weapon"
								+ "\nAll your attacks with it will refresh every single of your active shamanic bonds"
								+ "\nThey will also increase the weapon damage even more per hit");
			Main.buffNoSave[Type] = true;
		}
	}
}