using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod
{
	public class OrchidModGlobalProjectile : GlobalProjectile
	{
		public int baseCritChance = 0;
		public bool shamanProjectile = false;
		public bool gamblerProjectile = false;
		public bool dancerProjectile = false;
		public bool alchemistProjectile = false;
		public bool alchemistReactiveProjectile = false;
		public int shamanEmpowermentType = 0;
		public int gamblerInternalCooldown = 0;
		public bool gamblerDummyProj = false;
		public bool gamblerBonusTrigger = false;

		public delegate void GamblerBonusProjectilesDelegate(Player player, OrchidModPlayer modPlayer, Projectile projectile, OrchidModGlobalProjectile modProjectile, bool dummy = false);
		public GamblerBonusProjectilesDelegate gamblerBonusProjectilesDelegate;
		public delegate void AlchemistCatalyticTriggerDelegate(Player player, Projectile projectile, OrchidModGlobalProjectile modProjectile);
		public AlchemistCatalyticTriggerDelegate alchemistCatalyticTriggerDelegate;

		public override bool InstancePerEntity => true;

		public override void ModifyHitPlayer(Projectile projectile, Player target, ref int damage, ref bool crit)
		{
			OrchidModPlayer modPlayer = target.GetModPlayer<OrchidModPlayer>();
			if (modPlayer.generalTools && !target.HasBuff(BuffType<General.Buffs.Debuffs.ToolSetBuff>()))
			{
				List<int> trapProjTypes = new List<int>();
				trapProjTypes.Add(98); // Dart
				trapProjTypes.Add(99); // Boulder
				trapProjTypes.Add(186); // Super Dart
				trapProjTypes.Add(185); // Spiky Ball
				trapProjTypes.Add(184); // Spear

				if (trapProjTypes.Contains(projectile.type))
				{
					target.AddBuff(BuffType<General.Buffs.Debuffs.ToolSetBuff>(), 60 * 30);
					damage = (int)(damage * 0.1f);
				}
			}
		}

		public override bool? CanHitNPC(Projectile projectile, NPC target)
		{
			OrchidModGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>();
			Player player = Main.player[projectile.owner];
			if (modProjectile.gamblerDummyProj)
			{
				if (target.type != 488 && player.HeldItem.type != ItemType<Gambler.GamblerDummyTest>())
				{
					return false;
				}
			}
			return null;
		}
	}
}