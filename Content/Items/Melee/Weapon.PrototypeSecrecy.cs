using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Common.Interfaces;
using OrchidMod.Content.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Items.Melee
{
	public class PrototypeSecrecy : OrchidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prototype Secrecy");
			Tooltip.SetDefault("Damaging to the back guarantees a critical hit");
		}

		public override void SetDefaults()
		{
			item.noMelee = true;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.shootSpeed = 8f;
			item.shoot = ModContent.ProjectileType<PrototypeSecrecyProjectile>();
			item.damage = 13;
			item.knockBack = 9f;
			item.width = 20;
			item.height = 40;
			item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Magic_1");
			item.useAnimation = 15;
			item.useTime = 15;
			item.noUseGraphic = true;
			item.rare = ItemRarityID.Green;
			item.value = Item.sellPrice(0, 1, 20, 0);
			item.melee = true;
		}

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			Lighting.AddLight(item.Center, PrototypeSecrecyProjectile.EffectColor.ToVector3() * 0.2f);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			OrchidHelper.DrawSimpleItemGlowmaskInWorld(item, spriteBatch, ModContent.GetTexture("OrchidMod/Assets/Textures/Items/PrototypeSecrecy_Glow"), Color.White * 0.7f, rotation, scale);
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<PrototypeSecrecyProjectile>()] <= 1; // We need exactly 2, not 1

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(ItemID.EnchantedBoomerang, 1);
			recipe.AddIngredient(ModContent.ItemType<General.Items.Sets.StaticQuartz.StaticQuartz>(), 6);
			recipe.AddIngredient(ItemID.Silk, 2);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class PrototypeSecrecyProjectile : OrchidProjectile, IDrawAdditive
	{
		public static readonly Color EffectColor = new Color(224, 39, 83);
		private PrimitiveTrailSystem.Trail _trail;

		public override string Texture => "OrchidMod/Assets/Textures/Items/PrototypeSecrecy";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prototype Secrecy");
		}

		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 22;
			projectile.aiStyle = 3;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
		}

		public override void OnSpawn()
		{
			_trail = new RoundedTrail
			(
				target: projectile,
				length: 16 * 10,
				width: (p) => 4 * (1 - p),
				color: (p) => EffectColor * (1 - p),
				additive: true,
				smoothness: 20
			);
			PrimitiveTrailSystem.NewTrail(_trail);
		}

		public override void AI()
		{
			_trail?.SetCustomPositionMethod((proj) => proj.Center + new Vector2(-4, 0).RotatedBy((proj as Projectile).rotation));
			Lighting.AddLight(projectile.Center, EffectColor.ToVector3() * 0.2f);

			if (Main.rand.NextBool(3))
			{
				var dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, 60, 0f, 0f)];
				dust.noLight = true;
				dust.noGravity = true;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var texture = Main.projectileTexture[projectile.type];
			var position = projectile.position + projectile.Size * 0.5f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
			spriteBatch.Draw(texture, position, null, lightColor, projectile.rotation, texture.Size() * 0.5f, projectile.scale, SpriteEffects.None, 0);

			texture = ModContent.GetTexture("OrchidMod/Assets/Textures/Items/PrototypeSecrecy_Glow");
			spriteBatch.Draw(texture, position, null, Color.White * 0.7f, projectile.rotation, texture.Size() * 0.5f, projectile.scale, SpriteEffects.None, 0);
			return false;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			bool flag = false;
			flag |= target.velocity.X > 0 && projectile.Center.X < target.Center.X;
			flag |= target.velocity.X < 0 && projectile.Center.X > target.Center.X;

			if (flag)
			{
				crit = true;
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Magic_0"), projectile.Center);
			}

			Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<PrototypeSecrecyHitProjectile>(), 0, 0f, projectile.owner, flag.ToInt());
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.velocity = -oldVelocity;
			Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<PrototypeSecrecyHitProjectile>(), 0, 0f, projectile.owner, 0);

			return base.OnTileCollide(oldVelocity);
		}

		void IDrawAdditive.DrawAdditive(SpriteBatch spriteBatch)
		{
			var texture = OrchidHelper.GetExtraTexture(11);
			var position = projectile.position + projectile.Size * 0.5f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;

			spriteBatch.Draw(texture, position + new Vector2(-4, 0).RotatedBy(projectile.rotation), null, EffectColor * 0.6f, 0f, texture.Size() * 0.5f, projectile.scale * 0.4f, SpriteEffects.None, 0);
		}
	}

	public class PrototypeSecrecyHitProjectile : OrchidProjectile, IDrawAdditive
	{
		public override string Texture => "OrchidMod/Assets/Textures/Misc/Invisible";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("");
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.damage = 0;

			projectile.friendly = true;
			projectile.timeLeft = 20;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
		}

		public override void OnSpawn()
		{
			projectile.rotation += Main.rand.NextFloat(MathHelper.TwoPi);

			for (int i = 0; i < 8; i++)
			{
				var dust = Dust.NewDustPerfect(projectile.Center, 60, Vector2.UnitX.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0.5f, 2.5f), 0, default, Main.rand.NextFloat(0.5f, 1.2f));
				dust.noLight = true;
			}
		}

		public override void AI()
		{
			projectile.friendly = projectile.timeLeft == 10;
			projectile.rotation += 0.05f;
			projectile.scale = OrchidHelper.GradientValue<float>(MathHelper.Lerp, 1 - projectile.timeLeft / 20f, new float[] { 1f, 1.2f, 0.6f, 0f }) * (projectile.ai[0] > 0f ? 2.5f : 1f);

			Lighting.AddLight(projectile.Center, PrototypeSecrecyProjectile.EffectColor.ToVector3() * 0.25f * projectile.scale);
		}

		public override bool? CanCutTiles() => false;
		public override bool CanDamage() => false;

		void IDrawAdditive.DrawAdditive(SpriteBatch spriteBatch)
		{
			var texture = OrchidHelper.GetExtraTexture(17);
			var position = projectile.position + projectile.Size * 0.5f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;

			spriteBatch.Draw(texture, position, null, PrototypeSecrecyProjectile.EffectColor * projectile.scale, projectile.rotation, texture.Size() * 0.5f, projectile.scale, SpriteEffects.None, 0);
		}
	}
}