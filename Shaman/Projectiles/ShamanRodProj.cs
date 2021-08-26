using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
	public class ShamanRodProj : OrchidModShamanProjectile
	{
		private const float length = 16 * 37; // 37 tiles
		private Vector2 startPosition;

		private ref float Progress => ref projectile.ai[0]; // 2 -> 0 -> 2 -> 0 ...

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Leaf");

			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 15;
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;
			projectile.friendly = true;
			projectile.aiStyle = 0;
			projectile.timeLeft = 1801;
			projectile.penetrate = -1;
		}

		public override void OnSpawn()
		{
			projectile.rotation = (float)Math.PI / 3f * projectile.ai[1];
			startPosition = projectile.position;

			Progress = 0;
		}

		public override void AI()
		{
			if (Progress >= 0 && projectile.ai[1] == -1) Progress += projectile.velocity.Length() / length;

			projectile.friendly = projectile.ai[1] == -1;
			projectile.rotation -= 0.2f;
			projectile.position = Vector2.SmoothStep(startPosition + Vector2.Normalize(projectile.velocity) * length, startPosition, Math.Abs(1 - Progress));

			if (projectile.ai[1] == 0)
			{
				Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 65);
				projectile.ai[1] = -1;
				projectile.timeLeft = 1800;
			}

			if (projectile.ai[1] > 0 && projectile.timeLeft % 60 == 0) projectile.ai[1]--;

			if (Progress >= 2) Progress = 0;

			if (projectile.ai[1] == -1 && Main.rand.Next(15) == 0)
			{
				var dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.LeafDust>())];
				dust.scale *= Main.rand.NextFloat(1.25f, 1.75f);
				dust.velocity = new Vector2(Vector2.Normalize(projectile.velocity).X * -0.33f, Main.rand.NextFloat(0.2f, 0.45f));
			}
		}

		public override bool OrchidPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);

			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				float num = ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				Color color = Lighting.GetColor((int)(projectile.Center.X / 16), (int)(projectile.Center.Y / 16), new Color(10, 170, 140)) * num * 0.75f;
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.oldRot[k], drawOrigin, projectile.scale * num, SpriteEffects.None, 0f);
			}

			return true;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 65);

			for (int i = 0; i < 5; i++)
			{
				var dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.LeafDust>())];
				dust.scale *= Main.rand.NextFloat(1.25f, 1.75f);
				dust.velocity = new Vector2(Vector2.Normalize(projectile.velocity).X * 0.2f + Main.rand.NextFloat(-0.15f, 0.15f), Main.rand.NextFloat(0.2f, 0.45f));
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer)
		{
			//if (!(target.boss || target.type == NPCID.TargetDummy) && target.knockBackResist > 0f && slow)

			var owner = Main.player[projectile.owner];

			if (OrchidModShamanHelper.getNbShamanicBonds(owner, owner.GetModPlayer<OrchidModPlayer>(), mod) >= 2 && !target.boss && target.CanBeChasedBy() && target.knockBackResist > 0f)
			{
				target.AddBuff(ModContent.BuffType<Buffs.Debuffs.LeafSlow>(), 60 * 5);
			}
		}
	}
}