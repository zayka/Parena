using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PArena
{
    class Bullet_Boss4:Bullet
    {
         static float initSpeed = 200;

         public Bullet_Boss4(Vector2 pos, Vector2 dir, Texture2D tex)
            : base(pos, dir, tex, initSpeed)
        {
            pEmitter.Color = Color.Purple;
            ttl = 10;
            damage = 8;
            color = Color.Purple;
            //pps, pSpeed, 
            //posVar, alphaVel, 
            //minSize,maxSize, sizeVel,
            //ttl
            pEmitter.SetParam(300, 1,
                              1, 1700,
                              0.2f, 0.7f, 0.5f,
                              50);
            
        }
    }
}
