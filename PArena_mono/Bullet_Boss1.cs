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
    class Bullet_Boss1:Bullet
    {
        static float initSpeed = 600;

        public Bullet_Boss1(Vector2 pos, Vector2 dir, Texture2D tex)
            : base(pos, dir, tex, initSpeed)
        {
            pEmitter.Color = Color.Red;
            ttl = 2;
            damage = 30;
        }
    }
}
