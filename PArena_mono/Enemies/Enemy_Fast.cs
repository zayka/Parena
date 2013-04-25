using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PArena
{
    class Enemy_Fast:Enemy
    {

        public Enemy_Fast(Vector2 Pos)
            : base(Pos)
        {
            speed = 100;
            hitpoints = 20;
            sprite = Cnt.game.EnemyTextures["Fast"];
            Height = sprite.Height;
            Width = sprite.Width;
            origin = new Vector2(Height / 2, Width / 2);
            spotTime = 0;
            bloodColor = Color.Red;
            hasLight = true;
            light.radius = 125;
            radius = (float)Math.Sqrt(Height * Height + Width * Width);
        }

        public override void Update(GameTime gt)
        {
            if (!control) AI_1(gt);
            base.Update(gt);
        }

    }
}
