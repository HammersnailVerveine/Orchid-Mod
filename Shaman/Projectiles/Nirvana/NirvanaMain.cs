using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Nirvana
{
	public class NirvanaMain : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 120;
			Projectile.extraUpdates = 2;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			Projectile.alpha = 255;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nirvana Beam");
		}

		public override void AI()
		{
			Projectile.alpha = 100;
			int index2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 66, 0.0f, 0.0f, 0, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 2.5f);
			Main.dust[index2].scale = (float)Main.rand.Next(70, 110) * 0.013f;
			Main.dust[index2].velocity *= 0.2f;
			Main.dust[index2].noGravity = true;

			if (Projectile.localAI[0] == 0f)
			{
				AdjustMagnitude(ref Projectile.velocity);
				Projectile.localAI[0] = 1f;
			}
			Vector2 move = Vector2.Zero;
			float distance = 150f;
			bool target = false;
			for (int k = 0; k < 200; k++)
			{
				if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && Main.npc[k].type != NPCID.TargetDummy)
				{
					Vector2 newMove = Main.npc[k].Center - Projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo < distance)
					{
						move = newMove;
						distance = distanceTo;
						target = true;
					}
				}
			}
			if (target)
			{
				AdjustMagnitude(ref move);
				Projectile.velocity = (10 * Projectile.velocity + move) / 3f;
				AdjustMagnitude(ref Projectile.velocity);
			}
		}

		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 6f)
			{
				vector *= 6f / magnitude;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			int randF = Main.rand.Next(4);
			int randW = Main.rand.Next(4);
			while (randW == randF)
				randW = Main.rand.Next(4);
			int randE = Main.rand.Next(4);
			while (randE == randF || randE == randW)
				randE = Main.rand.Next(4);
			int randA = Main.rand.Next(4);
			while (randA == randF || randA == randW || randA == randE)
				randA = Main.rand.Next(4);

			int projTypeFire = ModContent.ProjectileType<NirvanaFire>();
			int projTypeWater = ModContent.ProjectileType<NirvanaWater>();
			int projTypeEarth = ModContent.ProjectileType<NirvanaEarth>();
			int projTypeAir = ModContent.ProjectileType<NirvanaWind>();

			if (randF == 0)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X - Main.rand.Next(125, 175), target.Center.Y - Main.rand.Next(250, 300), 0f, 0f, projTypeFire, 100, 3, player.whoAmI);
			if (randF == 1)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X + Main.rand.Next(125, 175), target.Center.Y - Main.rand.Next(250, 300), 0f, 0f, projTypeFire, 100, 3, player.whoAmI);
			if (randF == 2)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X + Main.rand.Next(250, 300), target.Center.Y - Main.rand.Next(250, 300), 0f, 0f, projTypeFire, 100, 3, player.whoAmI);
			if (randF == 3)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X - Main.rand.Next(250, 300), target.Center.Y - Main.rand.Next(250, 300), 0f, 0f, projTypeFire, 100, 3, player.whoAmI);

			if (randW == 0)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X - Main.rand.Next(125, 175), target.Center.Y - Main.rand.Next(250, 300), 0f, 0f, projTypeWater, 100, 3, player.whoAmI);
			if (randW == 1)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X + Main.rand.Next(125, 175), target.Center.Y - Main.rand.Next(250, 300), 0f, 0f, projTypeWater, 100, 3, player.whoAmI);
			if (randW == 2)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X + Main.rand.Next(250, 300), target.Center.Y - Main.rand.Next(250, 300), 0f, 0f, projTypeWater, 100, 3, player.whoAmI);
			if (randW == 3)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X - Main.rand.Next(250, 300), target.Center.Y - Main.rand.Next(250, 300), 0f, 0f, projTypeWater, 100, 3, player.whoAmI);

			if (randE == 0)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X - Main.rand.Next(125, 175), target.Center.Y - Main.rand.Next(250, 300), 0f, 0f, projTypeEarth, 100, 3, player.whoAmI);
			if (randE == 1)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X + Main.rand.Next(125, 175), target.Center.Y - Main.rand.Next(250, 300), 0f, 0f, projTypeEarth, 100, 3, player.whoAmI);
			if (randE == 2)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X + Main.rand.Next(250, 300), target.Center.Y - Main.rand.Next(250, 300), 0f, 0f, projTypeEarth, 100, 3, player.whoAmI);
			if (randE == 3)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X - Main.rand.Next(250, 300), target.Center.Y - Main.rand.Next(250, 300), 0f, 0f, projTypeEarth, 100, 3, player.whoAmI);

			if (randA == 0)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X - Main.rand.Next(125, 175), target.Center.Y - Main.rand.Next(250, 300), 0f, 0f, projTypeAir, 100, 3, player.whoAmI);
			if (randA == 1)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X + Main.rand.Next(125, 175), target.Center.Y - Main.rand.Next(250, 300), 0f, 0f, projTypeAir, 100, 3, player.whoAmI);
			if (randA == 2)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X + Main.rand.Next(250, 300), target.Center.Y - Main.rand.Next(250, 300), 0f, 0f, projTypeAir, 100, 3, player.whoAmI);
			if (randA == 3)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X - Main.rand.Next(250, 300), target.Center.Y - Main.rand.Next(250, 300), 0f, 0f, projTypeAir, 100, 3, player.whoAmI);

			if (modPlayer.GetNbShamanicBonds() > 2)
			{
				if (randF == 3)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X - Main.rand.Next(125, 175), target.Center.Y - Main.rand.Next(350, 400), 0f, 0f, projTypeFire, 100, 3, player.whoAmI);
				if (randF == 2)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X + Main.rand.Next(125, 175), target.Center.Y - Main.rand.Next(350, 400), 0f, 0f, projTypeFire, 100, 3, player.whoAmI);
				if (randF == 1)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X + Main.rand.Next(250, 300), target.Center.Y - Main.rand.Next(350, 400), 0f, 0f, projTypeFire, 100, 3, player.whoAmI);
				if (randF == 0)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X - Main.rand.Next(250, 300), target.Center.Y - Main.rand.Next(350, 400), 0f, 0f, projTypeFire, 100, 3, player.whoAmI);

				if (randW == 3)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X - Main.rand.Next(125, 175), target.Center.Y - Main.rand.Next(350, 400), 0f, 0f, projTypeWater, 100, 3, player.whoAmI);
				if (randW == 2)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X + Main.rand.Next(125, 175), target.Center.Y - Main.rand.Next(350, 400), 0f, 0f, projTypeWater, 100, 3, player.whoAmI);
				if (randW == 1)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X + Main.rand.Next(250, 300), target.Center.Y - Main.rand.Next(350, 400), 0f, 0f, projTypeWater, 100, 3, player.whoAmI);
				if (randW == 0)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X - Main.rand.Next(250, 300), target.Center.Y - Main.rand.Next(350, 400), 0f, 0f, projTypeWater, 100, 3, player.whoAmI);

				if (randE == 3)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X - Main.rand.Next(125, 175), target.Center.Y - Main.rand.Next(350, 400), 0f, 0f, projTypeEarth, 100, 3, player.whoAmI);
				if (randE == 2)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X + Main.rand.Next(125, 175), target.Center.Y - Main.rand.Next(350, 400), 0f, 0f, projTypeEarth, 100, 3, player.whoAmI);
				if (randE == 1)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X + Main.rand.Next(250, 300), target.Center.Y - Main.rand.Next(350, 400), 0f, 0f, projTypeEarth, 100, 3, player.whoAmI);
				if (randE == 0)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X - Main.rand.Next(250, 300), target.Center.Y - Main.rand.Next(350, 400), 0f, 0f, projTypeEarth, 100, 3, player.whoAmI);

				if (randA == 3)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X - Main.rand.Next(125, 175), target.Center.Y - Main.rand.Next(350, 400), 0f, 0f, projTypeAir, 100, 3, player.whoAmI);
				if (randA == 2)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X + Main.rand.Next(125, 175), target.Center.Y - Main.rand.Next(350, 400), 0f, 0f, projTypeAir, 100, 3, player.whoAmI);
				if (randA == 1)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X + Main.rand.Next(250, 300), target.Center.Y - Main.rand.Next(350, 400), 0f, 0f, projTypeAir, 100, 3, player.whoAmI);
				if (randA == 0)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X - Main.rand.Next(250, 300), target.Center.Y - Main.rand.Next(350, 400), 0f, 0f, projTypeAir, 100, 3, player.whoAmI);

			}
		}
	}
}