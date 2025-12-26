using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using OrchidMod.Content.Shaman;
using OrchidMod.Common.ModObjects;
using Terraria.ID;
using System;

namespace OrchidMod
{
	public class OrchidShaman : ModPlayer
	{
        /// <summary> Array position in <c>Main.NPC</c> of the nearest valid target NPC for ShamanWeapons to attack. Set to -1 by default and if the player has no active ShamanWeapons.</summary>
        /// <remarks> Any active ShamanWeapons without special targeting behavior will choose to attack the <c>Center</c> of this NPC and ignore all other NPCs, and do nothing if this NPC is outside of their range, by their default AI.</remarks>
        public int NearestTarget = -1; //most shamanweapon behavior should be consistent assuming this variable stays synced, and it should stay the same so long as the player and enemy positions are synced. If it's inconsistent then consider manually syncing this when the value changes
        /// <summary> Whether this player's FireTarget mirrors the player's movement.</summary>
        public bool FireTargetRelative;
        /// <summary> The position FireShamanWeapons are currently targeting. <c>NearestTarget</c> will be ignored by FireShamanWeapons while this is set. Defaults to <c>Vector2.Zero</c>.</summary>
        public Vector2 FireTarget;
        /// <summary> The position EarthShamanWeapons are currently targeting. The player's position will be ignored by EarthShamanWeapons while this is set. Defaults to <c>Vector2.Zero</c>.</summary>
        public Vector2 EarthTarget;
        /// <summary> Array position in <c>Main.NPC</c> of the nearest valid target NPC from the position of <c>EarthTarget</c>. Set to -1 by default and if <c>EarthTarget</c> is <c>null</c>.
        /// While there is an <c>EarthTarget</c>, any active EarthShamanWeapons will choose to attack the <c>Center</c> of this NPC and ignore all other NPCs, and do nothing if this NPC is outside of their range, by their default AI.</summary>
        public int NearestTargetEarth;
        /// <summary> List of ShamanWeapon projectiles this player owns.</summary>
        public List<Projectile> ShamanWeapons = new();
        
        /// <summary> Alias for <c>FireTarget != Vector2.Zero</c></summary>
        public bool HasFireTarget => FireTarget != Vector2.Zero;
        
        /// <summary> Alias for <c>EarthTarget != Vector2.Zero</c></summary>
        public bool HasEarthTarget => EarthTarget != Vector2.Zero;

		public override void PostUpdateMiscEffects()
		{
            ShamanWeapons.RemoveAll(x => !x.active || x.ModProjectile is not ShamanWeaponAnchor);

			foreach (Projectile shamanWeapon in ShamanWeapons)
            {
                if (shamanWeapon.ModProjectile is ShamanWeaponAnchor)
                {
                    //Dust.NewDustPerfect(shamanWeapon.Center, DustID.TheDestroyer);
                }
            }

            if (ShamanWeapons.Count > 0)
            {
                float nearestDist = NearestTarget != -1 && OrchidModProjectile.IsValidTarget(Main.npc[NearestTarget]) ? Main.npc[NearestTarget].Distance(Player.Center) : 16000;
                float nearestDistEarth = HasEarthTarget && NearestTargetEarth != -1 && OrchidModProjectile.IsValidTarget(Main.npc[NearestTargetEarth]) ? Main.npc[NearestTargetEarth].Distance(EarthTarget) : 16000;
                foreach (NPC npc in Main.npc)
                {
                    if (OrchidModProjectile.IsValidTarget(npc))
                    {
                        float dist = npc.Distance(Player.Center);
                        if (dist < nearestDist)
                        {
                            NearestTarget = npc.whoAmI;
                            nearestDist = dist;
                        }
                        if (HasEarthTarget)
                        {
                            dist = npc.Distance(EarthTarget);
                            if (npc.Distance(EarthTarget) < nearestDistEarth)
                            {
                                NearestTargetEarth = npc.whoAmI;
                                nearestDistEarth = dist;
                            }
                        }
                    }
                }
                if (HasFireTarget) 
                {
                    Vector2 fireTargetVisualPosition = new Vector2((float)Math.Sin(Main.timeForVisualEffects * 0.06f) * 16f, 0).RotatedBy(Main.timeForVisualEffects * 0.1f);
                    for (int i = 0; i < 2; i++)
                    {
                        Dust dust = Dust.NewDustPerfect(FireTarget + fireTargetVisualPosition, DustID.Torch, Scale: 1.5f);
                        dust.velocity *= 0.5f;
                        dust.noLight = true;
                        dust.noGravity = true;
                        fireTargetVisualPosition = -fireTargetVisualPosition;
                    }
                }
            }
		}
	}
}