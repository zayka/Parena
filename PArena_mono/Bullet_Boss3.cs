using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PArena
{
    class Bullet_Boss3 : Bullet
    {
        static float initSpeed = 300;

        public Bullet_Boss3(Vector2 pos, Vector2 dir, Texture2D tex)
            : base(pos, dir, tex, initSpeed)
        {
            pEmitter.Color = Color.Gray; 
            ttl = 4;
            damage = 10;
            //pps, pSpeed, 
            //posVar, alphaVel, 
            //minSize,maxSize, sizeVel,
            //ttl
            pEmitter.SetParam(300, 1,
                              1, 2000,
                              0.2f, 0.7f, 0.5f,
                              50);
        }
    }
    
}
