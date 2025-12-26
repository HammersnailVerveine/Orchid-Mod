using Terraria.ModLoader;
using OrchidMod.Common.ModObjects;

namespace OrchidMod.Content.Shaman
{
	public abstract class OrchidModShamanProjectile : OrchidModProjectile
    {
        //NOTE: ShamanWeaponAnchors are NOT ShamanProjectiles
        //this abstract class is for attacking projectiles spawned by ShamanWeaponAnchors
        //things intended to apply to the anchors themselves should go in the ShamanWeaponAnchor class in OrchidModShamanWeapon

		public sealed override void AltSetDefaults()
		{
			Projectile.DamageType = ModContent.GetInstance<ShamanDamageClass>();
			SafeSetDefaults();
		}
    }
}