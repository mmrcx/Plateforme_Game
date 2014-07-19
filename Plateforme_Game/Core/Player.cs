using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plateforme_Game.Core
{
    class Player: GameObject
    {
        readonly Rectangle StandRight;
        readonly Rectangle StandLeft;

        private int frameNumber;
        private Rectangle CurrentFrame;
        private int lastFrameUpdatedTime;

        private Vector2 _direction = Vector2.Zero; //-- y: for gravity

        int Speed = 18;

        public Player()
            : base(new Vector2(64, 64), new Vector2(18, 23))
        {
            StandRight = new Rectangle(
                0,
                0,
                18,
                23);
            StandLeft = new Rectangle(
                0,
                24,
                18,
                23);
        }

        public void Update(GameTime gameTime)
        {
            AffectWithGravity();
            SimulateFriction();

            if (!Game1.IsPaused)
            {
                Position += _direction * (float)gameTime.ElapsedGameTime.TotalMilliseconds / (1000 / Speed);
            }

            

            if (Math.Abs(_direction.X) < 1) //-- bouge pas
            {
                if (_direction.X < 0)
                    CurrentFrame = StandLeft;
                else
                    CurrentFrame = StandRight;
            }
            else
            {
                lastFrameUpdatedTime += gameTime.ElapsedGameTime.Milliseconds;
                if (this.lastFrameUpdatedTime > 64)
                {
                    lastFrameUpdatedTime -= 64;
                    frameNumber ++;
                    if(frameNumber % 2 == 1){
                        CurrentFrame = (_direction.X < 0) ? new Rectangle(18, 24, 18, 23) : new Rectangle(18, 0, 18, 23);

                        frameNumber = 1;
                    }
                    else
                        CurrentFrame = (_direction.X < 0) ? new Rectangle(36, 24, 18, 23) : new Rectangle(36, 0, 18, 23);
                }
            }
            
        }

        private void AffectWithGravity()
        {
            _direction += Vector2.UnitY * .9f;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, CurrentFrame, Color.White);
        }

        internal void MoveRight()
        {
            _direction += Vector2.UnitX;
        }

        internal void MoveLeft()
        {
            _direction -= Vector2.UnitX;
        }

        private void SimulateFriction()
        {
            _direction -= _direction * Vector2.One * .08f;
        }

        internal void Jump()
        {
            _direction = -Vector2.UnitY * 35;
        }

    }
}
