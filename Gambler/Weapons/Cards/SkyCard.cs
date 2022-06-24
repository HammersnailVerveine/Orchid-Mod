using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class SkyCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 21;
			Item.crit = 4;
			Item.knockBack = 2f;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.shootSpeed = 8f;
			this.cardRequirement = 3;
			this.gamblerCardSets.Add("Biome");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Sky");
			Tooltip.SetDefault("Calls stars from the sky"
							+ "\nThe stars will sharply turn upon reaching cursor height"
							+ "\nPeriodically summons a skyware banana, replicating the attack");
		}

		public override void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, bool dummy = false)
		{
			SoundEngine.PlaySound(SoundID.Item1);
			int projType = ProjectileType<Gambler.Projectiles.SkyCardProjAlt>();
			
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == projType && proj.owner == player.whoAmI && proj.ai[1] == 0f)
				{
					float distance = (position - proj.Center).Length();
					if (distance < 500f) {
						return;
					}
				}
			}
			
			velocity = new Vector2(0f, -1f).RotatedBy(MathHelper.ToRadians(10));
			int newProjectile = DummyProjectile(Projectile.NewProjectile(source, position, velocity, projType, damage, knockback, player.whoAmI), dummy);
			Main.projectile[newProjectile].ai[1] = 0f;
			Main.projectile[newProjectile].netUpdate = true;
			for (int i = 0; i < 5; i++)
			{
				int dustType = 31;
				Main.dust[Dust.NewDust(player.Center, 10, 10, dustType)].velocity *= 0.25f;
			}
		}
	}
}
