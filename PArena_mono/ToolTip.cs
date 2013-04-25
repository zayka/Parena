using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace PArena
{
    class ToolTip
    {
        Rectangle activeRect;
        Rectangle mainRect;
        Rectangle texureRect;
        public bool Visible { get { return timeToFade!=0; } }
        public bool Active { get { return active; } }
        bool active;
        string text;
        Color color;
        float fadetime=0.5f;
        float timeToFade=0;
        public Pony playerType;
       


        public ToolTip(Rectangle activeRect, Rectangle mainRect, string text, Color color, Pony p)
        {
            this.activeRect = activeRect;
            this.mainRect = mainRect;
            texureRect = new Rectangle(0, 122, 270, 90);
            this.text = text;
            this.color = color;
            playerType = p;

        }

        public void Update(MouseState ms, GameTime gt)
        {
            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;
            Vector2 mouse = new Vector2(ms.X,ms.Y);
            Vector2 v1 = new Vector2(activeRect.Left, activeRect.Top);
            Vector2 v2 = new Vector2(activeRect.Right, activeRect.Bottom);
            if (mouse == Vector2.Clamp(mouse, v1, v2))
            {
                active = true;
                timeToFade = fadetime;
            }
            else
            {
                if (timeToFade > 0) {  timeToFade -= elapsed; }

                active = false;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            Color c = Color.Lerp(Color.Transparent, Color.White, timeToFade / fadetime);
            Color c2 = Color.Lerp(Color.Transparent, color, timeToFade / fadetime);

            Vector2 v1 = new Vector2(mainRect.Left+10, mainRect.Top+10);
            sb.Draw(Cnt.game.tooltipTex,mainRect,texureRect, c);
            sb.DrawString(Cnt.game.fontVerdana, text, v1, c2);
        }
    }
}
