using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Catalysts
{
	public class CrimtaneCatalyst : OrchidModAlchemistCatalyst
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Crimson Catalyst");
			Tooltip.SetDefault("Used to interact with alchemist catalytic elements"
							+ "\nUpon successful catalysis, confuses nearby enemies"
							+ "\nHit an enemy to apply catalyzed"
							+ "\nCatalyzed replaces most alchemical debuffs");
		}
		
		public override void SafeSetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 75, 0);
			this.catalystType = 1;
		}
		
		public override void CatalystInteractionEffect(Player player) {
			for (int k = 0; k < Main.npc.Length; k++)
			{
				if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5)
				{
					Vector2 newMove = Main.npc[k].Center - player.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo < 300f)
					{
						Main.npc[k].AddBuff(31, 5 * 60); // Confused
					}
				}
			}
		}
	}
}
