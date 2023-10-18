using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Buffs
{
	public class EmeraldEmpowerment : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			// DisplayName.SetDefault("Emerald Empowerment");
			// Description.SetDefault("Increases movement speed by 10%");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.moveSpeed += 0.1f;
		}
	}
}