using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Light
{
	public class GlowingVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 5;
			item.width = 24;
			item.height = 24;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.LIGHT;
			this.rightClickDust = 57;
			this.colorR = 253;
			this.colorG = 194;
			this.colorB = 18;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glowing Vial");
			
		    Tooltip.SetDefault("Confuses your target briefly"
							+  "\n[c/FF0000:Test Item]");
		}
		
		public override void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer, 
		OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem) {
			target.AddBuff(BuffID.Confused, 60 * (alchProj.nbElements));
		}
	}
}
