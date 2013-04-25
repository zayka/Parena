using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PArena
{
    class Enemy_StableT2 : Enemy
    {
        public Enemy_StableT2(Vector2 Pos)
            : base(Pos)
        {
            speed = 0;
            hitpoints = 100;
            sprite = Cnt.game.EnemyTextures["Stable"];
            Height = sprite.Height;
            Width = sprite.Width;
            origin = new Vector2(Height / 2, Width / 2);
            spotTime = 0;
            bloodColor = Color.Purple;
            BulletType = typeof(Bullet_EnemyStableT2);
            rof = 3;
            isShooter = true;
            radius = (float)Math.Sqrt(Height * Height + Width * Width);
        }

        public override void Update(GameTime gt)
        {
            if (!control) AI_2(gt);
            base.Update(gt);
        }

    }
}
