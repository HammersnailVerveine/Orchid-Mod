using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using System;
using OrchidMod.Interfaces;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
    public class ShroomiteScepter : OrchidModShamanItem, IGlowingItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 93;
			item.width = 44;
			item.height = 44;
			item.useTime = 40;
			item.useAnimation = 40;
			item.knockBack = 1.15f;
			item.rare = ItemRarityID.Yellow;
			item.value = Item.sellPrice(0, 20, 0, 0);
			item.UseSound = SoundID.Item45;
			item.autoReuse = false;
			item.shootSpeed = 15f;
			item.shoot = ModContent.ProjectileType<Projectiles.ShroomiteScepterProj>();
			this.empowermentType = 4;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloom Shroom");
			Tooltip.SetDefault("Summons a protective shroom, harming nearby enemies"
				+ "\nHaving 3 or more active shamanic bonds weakens hit targets"
				+ "\nHaving 5 active shamanic bonds increases nearby players health regeneration");
		}

		public override bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) position += muzzleOffset;

			int projectileType = ModContent.ProjectileType<Projectiles.ShroomiteScepterProj>();
			int nbBonds = OrchidModShamanHelper.getNbShamanicBonds(player, player.GetModPlayer<OrchidModPlayer>(), mod);

			if (player.ownedProjectileCounts[projectileType] > 0)
			{
				var oldProjs = Array.FindAll(Main.projectile, i => i.active && i.type == projectileType && i.owner == player.whoAmI);
				foreach (var elem in oldProjs) elem?.Kill();
			}

			var projectile = CreateNewProjectile(item, player, projectileType);
			projectile.ai[1] = nbBonds;
			projectile.netUpdate = true;
			
			return false;
		}

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			Color color = new Color(0.3f, 0.35f, 0.9f) * 0.25f;
			Lighting.AddLight(item.Center, color.R / 255f, color.G / 255f, color.B / 255f);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			OrchidHelper.DrawSimpleItemGlowmaskInWorld(item, spriteBatch, ModContent.GetTexture("OrchidMod/Glowmasks/ShroomiteScepter_Glowmask"), Color.White, rotation, scale);
		}

		public void DrawItemGlowmask(PlayerDrawInfo drawInfo)
		{
			OrchidHelper.DrawSimpleItemGlowmaskOnPlayer(drawInfo, ModContent.GetTexture("OrchidMod/Glowmasks/ShroomiteScepter_Glowmask"));
		}

		private Projectile CreateNewProjectile(Item item, Player player, int projectileType)
		{
			Point point = new Point((int)((float)Main.mouseX + Main.screenPosition.X) / 16, (int)((float)Main.mouseY + Main.screenPosition.Y) / 16);
			if (player.gravDir == -1f) point.Y = (int)(Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY) / 16;
			while (point.Y < Main.maxTilesY - 10 && Main.tile[point.X, point.Y] != null && !WorldGen.SolidTile2(point.X, point.Y) && Main.tile[point.X - 1, point.Y] != null && !WorldGen.SolidTile2(point.X - 1, point.Y) && Main.tile[point.X + 1, point.Y] != null && !WorldGen.SolidTile2(point.X + 1, point.Y))
			{
				point.Y++;
			}
			var projectile = Projectile.NewProjectile(Main.mouseX + Main.screenPosition.X, point.Y * 16 - 22, 0f, 15f, projectileType, item.damage, item.knockBack, player.whoAmI, 0f, 0f);
			return Main.projectile[projectile];
		}
	}
}
