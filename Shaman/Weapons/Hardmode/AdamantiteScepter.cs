using Microsoft.Xna.Framework;
using OrchidMod.Shaman.Projectiles.OreOrbs.Big;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class AdamantiteScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 51;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 50;
			Item.useAnimation = 50;
			Item.knockBack = 4.15f;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 2, 70, 0);
			Item.UseSound = SoundID.Item117;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			Item.shoot = ModContent.ProjectileType<AdamantiteScepterProj>();
			this.empowermentType = 4;
			this.energy = 12;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Adamantite Scepter");
			Tooltip.SetDefault("Shoots a potent adamantite bolt, hitting your enemy 3 times"
							  + "\nHitting the same target with all 3 shots will grant you an adamantite orb"
							  + "\nIf you have 5 adamantite orbs, your attack will be empowered, dealing double damage");
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 newVelocity = velocity;
			OrchidModPlayerShaman modPlayer = new OrchidModPlayerShaman();
			if (modPlayer.orbCountBig >= 15 && modPlayer.shamanOrbBig == ShamanOrbBig.ADAMANTITE)
			{
				newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));
				modPlayer.orbCountBig = -3;
				damage *= 2;
			}

			for (int i = 0; i < 3; i++)
				this.NewShamanProjectile(player, source, position, newVelocity, type, damage, knockback);
			return false;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ItemID.AdamantiteBar, 12)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
