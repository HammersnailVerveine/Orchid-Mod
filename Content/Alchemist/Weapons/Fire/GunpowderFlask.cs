using Microsoft.Xna.Framework;
using OrchidMod.Common.Global.Items;
using OrchidMod.Content.Alchemist.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;


namespace OrchidMod.Content.Alchemist.Weapons.Fire
{
	public class GunpowderFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 15;
			Item.width = 30;
			Item.height = 30;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			this.potencyCost = 4;
			this.element = AlchemistElement.FIRE;
			this.rightClickDust = 37;
			this.colorR = 66;
			this.colorG = 66;
			this.colorB = 66;
			this.secondaryDamage = 10;
			this.secondaryScaling = 15f;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Gunpowder Flask");
			/* Tooltip.SetDefault("Creates a small explosion"
							+ "\nCan be used to trigger catalytic elements, but prevents them from spawning"); */
		}

		public override void KillSecond(int timeLeft, Player player, OrchidAlchemist modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidGlobalItemPerEntity globalItem)
		{
			int dmg = GetSecondaryDamage(player, alchProj.nbElements);
			SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, Vector2.Zero, ProjectileType<Content.Alchemist.Projectiles.Fire.GunpowderFlaskProj>(), dmg, 3f, projectile.owner, 0.0f, 0.0f);
			SoundEngine.PlaySound(SoundID.Item14);
		}
	}
}
