using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PArena
{
    public enum WaveType
    {
        Line,
        Ngon,
        Helix,
        Random,
        Clin
    }
    /*
    public enum WaveDir
    {
        Up,
        Down,
        Left,
        Right
    }*/
    public struct WaveParam
    {
        public float R;
        public int numEnemy;
        public int N;
        public Vector2 center;
        public float duration;
        public int HelixP;
        public float speed;
        public Type eType;
        public Vector2 waveDir;
    }
    public class Wave
    {
        delegate void WaveDelegate(GameTime gt);
        WaveDelegate CurrentWave;
        List<Enemy> enemyList = new List<Enemy>();
        List<Enemy> waveList = new List<Enemy>();
        Player player;
        float duration;
        Vector2 Center;
        WaveParam parameters;
        public int Count { get { return waveList.Count; } }

        /// <summary>
        /// Конструктор волн
        /// </summary>
        /// <param name="type"></param>
        /// <param name="duration">длительность волны</param>
        /// <param name="numEnemy">количество врагов</param>
        /// <param name="p3">Ngon = количество углов, Helix - радиус спирали</param>
        public Wave(WaveType type, WaveParam w)
        {
            parameters = w;
            enemyList = Cnt.game.EnemyList;
            player = Cnt.game.CurrentPlayer;
            this.duration = w.duration;
            switch (type)
            {
                case WaveType.Random:
                    init_Wave_Random(w);
                    CurrentWave = Wave_Random;
                    break;
                case WaveType.Line:
                    init_Wave_Line(w);
                    CurrentWave = Wave_Common;
                    break;
                case WaveType.Ngon:
                    init_Wave_Ngon(w);
                    CurrentWave = Wave_Common;
                    break;
                case WaveType.Helix:
                    init_Wave_MoveHelix(w);
                    CurrentWave = Wave_MoveHelix;
                    break;
                default:
                    break;
            }
        }

        public void Update(GameTime gt)
        {
            CurrentWave(gt);
        }

        //line
        void init_Wave_Line(WaveParam w)
        {

            float step = Game1.screenWidth / w.numEnemy;
            if (w.waveDir.X == 1) step = Game1.screenHeight / w.numEnemy;
            int offsetScreen = 20;
            for (int i = 0; i < w.numEnemy; i++)
            {
                Vector2 place = Vector2.Zero;                
                /*
                switch ()
                {
                    case WaveDir.Up:
                        place = new Vector2(i * step, Game1.screenHeight+20);
                        dirTmp = new Vector2(0, -1);
                        break;
                    case WaveDir.Down:
                        place = new Vector2(i * step, -20);
                        dirTmp = new Vector2(0, 1);                                                
                        break;
                    case WaveDir.Left:
                        place = new Vector2(Game1.screenWidth+20,i * step);
                        dirTmp = new Vector2(-1, 0);
                        break;
                    case WaveDir.Right:
                        place = new Vector2(-20, i * step);
                        dirTmp = new Vector2(1, 0);
                        break;
                    default:
                        break;
                }*/
                if (w.waveDir == new Vector2(0, -1)) place = new Vector2(i * step + offsetScreen, Game1.screenHeight + 20);
                if (w.waveDir == new Vector2(0, 1)) place = new Vector2(i * step + offsetScreen, -20);
                if (w.waveDir == new Vector2(-1, 0)) place = new Vector2(Game1.screenWidth + 20, i * step + offsetScreen);
                if (w.waveDir == new Vector2(1, 0)) place = new Vector2(-20, i * step + offsetScreen);
                    

                
                //Enemy e = new Enemy(place, w.eType);
                Enemy e = (Enemy)Activator.CreateInstance(w.eType, new object[] { place });
                if (w.speed != 0) e.Speed = w.speed;
                e.Dir = w.waveDir;
                e.control = true;
                enemyList.Add(e);
                waveList.Add(e);
            }
        }

        void init_Wave_Ngon(WaveParam w)
        {
            // numEnemy,N
            int N = w.N;
            float centerX = Game1.screenWidth / 2;
            float centerY = Game1.screenHeight / 2;
            float R = centerY+160;
            int sideEnemy = w.numEnemy/N;

            for (int i = 0; i < w.N; i++)
			{
                Vector2 vert1 = new Vector2(centerX + R * (float)Math.Cos(i * MathHelper.TwoPi / N), centerY + R * (float)Math.Sin(i * MathHelper.TwoPi / N));
                Vector2 vert2 = new Vector2(centerX + R * (float)Math.Cos((i+1) * MathHelper.TwoPi / N), centerY + R * (float)Math.Sin((i+1) * MathHelper.TwoPi / N));
                Vector2 side = vert2 - vert1;
                float step = side.Length()/sideEnemy;
                side.Normalize();
                for (int j = 0; j < sideEnemy; j++)
                {

                    Enemy e = (Enemy)Activator.CreateInstance(w.eType, new object[] { vert1 });
                    e.Dir = new Vector2(centerX,centerY)-e.Pos;                    
                    e.Dir.Normalize();
                    //e.Dir = Vector2.Zero;
                    e.control = true;
                    enemyList.Add(e);
                    waveList.Add(e);
                    vert1 += side * step;
                }
			}
            
        }

        void init_Wave_MoveHelix(WaveParam w)
        {
            // numEnemy, N, center
            int N = w.N;
            //float X = 0;
            Vector2 XY = Vector2.Zero;
            //Vector2 XY = w.waveDir;
            Center = w.center;
            for (int i = 0; i < w.numEnemy; i++)
            {
                Vector2 vert1 = Vector2.Zero;                
                vert1 = new Vector2(XY.X + Center.X + w.R * (float)Math.Cos(i * MathHelper.TwoPi / N), XY.Y+Center.Y + w.R * (float)Math.Sin(i * MathHelper.TwoPi / N));
                Enemy e = (Enemy)Activator.CreateInstance(w.eType, new object[] { vert1 });
                //e.Dir = new Vector2(centerX, centerY) - e.Pos;
                //e.Dir.Normalize();
                e.Dir = Vector2.Zero;
                e.control = true;
                enemyList.Add(e);
                waveList.Add(e);
                //X -= w.HelixP;
                XY-=w.waveDir*w.HelixP;
                //if (XY.X != 0) XY.X -= w.HelixP;
               // if (XY.Y != 0) XY.Y -= w.HelixP;
                
            }
        }

        void Wave_MoveHelix(GameTime gt)
        {
            Matrix m = Matrix.CreateFromAxisAngle(Vector3.UnitZ, -MathHelper.Pi / 36);
            //float X = 0;
            Vector2 XY = Vector2.Zero;
            foreach (Enemy e in waveList)
            {               
                //Vector2 rv = e.Pos - new Vector2(Center.X + X, Center.Y);                
                //rv = Vector2.Transform(rv, m);                
                //e.Pos = new Vector2(Center.X + X, Center.Y) + rv;               
                //X -= parameters.HelixP;               
                //e.Pos.X += (float)gt.ElapsedGameTime.TotalSeconds * parameters.speed;
                
                Vector2 rv = e.Pos - (Center + XY);
                rv = Vector2.Transform(rv, m);
                e.Pos = Center + XY + rv;
                XY -= parameters.waveDir * parameters.HelixP;
                e.Pos +=parameters.waveDir*(float)gt.ElapsedGameTime.TotalSeconds * parameters.speed;
            }
            //Center.X += (float)gt.ElapsedGameTime.TotalSeconds * parameters.speed;
            Center += parameters.waveDir*(float)gt.ElapsedGameTime.TotalSeconds * parameters.speed;


            duration -= (float)gt.ElapsedGameTime.TotalSeconds;
            for (int i = 0; i < waveList.Count; i++)
            {
                if (waveList[i].isShooter) waveList[i].AI_2(gt);
                if (waveList[i].toDie) waveList.RemoveAt(i);

            }
            if (duration <= 0)
            {
                CurrentWave = Wave_Random;                
            }
        }

        void Wave_Common(GameTime gt)
        {
            
            duration -= (float)gt.ElapsedGameTime.TotalSeconds;
            for (int i = 0; i < waveList.Count; i++)
            {
                if (waveList[i].isShooter) waveList[i].AI_2(gt);
                if (waveList[i].toDie) waveList.RemoveAt(i);
                
            }   

            if (duration <= 0)
            {
                CurrentWave = Wave_Common;
                foreach (Enemy e in waveList)
                {
                    e.control = false;
                }
            }


        }


        void init_Wave_Random(WaveParam w)
        {
            for (int i = 0; i < 2*w.numEnemy/3; i++)
            {
                Vector2 screen = new Vector2(Game1.screenWidth, Game1.screenHeight);
                Vector2 Pos = new Vector2(1, 1);
                while (Pos == Vector2.Clamp(Pos, Vector2.Zero, screen))
                {
                    Pos.X = (float)Game1.rnd.NextDouble() * (Game1.screenWidth + 80) - 40;
                    Pos.Y = (float)Game1.rnd.NextDouble() * (Game1.screenHeight + 80) - 40;
                }
                Enemy e = new Enemy_Fast(Pos);
                e.control = true;
                waveList.Add(e);
                enemyList.Add(e);
            }

            for (int i = 0; i <  w.numEnemy / 3; i++)
            {
                Vector2 screen = new Vector2(Game1.screenWidth, Game1.screenHeight);
                Vector2 Pos = new Vector2(1, 1);
                while (Pos == Vector2.Clamp(Pos, Vector2.Zero, screen))
                {
                    Pos.X = (float)Game1.rnd.NextDouble() * (Game1.screenWidth + 80) - 40;
                    Pos.Y = (float)Game1.rnd.NextDouble() * (Game1.screenHeight + 80) - 40;
                }
                Enemy e = new Enemy_Normal(Pos);
                e.control = true;
                waveList.Add(e);
                enemyList.Add(e);
            }
        }

        void Wave_Random(GameTime gt)
        {
            for (int i = 0; i < waveList.Count; i++)
            {
                if (Game1.rnd.Next(100) > 95)
                {
                    waveList[i].Dir = new Vector2((float)Game1.rnd.NextDouble() * 2 - 1, (float)Game1.rnd.NextDouble() * 2 - 1);
                    waveList[i].Dir += (player.Pos - waveList[i].Pos) * 0.01f;
                    waveList[i].Dir.Normalize();
                }
                if (waveList[i].toDie) waveList.RemoveAt(i);
            }            
        }
    }
}
