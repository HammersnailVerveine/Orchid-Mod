using Microsoft.Xna.Framework;
using System;
using Terraria;
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
			Item.rare = 8;
			Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.UseSound = SoundID.Item72;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			Item.shoot = Mod.Find<ModProjectile>("TerraSpecterProj2").Type;
			this.empowermentType = 5;
			this.energy = 5;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Terra Scepter");
			Tooltip.SetDefault("Conjures harmful terra bolts at your enemies"
							  + "\nHitting with the main projectile will empower a powerful terra orb"
							  + "\nAfter reaching a certain power, or if you lose it, the orb will break free and home into your enemies"
							  + "\nThe damage dealt by the orb increases with the accumulated power, and with your shamanic damage"
							  + "\nThe blast produced by an orb at its maximum power will increase your shamanic damage for a time"
							  + "\nThe number of projectiles shot scales with the number of active shamanic bonds");
		}
		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int BuffsCount = OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod);
			BuffsCount -= BuffsCount > 0 ? 1 : 0;

			float spread = 1f * 0.0574f;
			float baseSpeed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
			double startAngle = Math.Atan2(speedX, speedY) - spread;
			double deltaAngle = spread;
			double offsetAngle = startAngle - deltaAngle * 5;
			if (BuffsCount > 3) offsetAngle -= deltaAngle * 4;
			int i;
			for (i = 0; i < BuffsCount; i++)
			{
				offsetAngle += deltaAngle * 4;
				this.NewShamanProjectile(position.X, position.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), Mod.Find<ModProjectile>("TerraSpecterProj").Type, (int)(Item.damage * 0.55), knockBack, Item.playerIndexTheItemIsReservedFor);
			}
			return true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(null, "TrueSanctify", 1);
			recipe.AddIngredient(null, "TrueDepthsBaton", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
			recipe.AddRecipe();
		}
	}
}
