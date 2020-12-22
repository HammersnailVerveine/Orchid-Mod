using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
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
			item.damage = 98;
			item.width = 30;
			item.height = 30;
			item.useTime = 20;
			item.useAnimation = 20;
			item.knockBack = 1.15f;
			item.rare = 8;
			item.value = Item.sellPrice(0, 20, 0, 0);
			item.UseSound = SoundID.Item72;
			item.autoReuse = true;
			item.shootSpeed = 15f;
			item.shoot = mod.ProjectileType("TerraSpecterProj2");
			this.empowermentType = 5;
			this.empowermentLevel = 4;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Terra Scepter");
		  Tooltip.SetDefault("Conjures harmful terra bolts at your enemies"
							+"\nHitting with the main projectile will empower a powerful terra orb"
							+"\nAfter reaching a certain power, or if you lose it, the orb will break free and home into your enemies"
							+"\nThe damage dealt by the orb increases with the accumulated power, and with your shamanic damage"
							+"\nThe blast produced by an orb at its maximum power will increase your shamanic damage for a time"
							+"\nThe number of projectiles shot scales with the number of active shamanic bonds");
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
	    {
			
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			int BuffsCount = 0;
			if (modPlayer.shamanWaterBuff != 0)
				BuffsCount ++;
			
			if (modPlayer.shamanAirBuff != 0)
				BuffsCount ++;
			
			if (modPlayer.shamanEarthBuff != 0)
				BuffsCount ++;
				
			if (modPlayer.shamanSpiritBuff != 0)
				BuffsCount ++;
			
			if (modPlayer.shamanFireBuff != 0)
				BuffsCount ++;
			
			BuffsCount -= BuffsCount > 0 ? 1 : 0;
			
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 50f;
			
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }			
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
                Projectile.NewProjectile(position.X, position.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), mod.ProjectileType("TerraSpecterProj"), (int)(item.damage*0.55), knockBack, item.owner);
            }
			return true;
	    }
		
		public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "TrueSanctify", 1);
			recipe.AddIngredient(null, "TrueDepthsBaton", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}
