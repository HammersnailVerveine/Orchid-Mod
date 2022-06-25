using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class OceanCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 25;
			Item.knockBack = 5f;
			Item.useAnimation = 50;
			Item.useTime = 50;
			Item.shootSpeed = 5f;
			this.cardRequirement = 1;
			this.gamblerCardSets.Add("Biome");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Ocean");
			Tooltip.SetDefault("Throws rolling coconuts"
							+ "\nPeriodically summons a seed, replicating the attack");
		}

		public override void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback, bool dummy = false)
		{
			SoundEngine.PlaySound(SoundID.Item1);
			int projType = ProjectileType<Projectiles.OceanCardProjAlt>();
			
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
