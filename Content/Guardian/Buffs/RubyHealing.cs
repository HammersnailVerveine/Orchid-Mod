using OrchidMod.Content.Guardian.Weapons.Gauntlets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Buffs
{
	public class RubyHealing : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.lifeRegen += 4;

			if (Main.rand.NextBool(30))
			{
				Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.RedTorch);
				dust.noGravity = true;
				dust.noLight = true;
				dust.velocity *= 0.5f;
				dust.scale *= Main.rand.NextFloat(1f, 2f);
			}

			if (player.HeldItem.ModItem is not JewelerGauntlet)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}
}