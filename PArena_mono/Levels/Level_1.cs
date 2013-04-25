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
    public class Level_1:Level
    {
        public Level_1():base()
        {
            mainBackground = Cnt.game.Content.Load<Texture2D>("LevelBG\\mainBG");
            levelBackground = Cnt.game.Content.Load<Texture2D>("LevelBG\\bg_l1");
            levelTitle = Cnt.game.Content.Load<Texture2D>("LevelBG\\level1");
            //nextLevel = new Level_2();        

            backgroundRT = new RenderTarget2D(Cnt.game.GraphicsDevice, mainBackground.Width, mainBackground.Height, false, Cnt.game.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
            Cnt.game.GraphicsDevice.SetRenderTarget(backgroundRT);
            Cnt.game.GraphicsDevice.Clear(Color.Black);
            Game1.spriteBatch.Begin();
            Game1.spriteBatch.Draw(mainBackground, Vector2.Zero, Color.White);
            Game1.spriteBatch.Draw(levelBackground, Vector2.Zero, Color.White);

            Game1.spriteBatch.End();
            Cnt.game.GraphicsDevice.SetRenderTarget(null);
            //mainBackground.Dispose();
            //mainBackground = (Texture2D)mainrender;
        }

        public override void Update(GameTime gt)
        {       
    
            WaveParam w=new WaveParam();
            
            float elapsed =(float)gt.ElapsedGameTime.TotalSeconds;
            time += elapsed;
            levelTime += elapsed;

            /*
            if (time > 2 && (time - elapsed < 2))
            {
                Enemy e = new Enemy_StableT1(new Vector2(200, 200));
                enemyList.Add(e);
            }
            */
            
            
            if (time > 2 && (time - elapsed < 2)) 
            {
                w.numEnemy = 25;
                waves.Add(new Wave(WaveType.Random, w));
            }
            if (time > 10 && (time - elapsed < 10))
            {
                w.eType = typeof(Enemy_Fast);
                w.numEnemy = 20;
                w.speed = 325;
                w.duration = 10;                
                w.waveDir = new Vector2(0, 1);
                waves.Add(new Wave(WaveType.Line, w));
            }
            if (time > 25 && (time - elapsed < 25))
            {
                w.numEnemy = 25;
                w.speed = 175;
                w.duration = 10;
                w.eType = typeof(Enemy_Normal);
                w.waveDir = new Vector2(0, 1);
                waves.Add(new Wave(WaveType.Line, w));
            }
            if (time > 35 && (time - elapsed < 35))
            {
                w.numEnemy = 50;
                waves.Add(new Wave(WaveType.Random, w));
            }

            if (time > 45&& time - elapsed < 45 )
            {
                if (waves.Count == 0)
                {
                    w.numEnemy = 30;
                    w.speed = 325;
                    w.duration = 10;
                    w.eType = typeof(Enemy_Fast);
                    w.waveDir = new Vector2(1, 0);
                    waves.Add(new Wave(WaveType.Line, w));
                    time = 45 +  elapsed;
                }
                else time = 45 - 10*elapsed;
            }
            if (time > 50 && (time - elapsed < 50))
            {
                w.numEnemy = 25;
                w.speed = 175;
                w.duration = 10;
                w.eType = typeof(Enemy_Normal);
                w.waveDir = new Vector2(1, 0);
                waves.Add(new Wave(WaveType.Line, w));
            }
            
            if (time > 60 && time - elapsed < 60)
            {
                if (waves.Count == 0)
                {
                    boss = new Boss1(Cnt.game.Content.Load<Texture2D>("boss1"));
                    enemyList.Add(boss);
                    //MediaPlayer.Stop();
                    //MediaPlayer.Play(Cnt.game.bossTheme);
                    Cnt.game.waveOutDevice.Stop();
                    Cnt.game.mainOutputStream = Cnt.game.CreateInputStream(Cnt.game.bossTheme);
                    Cnt.game.waveOutDevice.Init(Cnt.game.mainOutputStream);
                    Cnt.game.waveOutDevice.Play();

                }
                else time = 60 - 10 * elapsed;
            }
            
            if (boss != null && boss.toDie)
            {
                Player pl = new Player(player.Pos, Vector2.Zero, player.pony);
                Cnt.game.CurrentPlayer = pl;
                nextLevel = new Level_2();
            }
            
            base.Update(gt);
        }        

        public override void Draw(SpriteBatch sp, Rectangle rect)
        {          
            base.Draw(sp, rect);
        }
    }   
}



/*
 w.numEnemy = 30;
                w.duration = 11;
                w.waveDir = new Vector2(0, -1);
                w.eType = EnemyType.Fast;
                w.HelixP = 5;
                w.N = 16;
                w.R = 100;
                w.speed = 100;
                w.center = new Vector2(500, Game1.screenHeight / 2);
                waves.Add(new Wave(WaveType.Helix, w)); 
*/