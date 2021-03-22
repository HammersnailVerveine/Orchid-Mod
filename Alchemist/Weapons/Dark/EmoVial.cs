using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;


namespace OrchidMod.Alchemist.Weapons.Dark
{
	public class EmoVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 5;
			item.width = 24;
			item.height = 24;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.DARK;
			this.rightClickDust = 27;
			this.colorR = 182;
			this.colorG = 27;
			this.colorB = 248;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Emo Vial");
			
		    Tooltip.SetDefault("Briefly shadowburns your target"
							+  "\n[c/FF0000:Test Item]");
		}
		
		public override void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer, 
		OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem) {
			target.AddBuff(153, 60 * ((((int)(alchProj.nbElements / 2)) == 0) ? 1 : ((int)(alchProj.nbElements / 2)))); // Shadowflame
		}
	}
}
