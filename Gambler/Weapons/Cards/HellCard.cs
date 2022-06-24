using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class HellCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 48;
			Item.crit = 4;
			Item.knockBack = 3f;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.shootSpeed = 13f;
			this.cardRequirement = 3;
			this.gamblerCardSets.Add("Biome");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Hell");
			Tooltip.SetDefault("Launches fiery mortar"
							+ "\nPeriodically summons a pepper, replicating the attack");
		}

		public override void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, bool dummy = false)
		{
			SoundEngine.PlaySound(SoundID.Item1);
			int projType = ProjectileType<Projectiles.HellCardProjAlt>();
			
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
