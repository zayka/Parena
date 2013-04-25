﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PArena
{
    class FPSCounter:DrawableGameComponent
    {
        public int FPS;
        int frames;
        double seconds;

        SpriteFont font;
        SpriteBatch sb;

        public FPSCounter(Game game, SpriteFont font, SpriteBatch sb):base(game)
        {
            this.font = font;
            this.sb = sb;
        }

        public override void Update(GameTime gameTime)
        {
            seconds += gameTime.ElapsedGameTime.TotalSeconds;
            if (seconds >= 0.5)
            {
                FPS = (int)(frames/seconds);
                seconds = 0;
                frames = 0;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            frames++;
            Vector2 v = new Vector2(Game1.screenWidth - 6 * 10, 0);
            sb.Begin();
            sb.DrawString(font, "FPS:"+FPS,v,Color.White);
            sb.End();
            base.Draw(gameTime);
        }
    }
}
