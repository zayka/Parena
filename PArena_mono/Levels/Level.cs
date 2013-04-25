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
    public class Level
    {
        public float Time { get { return time; } }
        public float levelTime = 0;
        protected float time = 0;
        protected List<Enemy> enemyList = new List<Enemy>();
        protected Player player;
        protected List<Wave> waves = new List<Wave>();
        public Texture2D mainBackground;
        public Texture2D levelBackground;
        protected Texture2D levelTitle;
        protected float startTime = 2;
        //public bool needDraw { get { return time < startTime || boss!=null; } }
        public Boss LevelBoss { get { return boss; } }
        protected Boss boss;
        Texture2D bossTitle;
        Texture2D levelComplete;
        public Level nextLevel;
        public RenderTarget2D backgroundRT;

        public Level()
        {           
            time = 0;
            enemyList = Cnt.game.EnemyList;
            player = Cnt.game.CurrentPlayer;
            bossTitle = Cnt.game.Content.Load<Texture2D>("LevelBG\\BossTitle");
            levelComplete = Cnt.game.Content.Load<Texture2D>("levelComplete");
            Cnt.game.BonusList.Clear();
        }

        public virtual void Update(GameTime gt) 
        {
            for (int i = 0; i < waves.Count; i++)
            {
                waves[i].Update(gt);
                if (waves[i].Count == 0) waves.RemoveAt(i);
            }
            if (boss != null && boss.toDie)
            {               
                Cnt.game.gameState = GameStates.GS_ChangeLevel;
                MediaPlayer.Stop();
              
                //Cue victory = Cnt.game.soundBank.GetCue("victory");
                //victory.Play();
                Cnt.game.waveOutDevice.Stop();
                Cnt.game.soundPlayer.SoundLocation = Cnt.game.victorySound;
                Cnt.game.soundPlayer.Play();
            }

        }

        public virtual void UpdateTime(GameTime gt)
        {
            float elapsed = (float)gt.ElapsedGameTime.TotalSeconds;
            time += elapsed;
            levelTime += elapsed;
        }

        public virtual void  Draw(SpriteBatch sp, Rectangle rect)
        {

            Color c = Color.Lerp(Color.Black, Color.White, time / startTime);
            sp.Draw((Texture2D)backgroundRT, rect, c);           
            // про босса
            if (boss != null && startTime > boss.bossTime)
            {
                c = Color.Lerp(new Color(0, 0, 0, 0), Color.White, boss.bossTime / startTime);
                //sp.DrawString(Cnt.game.font, "BOSS 1", new Vector2(500, 500), Color.White);
                sp.Draw(bossTitle, new Vector2(250, 500), c);
            }
            else if (boss != null && 2 * startTime > boss.bossTime)
            {
                Color c2 = Color.Lerp(Color.White, new Color(0, 0, 0, 0), (boss.bossTime - startTime) / startTime);
                sp.Draw(bossTitle, new Vector2(250, 500), c2);
            }
            
            // про старт уровня
            if (startTime > Time)
            {
                c = Color.Lerp(new Color(0, 0, 0, 0), Color.White, time / startTime);
                sp.Draw(levelTitle, new Vector2(250, 100), c);
            }
            else if (2 * startTime > Time)
            {
                Color c2 = Color.Lerp(Color.White, new Color(0, 0, 0, 0), (time - startTime) / startTime);
                sp.Draw(levelTitle, new Vector2(250, 100), c2);
            }

            // про переход уровня
            if (boss!=null&&boss.toDie)
            {
                sp.Draw(levelComplete, new Vector2(250, 500), c);
               // Cnt.game.gameState = GameStates.GS_ChangeLevel;
            }

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