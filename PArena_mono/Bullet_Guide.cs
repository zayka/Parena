using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PArena
{
    class Bullet_Guide : Bullet
    {
        static public float initSpeed = 220;
        //new static public int maxBullet = 5;

        public Bullet_Guide(Vector2 pos, Vector2 dir, Texture2D tex, float mul)
            : base(pos, dir, tex, initSpeed)
        {
            ttl = 2;
            damage = 49.0f*mul;
            soundfxString = Cnt.game.guidedSound;
           /* soundfx = Cnt.game.soundBank.GetCue("guided");*/
            pEmitter.Color = Color.Purple;
            //pps, pSpeed, 
            //posVar, alphaVel, 
            //minSize,maxSize, sizeVel,
            //ttl
            pEmitter.SetParam(100, 2.0f,
                              0, 60,
                              0.2f, 0.6f, 0.1f,
                              50);
            color = Color.Purple;

        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
            float min = 9999999;
            Enemy target=null;
            foreach (Enemy e in Cnt.game.EnemyList)
            {
                float curLength = (e.Pos-Pos).LengthSquared();
                if (curLength < min) { min = curLength; target = e; }
            }
            if (target!=null)
            {
                Dir = target.Pos - Pos;
                Dir.Normalize();
                Dir.X += (float)Game1.rnd.NextDouble() * 0.5f;
                Dir.Y += (float)Game1.rnd.NextDouble() * 0.5f;
                Dir.Normalize();
            }
        }

    }
}
