using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod
{
    public class OrchidModGlobalProjectile : GlobalProjectile
    {
		public bool alchemistProjectile = false;
		public bool alchemistReactiveProjectile = false;
		public bool shamanProjectile = false;
		public bool gamblerProjectile = false;
		public int gamblerInternalCooldown  = 0;
		public int baseCritChance = 0;
		
		public override bool InstancePerEntity => true;
		
		public override void ModifyHitPlayer(Projectile projectile, Player target, ref int damage, ref bool crit) {
			OrchidModPlayer modPlayer = target.GetModPlayer<OrchidModPlayer>();
			if (modPlayer.generalTools && !target.HasBuff(BuffType<General.Buffs.Debuffs.ToolSetBuff>())) {
				List<int> trapProjTypes = new List<int>();
				trapProjTypes.Add(98); // Dart
				trapProjTypes.Add(99); // Boulder
				trapProjTypes.Add(186); // Super Dart
				trapProjTypes.Add(185); // Spiky Ball
				trapProjTypes.Add(184); // Spear
				
				if (trapProjTypes.Contains(projectile.type)) {
					target.AddBuff(BuffType<General.Buffs.Debuffs.ToolSetBuff>(), 60 * 30);
					damage = (int)(damage * 0.1f);
				}
			}
		}
	}
}  