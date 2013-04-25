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
    class Level_2:Level
    {
        public Level_2()
            : base()
        {
            mainBackground = Cnt.game.Content.Load<Texture2D>("LevelBG\\mainBG");
            levelBackground = Cnt.game.Content.Load<Texture2D>("LevelBG\\bg_l2");
            levelTitle = Cnt.game.Content.Load<Texture2D>("LevelBG\\level2");
            //nextLevel = new Level_3();
            backgroundRT = new RenderTarget2D(Cnt.game.GraphicsDevice, mainBackground.Width, mainBackground.Height, false, Cnt.game.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
            Cnt.game.GraphicsDevice.SetRenderTarget(backgroundRT);
            Cnt.game.GraphicsDevice.Clear(Color.Black);
            Game1.spriteBatch.Begin();
            Game1.spriteBatch.Draw(mainBackground, Vector2.Zero, Color.White);
            Game1.spriteBatch.Draw(levelBackground, Vector2.Zero, Color.White);

            Game1.spriteBatch.End();
            Cnt.game.GraphicsDevice.SetRenderTarget(null);
            nextLevel = new Level_3();
        }

        public override void Update(GameTime gt)
        {

            WaveParam w = new WaveParam();

            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;
            time += elapsed;
            levelTime += elapsed;
            
            
            if (time > 2 && (time - elapsed < 2))
            {
                w.numEnemy = 30;
                waves.Add(new Wave(WaveType.Random, w));
            }
            
            if (time > 10 && (time - elapsed < 10))
            {
                
                w.eType = typeof(Enemy_Normal);
                w.numEnemy = 10;
                w.speed = 180;
                w.duration = 15;
                w.waveDir = new Vector2(1, 0);
                waves.Add(new Wave(WaveType.Line, w));

                w.eType = typeof(Enemy_Normal);
                w.numEnemy = 10;
                w.speed = 180;
                w.duration = 15;
                w.waveDir = new Vector2(-1, 0);
                waves.Add(new Wave(WaveType.Line, w));
            }
            if (time > 27 && (time - elapsed < 27))
            {
                w.eType = typeof(Enemy_Fast);
                w.numEnemy = 15;
                w.speed = 310;
                w.duration = 18;
                w.waveDir = new Vector2(1, 0);
                waves.Add(new Wave(WaveType.Line, w));

                w.eType = typeof(Enemy_Fast);
                w.numEnemy = 15;
                w.speed = 310;
                w.duration = 15;
                w.waveDir = new Vector2(-1, 0);
                waves.Add(new Wave(WaveType.Line, w));
            }
            if (time > 46 && (time - elapsed < 46))
            {
                w.numEnemy = 50;
                waves.Add(new Wave(WaveType.Random, w));
            }

            if (time > 65 && time - elapsed < 65)
            {
                if (waves.Count == 0)
                {
                    w.eType = typeof(Enemy_Fast);
                    w.numEnemy = 11;
                    w.speed = 300;
                    w.duration = 20;
                    w.waveDir = new Vector2(1, 0);
                    waves.Add(new Wave(WaveType.Line, w));

                    w.eType = typeof(Enemy_Fast);
                    w.numEnemy = 12;
                    w.speed = 300;
                    w.duration = 20;
                    w.waveDir = new Vector2(-1, 0);
                    waves.Add(new Wave(WaveType.Line, w));
                }
                else time = 65 - 10 * elapsed;
            }

            if (time > 70 && (time - elapsed < 70))
            {
                w.eType = typeof(Enemy_Fast);
                w.numEnemy = 11;
                w.speed = 300;
                w.duration = 15;
                w.waveDir = new Vector2(0, 1);
                waves.Add(new Wave(WaveType.Line, w));

                w.eType = typeof(Enemy_Fast);
                w.numEnemy = 12;
                w.speed = 300;
                w.duration = 15;
                w.waveDir = new Vector2(0, -1);
                waves.Add(new Wave(WaveType.Line, w));
            }
            
            if (time > 85 && (time - elapsed < 85))
            {
                w.numEnemy = 40;
                waves.Add(new Wave(WaveType.Random, w));
            }
            
            //*/
            if (time > 90 && time - elapsed < 90)
            {
                if (waves.Count == 0)
                {
                    boss = new Boss2(Cnt.game.Content.Load<Texture2D>("boss2"));
                    enemyList.Add(boss);
                    //MediaPlayer.Stop();
                    //MediaPlayer.Play(Cnt.game.bossTheme);
                    Cnt.game.waveOutDevice.Stop();
                    Cnt.game.mainOutputStream = Cnt.game.CreateInputStream(Cnt.game.bossTheme);
                    Cnt.game.waveOutDevice.Init(Cnt.game.mainOutputStream);
                    Cnt.game.waveOutDevice.Play();

                }
                else time = 90 - 10 * elapsed;
            }

            if (boss != null && boss.toDie)
            {
                Player pl = new Player(player.Pos, Vector2.Zero, player.pony);
                Cnt.game.CurrentPlayer = pl;
                //nextLevel = new Level_3();
            }

            base.Update(gt);
        }

    }
}
