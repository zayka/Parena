using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PArena
{
    class Bullet_Boss5 : Bullet
    {
        static float initSpeed = 200;

        public Bullet_Boss5(Vector2 pos, Vector2 dir, Texture2D tex)
            : base(pos, dir, tex, initSpeed)
        {
            pEmitter.Color = Color.Ivory;
            ttl = 10;
            damage = 20;
            //pps, pSpeed, 
            //posVar, alphaVel, 
            //minSize,maxSize, sizeVel,
            //ttl
            pEmitter.SetParam(40, 1,
                              0, 1000,
                              0.7f, 0.8f, -0.05f,
                              50);
        }
    }
}
