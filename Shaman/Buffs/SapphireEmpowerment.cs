using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs
{
	public class SapphireEmpowerment : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Sapphire Empowerment");
			Description.SetDefault("Increases shamanic critical strike chance by 10%");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetCritChance<ShamanDamageClass>() += 10;
		}
	}
}