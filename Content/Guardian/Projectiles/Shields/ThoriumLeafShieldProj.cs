using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OrchidMod.Content.Guardian.Projectiles.Shields
{
    public class ThoriumLeafShieldProj : OrchidModGuardianProjectile
	{
        public override void SafeSetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.timeLeft = 60;
            Main.projFrames[Projectile.type] = 5;
            DrawOffsetX = -2;
            DrawOriginOffsetY = -4;
            Projectile.frame = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
        }

        ref float dir => ref Projectile.ai[0];
        ref float spin => ref Projectile.ai[1];

        public override void AI()
        {
            if (spin > 0)
            {
                spin--;
                Projectile.rotation = dir + 1.571f + (float)Math.Pow(1 + spin * 0.25f, 2) * 0.2f;
                Projectile.velocity *= 0.95f;
                Projectile.velocity += new Vector2(1 / (1 + Math.Abs(spin)), 0).RotatedBy(dir);
                if (Math.Abs(spin) <= 6)
                {
                    Projectile.frame = 1;
                }
            }
            else
            {
                if (Projectile.frame == 1)
                {
                    Projectile.penetrate = 1;
                    Projectile.tileCollide = true;
                    Projectile.rotation = dir + 1.571f;
                }
                Projectile.velocity *= 0.99f;
                Projectile.velocity += new Vector2(0.5f, 0).RotatedBy(dir);
                switch(Projectile.frameCounter)
                {
                    case 0:
                        Projectile.frame = 2;
                        break;
                    case 5:
                        Projectile.frame = 3;
                        break;
                    case 10:
                        Projectile.frame = 4;
                        break;
                    case 15:
                        goto case 5;
                }
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 20) Projectile.frameCounter = 0;
            }
        }

		public override void OnKill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.NPCHit11, Projectile.position);
            for (int i = 0; i < 3; i++)
            {
                int dust = Dust.NewDust(Projectile.Center - new Vector2(4, 4), 0, 0, DustID.Grass);
                Main.dust[dust].velocity = Projectile.velocity * 0.3f + Main.dust[dust].velocity * 0.5f;
            }
        }
    }
}