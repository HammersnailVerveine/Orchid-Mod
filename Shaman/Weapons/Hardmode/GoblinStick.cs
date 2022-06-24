using Microsoft.Xna.Framework;
using OrchidMod.Shaman.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class GoblinStick : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 54;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 25;
			Item.useAnimation = 60;
			Item.knockBack = 3.15f;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.UseSound = SoundID.Item103;
			Item.autoReuse = false;
			Item.shootSpeed = 3f;
			Item.shoot = ModContent.ProjectileType<GoblinStickProj>();
			this.empowermentType = 3;
			this.energy = 7;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Goblin Stick");
			Tooltip.SetDefault("Channels a volley of shadowflame balls"
							  + "\nThe number of projectiles shot during the channeling depends on the number of active shamanic bonds");
		}

		public override void UpdateInventory(Player player)
		{
			OrchidModPlayerShaman modPlayer = player.GetModPlayer<OrchidModPlayerShaman>();
			int nbBonds = modPlayer.GetNbShamanicBonds();
			switch (nbBonds)
			{
				case 0:
					Item.useTime = 60;
					break;
				case 1:
					Item.useTime = 30;
					break;
				case 2:
					Item.useTime = 20;
					break;
				case 3:
					Item.useTime = 15;
					break;
				case 4:
					Item.useTime = 12;
					break;
				case 5:
					Item.useTime = 10;
					break;
			}
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int rand = 1 + Main.rand.Next(2);
			for (int i = 0; i < rand; i++)
			{
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));
				this.NewShamanProjectile(player, source, position, newVelocity, type, damage, knockback);
			}
			return false;
		}
	}
}
