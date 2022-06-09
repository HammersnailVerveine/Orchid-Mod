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
using Terraria.Audio;
using Terraria.GameContent;
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
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shootSpeed = 8f;
			Item.shoot = ModContent.ProjectileType<PrototypeSecrecyProjectile>();
			Item.damage = 13;
			Item.knockBack = 9f;
			Item.width = 20;
			Item.height = 40;
			Item.UseSound = Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Magic_1");
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.noUseGraphic = true;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 1, 20, 0);
			Item.melee = true;
		}

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			Lighting.AddLight(Item.Center, PrototypeSecrecyProjectile.EffectColor.ToVector3() * 0.2f);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			OrchidHelper.DrawSimpleItemGlowmaskInWorld(Item, spriteBatch, ModContent.GetTexture("OrchidMod/Assets/Textures/Items/PrototypeSecrecy_Glow"), Color.White * 0.7f, rotation, scale);
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<PrototypeSecrecyProjectile>()] <= 1; // We need exactly 2, not 1

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(Mod);
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
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.aiStyle = 3;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.melee = true;
		}

		public override void OnSpawn()
		{
			_trail = new RoundedTrail
			(
				target: Projectile,
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
			Lighting.AddLight(Projectile.Center, EffectColor.ToVector3() * 0.2f);

			if (Main.rand.NextBool(3))
			{
				var dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, 0f, 0f)];
				dust.noLight = true;
				dust.noGravity = true;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			var texture = TextureAssets.Projectile[Projectile.type].Value;
			var position = Projectile.position + Projectile.Size * 0.5f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
			spriteBatch.Draw(texture, position, null, lightColor, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);

			texture = ModContent.GetTexture("OrchidMod/Assets/Textures/Items/PrototypeSecrecy_Glow");
			spriteBatch.Draw(texture, position, null, Color.White * 0.7f, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			bool flag = false;
			flag |= target.velocity.X > 0 && Projectile.Center.X < target.Center.X;
			flag |= target.velocity.X < 0 && Projectile.Center.X > target.Center.X;

			if (flag)
			{
				crit = true;
				SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Magic_0"), Projectile.Center);
			}

			Projectile.NewProjectile(Projectile.Center, Vector2.Zero, ModContent.ProjectileType<PrototypeSecrecyHitProjectile>(), 0, 0f, Projectile.owner, flag.ToInt());
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity = -oldVelocity;
			Projectile.NewProjectile(Projectile.Center, Vector2.Zero, ModContent.ProjectileType<PrototypeSecrecyHitProjectile>(), 0, 0f, Projectile.owner, 0);

			return base.OnTileCollide(oldVelocity);
		}

		void IDrawAdditive.DrawAdditive(SpriteBatch spriteBatch)
		{
			var texture = OrchidHelper.GetExtraTexture(11);
			var position = Projectile.position + Projectile.Size * 0.5f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;

			spriteBatch.Draw(texture, position + new Vector2(-4, 0).RotatedBy(Projectile.rotation), null, EffectColor * 0.6f, 0f, texture.Size() * 0.5f, Projectile.scale * 0.4f, SpriteEffects.None, 0);
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
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.damage = 0;

			Projectile.friendly = true;
			Projectile.timeLeft = 20;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
		}

		public override void OnSpawn()
		{
			Projectile.rotation += Main.rand.NextFloat(MathHelper.TwoPi);

			for (int i = 0; i < 8; i++)
			{
				var dust = Dust.NewDustPerfect(Projectile.Center, 60, Vector2.UnitX.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0.5f, 2.5f), 0, default, Main.rand.NextFloat(0.5f, 1.2f));
				dust.noLight = true;
			}
		}

		public override void AI()
		{
			Projectile.friendly = Projectile.timeLeft == 10;
			Projectile.rotation += 0.05f;
			Projectile.scale = OrchidHelper.GradientValue<float>(MathHelper.Lerp, 1 - Projectile.timeLeft / 20f, new float[] { 1f, 1.2f, 0.6f, 0f }) * (Projectile.ai[0] > 0f ? 2.5f : 1f);

			Lighting.AddLight(Projectile.Center, PrototypeSecrecyProjectile.EffectColor.ToVector3() * 0.25f * Projectile.scale);
		}

		public override bool? CanCutTiles() => false;
		public override bool? CanDamage()/* Suggestion: Return null instead of false */ => false;

		void IDrawAdditive.DrawAdditive(SpriteBatch spriteBatch)
		{
			var texture = OrchidHelper.GetExtraTexture(17);
			var position = Projectile.position + Projectile.Size * 0.5f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;

			spriteBatch.Draw(texture, position, null, PrototypeSecrecyProjectile.EffectColor * Projectile.scale, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
		}
	}
}