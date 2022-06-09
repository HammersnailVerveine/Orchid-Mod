using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs
{
	public class RubyEmpowerment : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Ruby Empowerment");
			Description.SetDefault("Increases life regeneration");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.lifeRegen += 2;
		}
	}
}