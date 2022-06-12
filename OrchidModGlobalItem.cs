using Microsoft.Xna.Framework;
using OrchidMod.Alchemist;
using OrchidMod.Alchemist.Projectiles;
using OrchidMod.Common.Globals.NPCs;
using OrchidMod.Common.Interfaces;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace OrchidMod
{
	public class OrchidModGlobalItem : GlobalItem
	{
		public bool shamanWeapon = false;
		public int shamanWeaponElement = 0;
		public bool shamanWeaponNoUsetimeReforge = false;
		public bool shamanWeaponNoVelocityReforge = false;
		public bool alchemistWeapon = false;
		public bool alchemistCatalyst = false;
		public int alchemistColorR = 0;
		public int alchemistColorG = 0;
		public int alchemistColorB = 0;
		public int alchemistRightClickDust = -1;
		public int alchemistPotencyCost = 0;
		public int alchemistSecondaryDamage = 0;
		public float alchemistSecondaryScaling = 1f;
		public int gamblerCardRequirement = 0;
		public bool gamblerDeck = false;
		public bool guardianWeapon = false;
		public List<string> gamblerCardSets = new List<string>();
		public AlchemistElement alchemistElement = AlchemistElement.NULL;

		public delegate void KillFirstDelegate(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem);
		public delegate void KillSecondDelegate(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem);
		public delegate void KillThirdDelegate(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem);
		public delegate void OnHitNPCFirstDelegate(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer, OrchidModAlchemistNPC modTarget, OrchidGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem);
		public delegate void OnHitNPCSecondDelegate(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer, OrchidModAlchemistNPC modTarget, OrchidGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem);
		public delegate void OnHitNPCThirdDelegate(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer, OrchidModAlchemistNPC modTarget, OrchidGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem);
		public delegate void AddVariousEffectsDelegate(Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem);
		public delegate void GamblerShootDelegate(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack, bool dummy = false);

		public KillFirstDelegate killFirstDelegate;
		public KillSecondDelegate killSecondDelegate;
		public KillThirdDelegate killThirdDelegate;
		public OnHitNPCFirstDelegate onHitNPCFirstDelegate;
		public OnHitNPCSecondDelegate onHitNPCSecondDelegate;
		public OnHitNPCThirdDelegate onHitNPCThirdDelegate;
		public AddVariousEffectsDelegate addVariousEffectsDelegate;
		public GamblerShootDelegate gamblerShootDelegate;

		public override bool InstancePerEntity => true;

		protected override bool CloneNewInstances => true;

		public override GlobalItem NewInstance(Item target)
		{
			return base.NewInstance(target);
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			CrossmodTooltips(item, tooltips);
		}

		private void CrossmodTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (item.ModItem is ICrossmodItem crossmod)
			{
				string text = "This is a cross-content item: ";

				if (crossmod.CrossmodName == "Thorium Mod")
				{
					if (OrchidMod.ThoriumMod == null) text += crossmod.CrossmodName;
					else return;
				}
				else text += "Unknown Mod";

				var tooltip = new TooltipLine(Mod, "Crossmod", text)
				{
					OverrideColor = new Color(255, 85, 60)
				};
				tooltips.Add(tooltip);
			}
		}
	}
}