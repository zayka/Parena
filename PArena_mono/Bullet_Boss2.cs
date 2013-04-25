using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PArena
{
    class Bullet_Boss2:Bullet
    {
        static float initSpeed = 450;

        public Bullet_Boss2(Vector2 pos, Vector2 dir, Texture2D tex)
            : base(pos, dir, tex, initSpeed)
        {
            pEmitter.Color = Color.Blue;
            ttl = 2;
            damage = 15;
            color = Color.Blue;
        }

    }
}
