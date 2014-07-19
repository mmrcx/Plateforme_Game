using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plateforme_Game.Core
{
    [Flags]
    public enum Direction
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8
    }


    class GameObject
    {
        const int Offset = 5;


        public Vector2 Position;

        public Texture2D Texture;

        public Vector2 Size;

        public Rectangle Bounds;

        public GameObject(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }

        public virtual Direction HasColide(GameObject obj1)
        {
            if (obj1.Position.X + obj1.Size.X - Offset > this.Position.X && obj1.Position.X + Offset < this.Position.X + this.Size.X)
            {
                if (obj1.Position.Y >= this.Position.Y
                    &&
                    obj1.Position.Y <= this.Position.Y + this.Size.Y)
                {
                    return Direction.Up;
                }

                if (obj1.Position.Y + obj1.Size.Y >= this.Position.Y 
                    && 
                    obj1.Position.Y + obj1.Size.Y <= this.Position.Y + this.Size.Y)
                {
                    return Direction.Down;
                }
            }

            if (obj1.Position.Y + obj1.Size.Y - Offset > this.Position.Y && obj1.Position.Y + Offset < this.Position.Y + this.Size.Y)
            {
                if (obj1.Position.X >= this.Position.X
                    &&
                    obj1.Position.X <= this.Position.X + this.Size.X)
                {
                    return Direction.Left;
                }

                if (obj1.Position.X + obj1.Size.X >= this.Position.X
                    &&
                    obj1.Position.X + obj1.Size.X <= this.Position.X + this.Size.X)
                {
                    return Direction.Right;
                }
            }

            return Direction.None;

        }
    }
}
