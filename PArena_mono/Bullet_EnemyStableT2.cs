using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PArena
{
    class Bullet_EnemyStableT2 : Bullet
    {

        static float initSpeed = 200;
        public Bullet_EnemyStableT2(Vector2 pos, Vector2 dir, Texture2D tex, float mul)
            : base(pos, dir, tex, initSpeed)
        {
            double angle = Game1.rnd.NextDouble() * MathHelper.PiOver4 - MathHelper.PiOver4 / 2;
            Matrix m = Matrix.CreateFromAxisAngle(Vector3.UnitZ, (float)angle);
            Dir = Vector2.Transform(dir, m);
            Dir.Normalize();
            pos += Dir * ((float)Game1.rnd.NextDouble() * 20 - 10);
            pEmitter.Color = Color.Orange;
            //tailLength = 2;
            ttl = 0.4f * (float)Game1.rnd.NextDouble() + 0.6f;
            damage = 20 * mul;
            soundfxString = Cnt.game.shotgunSound;
            /*soundfx = Cnt.game.soundBank.GetCue("shotgun");*/
            //pps, pSpeed, 
            //posVar, alphaVel, 
            //minSize,maxSize, sizeVel,
            //ttl
            pEmitter.SetParam(100, 1.0f,
                              1.0f, 1000,
                              0.2f, 0.2f, 0.05f,
                              50);
            color = Color.Orange;



        }
    }
}
