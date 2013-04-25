using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PArena
{
    class Enemy_Normal:Enemy
    {
        public Enemy_Normal(Vector2 Pos)
            : base(Pos)
        {
            speed = 50;
            hitpoints = 40;
            bloodColor = Color.Green;
            sprite = Cnt.game.EnemyTextures["Normal"];
            Height = sprite.Height;
            Width = sprite.Width;
            origin = new Vector2(Height / 2, Width / 2);
            spotTime = 0;
            bloodColor = Color.Green;
            hasLight = true;
            light.radius = 200;
            radius = (float)Math.Sqrt(Height * Height + Width * Width);
        }

        public override void Update(GameTime gt)
        {
            if (!control) AI_1(gt);
            base.Update(gt);
        }

    }
}
