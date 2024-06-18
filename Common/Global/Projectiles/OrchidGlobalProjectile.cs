using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace OrchidMod.Common.Global.Projectiles
{
	public class OrchidGlobalProjectile : GlobalProjectile
	{
		public bool shamanProjectile = false;
		public bool gamblerProjectile = false;
		public bool dancerProjectile = false;
		public bool alchemistProjectile = false;
		public bool alchemistReactiveProjectile = false;
		public int shamanEmpowermentType = 0;
		public int gamblerInternalCooldown = 0;
		public bool gamblerDummyProj = false;
		public bool gamblerBonusTrigger = false;

		public delegate void GamblerBonusProjectilesDelegate(Player player, OrchidGambler modPlayer, Projectile projectile, OrchidGlobalProjectile modProjectile, bool dummy = false);
		public GamblerBonusProjectilesDelegate gamblerBonusProjectilesDelegate;
		public delegate void AlchemistCatalyticTriggerDelegate(Player player, Projectile projectile, OrchidGlobalProjectile modProjectile);
		public AlchemistCatalyticTriggerDelegate alchemistCatalyticTriggerDelegate;

		public override bool InstancePerEntity => true;

		public override bool? CanHitNPC(Projectile projectile, NPC target)
		{
			OrchidGlobalProjectile modProjectile = projectile.GetGlobalProjectile<OrchidGlobalProjectile>();
			Player player = Main.player[projectile.owner];
			if (modProjectile.gamblerDummyProj)
			{
				if (target.type != NPCID.TargetDummy && player.HeldItem.type != ItemType<Content.Gambler.GamblerDummyTest>())
				{
					return false;
				}
			}
			return null;
		}
	}
}