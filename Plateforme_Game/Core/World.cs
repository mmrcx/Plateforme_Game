using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plateforme_Game.Core
{
    class World
    {
        private readonly Game _game;
        List<GameObject> blocs = new List<GameObject>();

        IList<GameObject> Blocs { get { return blocs; } }
        public GameObject Door;

        public World(Game game)
        {
            _game = game;
        }


        public void Initialize(string levelName)
        {
            Texture2D level = _game.Content.Load<Texture2D>(levelName);
            Texture2D doorTexture = _game.Content.Load<Texture2D>("Door");
            Texture2D grassTexture = _game.Content.Load<Texture2D>("grass_side");

            Color[] data = new Color[level.Width * level.Height];

            level.GetData(data);

            for (int y = 0; y < level.Height; y++)
                for (int x = 0; x < level.Width; x++)
                {
                    int index = x + y * level.Width;

                    Color tile_data = data[index];

                    int posX = x - 1; //-- 1 hidden bloc left
                    int posY = y - 1; //-- 1 hidden bloc top

                    if (tile_data == Color.Red)
                    {
                        var bloc = new GameObject(new Vector2(posX * 32, posY * 32), new Vector2(32, 32));
                        bloc.Texture = grassTexture;

                        blocs.Add(bloc);
                    }
                    else if (tile_data == Color.Blue)
                    {
                        var bloc = new GameObject(new Vector2(posX * 32, posY * 32), new Vector2(32, 32));
                        bloc.Texture = doorTexture;

                        Door = bloc;
                    }

                }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var bloc in blocs)
            {
                bloc.Draw(spriteBatch);
            }

            Door.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            //-- Mouvable blocks
        }

        public bool HasRoomForRectangle(Rectangle rectangleToCheck)
        {
            foreach (var bloc in blocs)
            {
                if (bloc.Bounds.Intersects(rectangleToCheck))
                {
                    return false;
                }
            }
            return true;
        }

        internal Vector2 CheckAndFixPlayerPosition(Vector2 oldPosition, Vector2 destination, Vector2 size)
        {
            Vector2 movement = destination - oldPosition;
            Vector2 furthestAvailableLocationSoFar = oldPosition;

            int numberOfStepsToBreakMovementInto = (int)(movement.Length() * 2) + 1;
            Vector2 oneStep = movement / numberOfStepsToBreakMovementInto;

            for (int i = 1; i <= numberOfStepsToBreakMovementInto; i++)
            {
                Vector2 positionToTry = oldPosition + oneStep * i;
                Rectangle newBoundary = CreateRectangleAtPosition(positionToTry, size);
                if (HasRoomForRectangle(newBoundary)) { furthestAvailableLocationSoFar = positionToTry; }
                else
                {
                    bool isDiagonalMove = movement.X != 0 && movement.Y != 0;
                    if (isDiagonalMove)
                    {
                        int stepsLeft = numberOfStepsToBreakMovementInto - (i - 1);

                        Vector2 remainingHorizontalMovement = oneStep.X * Vector2.UnitX * stepsLeft;
                        Vector2 finalPositionIfMovingHorizontally = furthestAvailableLocationSoFar + remainingHorizontalMovement;
                        furthestAvailableLocationSoFar =
                            CheckAndFixPlayerPosition(furthestAvailableLocationSoFar, finalPositionIfMovingHorizontally, size);

                        Vector2 remainingVerticalMovement = oneStep.Y * Vector2.UnitY * stepsLeft;
                        Vector2 finalPositionIfMovingVertically = furthestAvailableLocationSoFar + remainingVerticalMovement;
                        furthestAvailableLocationSoFar =
                            CheckAndFixPlayerPosition(furthestAvailableLocationSoFar, finalPositionIfMovingVertically, size);
                    }
                    break;
                }
            }


            return furthestAvailableLocationSoFar;
        }

        private Rectangle CreateRectangleAtPosition(Vector2 positionToTry, Vector2 size)
        {
            return new Rectangle((int)positionToTry.X, (int)positionToTry.Y, (int)size.X, (int)size.Y);
        }

        internal bool IsOnGround(Player player)
        {
            Rectangle onePixelLower = new Rectangle((int)player.Position.X, (int)player.Position.Y + 1, (int)player.Size.X, (int)player.Size.Y); ;
            
            return !HasRoomForRectangle(onePixelLower);
        }
    }
}
