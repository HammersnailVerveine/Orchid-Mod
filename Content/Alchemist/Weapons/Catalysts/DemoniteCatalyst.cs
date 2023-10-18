using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Alchemist.Weapons.Catalysts
{
	public class DemoniteCatalyst : OrchidModAlchemistCatalyst
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 75, 0);
			this.catalystType = 1;

		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Corrupt Catalyst");
			/* Tooltip.SetDefault("Used to interact with alchemist catalytic elements"
							+ "\nUpon successful catalysis, burns nearby enemies in shadowflames"
							+ "\nHit an enemy to apply catalyzed"
							+ "\nCatalyzed replaces most alchemical debuffs"); */
		}

		public override void CatalystInteractionEffect(Player player)
		{
			for (int k = 0; k < Main.npc.Length; k++)
			{
				if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5)
				{
					Vector2 newMove = Main.npc[k].Center - player.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo < 300f)
					{
						Main.npc[k].AddBuff(153, 2 * 60); // Shadowflame
					}
				}
			}
		}
	}
}
