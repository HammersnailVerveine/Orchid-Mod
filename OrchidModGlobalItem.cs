using Microsoft.Xna.Framework;
using OrchidMod.Alchemist;
using OrchidMod.Alchemist.Projectiles;
using OrchidMod.Common.Interfaces;
using System.Collections.Generic;
using Terraria;
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
		public List<string> gamblerCardSets = new List<string>();
		public AlchemistElement alchemistElement = AlchemistElement.NULL;

		public delegate void KillFirstDelegate(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem);
		public delegate void KillSecondDelegate(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem);
		public delegate void KillThirdDelegate(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem);
		public delegate void OnHitNPCFirstDelegate(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer, OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem);
		public delegate void OnHitNPCSecondDelegate(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer, OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem);
		public delegate void OnHitNPCThirdDelegate(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer, OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem);
		public delegate void AddVariousEffectsDelegate(Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem);
		public delegate void GamblerShootDelegate(Player player, Vector2 position, float speedX, float speedY, int type, int damage, float knockBack, bool dummy = false);

		public KillFirstDelegate killFirstDelegate;
		public KillSecondDelegate killSecondDelegate;
		public KillThirdDelegate killThirdDelegate;
		public OnHitNPCFirstDelegate onHitNPCFirstDelegate;
		public OnHitNPCSecondDelegate onHitNPCSecondDelegate;
		public OnHitNPCThirdDelegate onHitNPCThirdDelegate;
		public AddVariousEffectsDelegate addVariousEffectsDelegate;
		public GamblerShootDelegate gamblerShootDelegate;

		public override bool InstancePerEntity => true;

		public override bool CloneNewInstances
		{
			get
			{
				return true;
			}
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			CrossmodTooltips(item, tooltips);
		}

		private void CrossmodTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (item.modItem is ICrossmodItem crossmod)
			{
				string text = "This is a cross-content item: ";

				if (crossmod.CrossmodName == "Thorium Mod")
				{
					if (OrchidMod.ThoriumMod == null) text += crossmod.CrossmodName;
					else return;
				}
				else text += "Unknown Mod";

				var tooltip = new TooltipLine(mod, "Crossmod", text)
				{
					overrideColor = new Color(255, 85, 60)
				};
				tooltips.Add(tooltip);
			}
		}
	}
}