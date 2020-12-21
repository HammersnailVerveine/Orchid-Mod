using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using OrchidMod;
using OrchidMod.Alchemist;

namespace OrchidMod
{
    public class OrchidModGlobalItem : GlobalItem
    {
		public bool shamanWeapon = false;
		public bool shamanWeaponNoUsetimeReforge = false;
		public bool shamanWeaponNoVelocityReforge = false;
		public bool alchemistWeapon = false;
		public bool alchemistCatalyst = false;
		public int alchemistColorR = 0;
		public int alchemistColorG = 0;
		public int alchemistColorB = 0;
		public int alchemistRightClickDust = 0;
		public int alchemistPotencyCost = 0;
		public int gamblerCardRequirement = 0;
		public List<string> gamblerCardSets = new List<string>();
		public AlchemistElement alchemistElement = AlchemistElement.NULL;

		public override bool InstancePerEntity => true;
		
		public override bool CloneNewInstances {
			get
			{
				return true;
			}
		}
	}
}  