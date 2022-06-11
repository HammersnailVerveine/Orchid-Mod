using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Buffs
{
	public class AttractiteAuraSmall : ModBuff
	{
		public override string Texture => OrchidAssets.AlchemistBuffsPath + Name;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lesser Attractite Aura");
			Description.SetDefault("Applies attractite to nearby enemies");
			Main.buffNoTimeDisplay[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			OrchidModProjectile.spawnDustCircle(player.Center, 60, 1, 1, true, 1.5f, 1f, 10f, true, true, false, 0, 0, true);
			float distance = 100f;
			for (int k = 0; k < Main.maxNPCs ; k++)
			{
				NPC target = Main.npc[k];
				if (target.active && !target.dontTakeDamage && !target.friendly)
				{
					Vector2 newMove = Main.npc[k].Center - player.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo < distance)
					{
						target.AddBuff(BuffType<Alchemist.Debuffs.Attraction>(), 60);
					}
				}
			}
		}
	}
}