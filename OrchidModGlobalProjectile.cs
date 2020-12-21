using System;
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
	}
}  