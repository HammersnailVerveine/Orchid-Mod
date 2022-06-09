using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Buffs
{
	public class MeteorSpeed : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meteor Agility");
			Description.SetDefault("Immune to knockback, 20% increased movement speed");
			Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.moveSpeed += 0.2f;
			player.noKnockback = true;
		}
	}
}