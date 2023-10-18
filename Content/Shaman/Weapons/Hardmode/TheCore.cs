using Microsoft.Xna.Framework;
using OrchidMod.Content.Shaman.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shaman.Weapons.Hardmode
{
	public class TheCore : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 100;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.knockBack = 4.15f;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 10f;
			//Item.shoot = ModContent.ProjectileType<TheCoreProj>();
			this.Element = 4;
			this.energy = 8;
		}

		public override void SafeSetStaticDefaults()
		{
			// DisplayName.SetDefault("The Core");
			/* Tooltip.SetDefault("Shoots life-seeking essence bolts"
							  + "\nThe number of projectiles depends on the number of active shamanic bonds"
							  + "\n'You can feel heartbeats emanating from the staff'"); */
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			int BuffsCount = modPlayer.GetNbShamanicBonds();

			int numberProjectiles = 2 + BuffsCount;

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(10));
				this.NewShamanProjectile(player, source, position, newVelocity, type, damage, knockback);
			}
			return false;
		}
	}
}
