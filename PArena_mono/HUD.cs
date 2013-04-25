using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PArena
{
    class HUD
    {
        Game1 game;
        Rectangle playerHPRect;
        Rectangle bossHPRect;
        Rectangle playerHPBackRect;
        Rectangle bossHPBackRect;
        Texture2D HUDMain;
        Texture2D HUDBack;

        public HUD()
        {
            game = Cnt.game;
            HUDMain = game.Content.Load<Texture2D>("hud");
            HUDBack = game.Content.Load<Texture2D>("hudBack");   
            playerHPRect=new Rectangle(28,20,240,25);
            bossHPRect=new Rectangle(383,20,621,25);
            playerHPBackRect=new Rectangle(22,54,252,37);
            bossHPBackRect = new Rectangle(377, 54, 634, 37);
        }


        public void Update()
        {

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(HUDBack, Vector2.Zero, Color.White);
            
            float fill1 = game.CurrentPlayer.hitpoints / game.CurrentPlayer.maxHitpoints;
            
            sb.Draw(HUDMain, new Vector2(28, 20), playerHPRect, Color.White, 0, Vector2.Zero, new Vector2(fill1, 1), SpriteEffects.None, 0);
            sb.Draw(HUDMain, new Vector2(22, 14), playerHPBackRect, Color.White, 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0);

            if (game.CurrentLevel.LevelBoss != null)
            {
                float fill2 = game.CurrentLevel.LevelBoss.hitpoints / game.CurrentLevel.LevelBoss.maxHitpoints;

                sb.Draw(HUDMain, new Vector2(381 + (1 - fill2) * 621, 20), bossHPRect, Color.White, 0, Vector2.Zero, new Vector2(fill2, 1), SpriteEffects.FlipHorizontally, 0);
                sb.Draw(HUDMain, new Vector2(375, 14), bossHPBackRect, Color.White, 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0);

            }
        }
    }
}
