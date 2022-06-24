using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.OreOrbs.Unique
{
	public class TerraSpecterProj2 : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 40;
			Projectile.scale = 1f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terric Magic");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 0.010f, 0.010f, 0f);
			for (int i = 0; i < 2; i++)
			{
				Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
				int dust = Dust.NewDust(pos, Projectile.width, Projectile.height / 2, 169, Projectile.velocity.X * 0f, Projectile.velocity.Y * 0f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1f;
				Main.dust[dust].velocity /= 10f;
				Main.dust[dust].noLight = true;

				int dust2 = Dust.NewDust(pos, Projectile.width, Projectile.height / 2, 259, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f);
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].scale = 1f;
				Main.dust[dust2].velocity /= 10f;
				Main.dust[dust2].noLight = true;

				int dust3 = Dust.NewDust(pos, Projectile.width, Projectile.height / 2, 246, Projectile.velocity.X * 2f, Projectile.velocity.Y * 2f);
				Main.dust[dust3].noGravity = true;
				Main.dust[dust3].scale = 1f;
				Main.dust[dust3].velocity /= 10f;
				Main.dust[dust3].noLight = true;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			SoundEngine.PlaySound(SoundID.Item10);
			return true;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 13; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 269);
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayerShaman modPlayer)
		{
			if (modPlayer.shamanOrbUnique != ShamanOrbUnique.TERRA)
			{
				modPlayer.shamanOrbUnique = ShamanOrbUnique.TERRA;
				modPlayer.orbCountUnique = 0;
			}
			modPlayer.orbCountUnique++;
			//modPlayer.sendOrbCountPackets();

			if (player.FindBuffIndex(Mod.Find<ModBuff>("ShamanicBaubles").Type) > -1 && modPlayer.orbCountUnique < 5)
			{
				modPlayer.orbCountUnique += 5;
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 79, 0f, 0f, Mod.Find<ModProjectile>("TerraScepterOrb1").Type, 0, 0, Projectile.owner, 0f, 0f);
				player.ClearBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type);
				//modPlayer.sendOrbCountPackets();
			}

			if (modPlayer.orbCountUnique == 5)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 79, 0f, 0f, Mod.Find<ModProjectile>("TerraScepterOrb1").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountUnique == 10)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 79, 0f, 0f, Mod.Find<ModProjectile>("TerraScepterOrb2").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountUnique == 15)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 79, 0f, 0f, Mod.Find<ModProjectile>("TerraScepterOrb3").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountUnique == 20)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 79, 0f, 0f, Mod.Find<ModProjectile>("TerraScepterOrb4").Type, 0, 0, Projectile.owner, 0f, 0f);
		}
	}
}