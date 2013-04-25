using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PArena
{
    public enum BonusType
    { 
        Health,
        Power,
        FastShot,
        Speed,
        Test
    }
    public class Bonus:GameObject
    {
        delegate void BonusDelegate(Player pl);
        BonusDelegate getBonus;
        float amount=0;
        public bool toRemove;
        public bool prepareToremove;
        float showTextTime = 2.0f;
        Rectangle rectTex;
        float angle=0;
        float originPosY;
        string getText = string.Empty;
        public Rectangle rectB { get { return new Rectangle((int)Math.Round(Pos.X - origin.X), (int)Math.Round(Pos.Y - origin.Y), Width, Height); } }

        public Bonus(Vector2 pos, BonusType bType)
            : base(pos, Vector2.Zero, null, 0)
        {

            switch (bType)
            {
                case BonusType.Health:
                    amount = 25;
                    getBonus = getHP;
                    getText = "Moar HP!";
                    break;
                case BonusType.Power:
                    amount = 0.12f;
                    getBonus = getPower;
                    getText = "POWEEER!";
                    break;
                case BonusType.FastShot:
                    amount = 0.9f;
                    getBonus = getRof;
                    getText = "Fast shots!";
                    break;
                case BonusType.Speed:
                    amount = 30.0f;
                    getBonus = getSpeed;
                    getText = "Speeeed!";
                    break;
                default:
                    break;
            }
            
            sprite = Cnt.game.bonusesTex;
            Height = 40;
            Width = 40;
            rectTex = new Rectangle((int)bType * Width, 0, Width, Height);
            origin = new Vector2(Width / 2, Height / 2);
            originPosY = pos.Y;
        }


        public static void LoadRandomBonus(Game1 game)
        { 
            int rand = Game1.rnd.Next(4);
            Vector2 v = new Vector2(Game1.rnd.Next(Game1.screenWidth - 80) + 40, Game1.rnd.Next(Game1.screenHeight - 80) + 40);
            Bonus b = new Bonus(v,(BonusType)rand);
            game.BonusList.Add(b);
        }


        public override void Draw(SpriteBatch sb)
        {
            if (!prepareToremove) sb.Draw(sprite, rectB,rectTex, Color.White);                      
        }

        public override void Draw(SpriteBatch sb, Vector2 offset)
        {

            if (!prepareToremove)
            {
                Rectangle r = rectB;
                r.Offset((int)offset.X, (int)offset.Y);
                sb.Draw(sprite, r, rectTex, Color.White);
            }                       
        }

        public void DrawText(SpriteBatch sb)
        {
            if (prepareToremove) sb.DrawString(Cnt.game.fontVerdana, getText, Pos - origin, Color.White);
        }


        public override void Update(GameTime gt)
        {
            if (prepareToremove) showTextTime -= (float)gt.ElapsedGameTime.TotalSeconds;

            angle += MathHelper.Pi*(float)gt.ElapsedGameTime.TotalSeconds;
            Pos.Y = originPosY-20*(float)Math.Sin(angle);

            if (showTextTime < 0 && prepareToremove) toRemove = true;
        }

        public void Interact(Player pl)
        {
            if (prepareToremove) return;
            getBonus(pl);
            prepareToremove = true;
        }

        void getHP(Player pl)
        {
            pl.hitpoints += amount;
            pl.hitpoints=MathHelper.Clamp(pl.hitpoints, 0, pl.maxHitpoints);
        }

        void getPower(Player pl)
        {
            pl.damageMul += amount;
        }
        void getRof(Player pl)
        {
            pl.rof *= amount;
            pl.rof = MathHelper.Clamp(pl.rof, 0.05f, 1000);
        }
        void getSpeed(Player pl)
        {
            pl.Speed += amount;
        }
    }
}
