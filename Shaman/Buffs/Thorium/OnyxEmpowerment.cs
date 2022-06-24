using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Buffs.Thorium
{
	public class OnyxEmpowerment : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			DisplayName.SetDefault("Onyx Empowerment");
			Description.SetDefault("Increases armor penetration by 3");
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.GetArmorPenetration(DamageClass.Generic) += 3;
		}
	}
}