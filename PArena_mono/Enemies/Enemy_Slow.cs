using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PArena
{
    class Enemy_Slow:Enemy
    {
        public Enemy_Slow(Vector2 Pos)
            : base(Pos)
        {
            speed = 25;
            hitpoints = 70;
            sprite = Cnt.game.EnemyTextures["Slow"];            
            Height = sprite.Height;
            Width = sprite.Width;
            origin = new Vector2(Height / 2, Width / 2);
            spotTime = 0;
            bloodColor = Color.Red;
            radius = (float)Math.Sqrt(Height * Height + Width * Width);
        }

        public override void Update(GameTime gt)
        {
            if (!control) AI_1(gt);
            base.Update(gt);
        }
    }
}
