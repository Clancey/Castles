// 
//  Copyright 2012  James Clancey, Xamarin Inc  (http://www.xamarin.com)
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using System.Collections.Generic;
using FarseerPhysics.Factories;

namespace Castles
{
	public class Game2 : Game
	{
		private Texture2D _box;
		private List<Body> _bodies = new List<Body> ();
		private World _world;
		private const float Scale = 100f;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
		Body box2;
		Body box1;
		
#if ZUNE
        int BufferWidth = 272;
        int BufferHeight = 480;
#elif IPHONE
        int BufferWidth = 320;
        int BufferHeight = 480;
#else
        int BufferWidth = 272;
        int BufferHeight = 480;
#endif
		public Game2 ()
		{
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
			
			graphics.PreferredBackBufferWidth = BufferHeight;
            graphics.PreferredBackBufferHeight = BufferWidth;
			graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

           // // Extend battery life under lock.
            //InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
		
			// Load texture
			_box = Content.Load<Texture2D>("box");
		
			// Create new simulation world
			_world = new World(new Vector2(0, 6f));
		
			// Define the ground
			float simulatedHeight = BufferWidth / Scale;
			float simulatedWidth = BufferHeight / Scale;
			BodyFactory.CreateEdge(_world, new Vector2(0.0f, simulatedHeight), new Vector2(simulatedWidth, simulatedHeight));
		
			// Create 2 boxes
			box1 = BodyFactory.CreateRectangle(_world, 0.4f, 0.4f, 1f);
			box1.Position = new Vector2(4, 0.75f);
			box1.BodyType = BodyType.Dynamic;
			_bodies.Add(box1);
		
			box2 = BodyFactory.CreateRectangle(_world, 0.4f, 0.4f, 1f);
			box2.Position = new Vector2(4.2f, 0.0f);
			box2.BodyType = BodyType.Dynamic;
			_bodies.Add(box2);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
              //  this.Exit();
			// Run physics simulation
			_world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
			Vector2 origin = new Vector2(_box.Width / 2f, _box.Height / 2f);
		
			spriteBatch.Begin();
			foreach (var body in _bodies)
			{
				spriteBatch.Draw(_box, body.Position * Scale , null, Color.White, body.Rotation, origin, 1f,
									   SpriteEffects.None, 0f);
			}
			spriteBatch.End();
		
			base.Draw(gameTime);
        }
    }
}
