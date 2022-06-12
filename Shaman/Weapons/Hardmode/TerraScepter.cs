using Microsoft.Xna.Framework;
using OrchidMod.Shaman.Projectiles.OreOrbs.Unique;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class TerraScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 98;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.knockBack = 1.15f;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.UseSound = SoundID.Item72;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			Item.shoot = ModContent.ProjectileType<TerraSpecterProj2>();
			this.empowermentType = 5;
			this.energy = 5;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Terra Scepter");
			Tooltip.SetDefault("Hits will grow a terra orb, breaking free after reacing a certain power"
							  + "\nHitting with the orb increases your shamanic damage"
							  + "\nShoots more projectiles based on the number of active shamanic bonds");
		}
		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int BuffsCount = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod);
			BuffsCount -= BuffsCount > 0 ? 1 : 0;

			float spread = 0.0574f;
			float baseSpeed = (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
			double startAngle = Math.Atan2(velocity.X, velocity.Y) - spread;
			double deltaAngle = spread;
			double offsetAngle = startAngle - deltaAngle * 5;
			if (BuffsCount > 3) offsetAngle -= deltaAngle * 4;

			int typeAlt = ModContent.ProjectileType<TerraSpecterProj>();
			int newDamage = (int)(damage * 0.55);
			for (int i = 0; i < BuffsCount; i++)
			{
				offsetAngle += deltaAngle * 4;
				Vector2 newVelocity = new Vector2(baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle));
				this.NewShamanProjectile(player, source, position, newVelocity, typeAlt, newDamage, knockback);
			}
			return true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AddRecipes() => CreateRecipe()
			.AddIngredient(ModContent.ItemType<TrueSanctify>(), 1)
			.AddIngredient(ModContent.ItemType<TrueDepthsBaton>(), 1)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
