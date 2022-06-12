using Microsoft.Xna.Framework;
using OrchidMod.Shaman.Projectiles.OreOrbs.Unique;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class IchoryCone : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 30;
			Item.width = 50;
			Item.height = 50;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.knockBack = 4.15f;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 7, 50, 0);
			Item.UseSound = SoundID.Item13;
			Item.autoReuse = true;
			Item.shootSpeed = 10f;
			Item.shoot = ModContent.ProjectileType<IchoryConeProj>();
			this.empowermentType = 1;
			this.energy = 4;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Ichor Scepter");
			Tooltip.SetDefault("Sprays your enemies with piercing ichor bursts"
							  + "\nThe first enemy hit will fill an ichor cyst above you"
							  + "\nYour next hit after the cyst is full will release a shower of ichor in the direction you're moving");
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(10));
			this.NewShamanProjectile(player, source, position, newVelocity, type, damage, knockback);
			return false;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ModContent.ItemType<Misc.RitualScepter>(), 1)
			.AddIngredient(ItemID.Ichor, 20)
			.AddIngredient(ItemID.SoulofNight, 15)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
