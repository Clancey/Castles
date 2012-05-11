#region File Description
//-----------------------------------------------------------------------------
// Human.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO.IsolatedStorage;
using System.IO;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using GameStateManagement;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Collision;
using FarseerPhysics.Controllers;
using FarseerPhysics.DebugViews;

#endregion

namespace Castles
{
	class GameplayScreen : GameScreen
	{
       
		Random random;
		private List<Body> _bodies = new List<Body> ();
		private World _world;
		private const float Scale = 100f;
		GraphicsDevice GraphicsDevice;
		SpriteBatch spriteBatch;
		Projectile _projectile;
		DebugViewXNA DebugView;
		Body slingShot;
		Vector2 startPos;
		public void LoadAssets ()
		{
			

			// Load texture
			LoadTextures();

			// Create new simulation world
			_world = new World (new Vector2 (0, 6f));
			_world.ContactManager.BeginContact = BeginContact;
			
			// Define the ground
			Constants.Initialize (ScreenManager);
			float simulatedHeight = Constants.FloorPosition.Y / Constants.Scale;
			float simulatedWidth = Constants.WorldWidth;
			startPos = new Vector2 (1.5f, simulatedHeight - 1.5f);
			//floor
			BodyFactory.CreateEdge (_world, new Vector2 (0.0f, simulatedHeight), new Vector2 (simulatedWidth* 2, simulatedHeight));
			//ceiling
			BodyFactory.CreateEdge (_world, new Vector2 (0.0f, -simulatedHeight), new Vector2 (simulatedWidth * 2, -simulatedHeight));
			//right wall
			BodyFactory.CreateEdge (_world, new Vector2 (simulatedWidth, simulatedHeight), new Vector2 (simulatedWidth, -simulatedHeight));
			//left wall
			BodyFactory.CreateEdge (_world, new Vector2 (0, -simulatedHeight), new Vector2 (0.0f,simulatedHeight));
			
			//Create 2 boxes
			Body box1 = PrefabBodyFactory.CreateBody (PrefabType.GoldBox, _world, new Vector2 (simulatedWidth - 3.6f, simulatedHeight - .5f));
			box1.Mass = 2;
			//box1.IsStatic = true;
			_bodies.Add (box1);
			
			
			Body box4 = PrefabBodyFactory.CreateBody (PrefabType.GoldBox, _world, new Vector2 (simulatedWidth - 2, simulatedHeight - .5f));
			box4.Mass = 2;
			//box4.IsStatic = true;
			_bodies.Add (box4);
			
			_bodies.Add(PrefabBodyFactory.CreateBody(PrefabType.Wood200x20,_world,new Vector2(simulatedWidth - 2.8f,simulatedHeight - .9f)));
			var woodVert1 =PrefabBodyFactory.CreateBody(PrefabType.Wood200x20,_world,new Vector2(simulatedWidth - 2f,simulatedHeight - 1.9f));
			woodVert1.Rotation = MathHelper.ToRadians(90f);
			//woodVert1.IsStatic = true;
			_bodies.Add(woodVert1);
			
			
			var woodVert2 =PrefabBodyFactory.CreateBody(PrefabType.Wood200x20,_world,new Vector2(simulatedWidth - 3.6f,simulatedHeight - 1.9f));
			woodVert2.Rotation = MathHelper.ToRadians(90f);
			//woodVert2.IsStatic = true;
			_bodies.Add(woodVert2);
			
			
			_bodies.Add(PrefabBodyFactory.CreateBody(PrefabType.Wood200x20,_world,new Vector2(simulatedWidth - 2.8f,woodVert1.Position.Y - 1f)));
			
			/*
			
			Body box2 = PrefabBodyFactory.CreateBody (PrefabType.GrayBox, _world, new Vector2 (simulatedWidth - 2, simulatedHeight-2f));
			_bodies.Add (box2);
			Body box3 = PrefabBodyFactory.CreateBody (PrefabType.GrayBox, _world, new Vector2 (simulatedWidth - 2, simulatedHeight-2.4f));
			_bodies.Add (box3);
			*/
			Body pig = PrefabBodyFactory.CreateBody (PrefabType.Ball, _world, startPos);
			_bodies.Add (pig);
			
			/*
			slingShot = BodyFactory.CreateRectangle(_world,.38f,1.5f,1f);
			slingShot.Position = new Vector2( 3f,simulatedHeight - .75f);
			_bodies.Add(slingShot);
			*/
            pig.FixtureList[0].Restitution = .4f;
			
			_projectile = new Projectile (pig);
			
			Body trangle = PrefabBodyFactory.CreateBody(PrefabType.TriangleLeft,_world,new Vector2(3, simulatedHeight- .2f));
			trangle.Mass = 20f;
			trangle.IsStatic = true;
			_bodies.Add(trangle);
			
			Body trangler = PrefabBodyFactory.CreateBody(PrefabType.TriangleRight,_world,new Vector2(3.4f, simulatedHeight- .2f));
			trangler.Mass = 20f;
			trangler.IsStatic = true;
			_bodies.Add(trangler);
			
			
			Body trangler2 = PrefabBodyFactory.CreateBody(PrefabType.TriangleRight,_world,new Vector2(.2f, simulatedHeight- .2f));
			trangler2.Mass = 20f;
			trangler2.IsStatic = true;
			_bodies.Add(trangler2);
        
			
			Camera.Current.CenterPointTarget = 620f;
			Camera.Current.StartTracking (_projectile.Body);
			Camera.Current.ScreenScale = ScreenManager.ScreenScale;
			
		}
		
		private void LoadTextures()
		{
			TextureLoader.Load(this,PrefabType.Pig);
			TextureLoader.Load(this,PrefabType.Ball);
			TextureLoader.Load(this,PrefabType.GoldBox);
			TextureLoader.Load(this,PrefabType.GrayBox);
			TextureLoader.Load(this,PrefabType.Smoke);
			TextureLoader.Load(this,PrefabType.TriangleLeft);
			TextureLoader.Load(this,PrefabType.TriangleRight);
			TextureLoader.Load(this,PrefabType.Wood100x20);
			TextureLoader.Load(this,PrefabType.Wood175x20);
			TextureLoader.Load(this,PrefabType.Wood200x20);
			TextureLoader.Load(this,PrefabType.Wood50x20);
			TextureLoader.Load(this,PrefabType.Wood25x20);
			TextureLoader.Load(this,PrefabType.Floor);
			/*
			_box1 = Load<Texture2D> ("box1");
			_box2 = Load<Texture2D> ("box2");
			_ball = Load<Texture2D> ("ball");
			_floor = Load<Texture2D> ("floor");
			_pig = Load<Texture2D> ("pig");
			_smoke = Load<Texture2D> ("smoke");
			_triangleL = Load<Texture2D> ("triangleL");
			_triangleR = Load<Texture2D> ("triangleR");
			_slingShotBack = Load<Texture2D> ("Textures/Slingshot/BackLeft.png");
			_slingShotFront = Load<Texture2D> ("Textures/Slingshot/FrontLeft.png");
			*/
		}

		public GameplayScreen ()
		{
			EnabledGestures = 
                GestureType.Hold |
                GestureType.Tap | 
                GestureType.DoubleTap |
                GestureType.FreeDrag |
                GestureType.Flick |
                GestureType.Pinch;

			random = new Random ();
		}

		public override void LoadContent ()
		{ 
			LoadAssets ();

			base.LoadContent ();
			spriteBatch = ScreenManager.SpriteBatch;
			GraphicsDevice = ScreenManager.GraphicsDevice;
			// Start the game
			Start ();
			
			
            DebugView = new DebugViewXNA(_world);
            DebugView.LoadContent(GraphicsDevice,ScreenManager.Game.Content);
		}

		void Start ()
		{
			
		}
		
		public bool BeginContact (Contact contact)
		{
			Body bodyA = contact.FixtureA.Body;
			Body bodyB = contact.FixtureB.Body;

			// get the speed of impact between the two bodies
			Manifold worldManifold;
			contact.GetManifold (out worldManifold);
			ManifoldPoint p = worldManifold.Points [0];
			Vector2 vA = bodyA.GetLinearVelocityFromLocalPoint (p.LocalPoint);
			Vector2 vB = bodyB.GetLinearVelocityFromLocalPoint (p.LocalPoint);
			float approachVelocity = Math.Abs (Vector2.Dot (vB - vA, worldManifold.LocalNormal));

			//deduct hitpoints from both bodies
			ProcessContact (contact, bodyA, approachVelocity);
			ProcessContact (contact, bodyB, approachVelocity);

			return true;
		}

		private void ProcessContact (Contact contact, Body body, float approachVelocity)
		{
			
			PrefabUserData userData = body.UserData as PrefabUserData;

			// only deduct hitpoints if there is userdata (word edges have no userdata)
			// and
			// if the body is not our projectile (projectiles are invincible)
			if (userData != null && !userData.IsProjectile) {
				if(userData.Status != BodyStatus.Active)
					return;
				var hitPoints = (int)Math.Round (approachVelocity);
				userData.HitPoints -= hitPoints;

				if (userData.HitPoints <= 0) {
					// let Farseer know this contact and body are processed and
					// will have no further impact (pun intended)
					contact.Enabled = false;
					body.IsSensor = true;
					// mark this status as ToBeDestroyed
					userData.Destroy(body);

					//userData.Status = BodyStatus.ToBeDestroyed;
				}
			}
		}

		public override void Update (GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
         
			Camera.Current.Update (gameTime);

			_world.Step (Math.Min ((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));
			
           	if(!_projectile.Body.Awake && !_projectile.IsBeingDragged)
				_projectile.Body.Position = startPos;
				
			base.Update (gameTime, otherScreenHasFocus, coveredByOtherScreen);
		}

		public override void HandleInput (InputState input)
		{
           
			_projectile.HandleInput (input.TouchState);
			
			foreach(var gesture in input.Gestures )
            {
                // read the next gesture from the queue

                // we can use the type of gesture to determine our behavior
                switch (gesture.GestureType)
                {


                    // on flicks, we want to update the selected sprite's velocity with
                    // the flick velocity, which is in pixels per second.
                   
                    // on pinches, we want to scale the selected sprite
                    case GestureType.Pinch:
                        if (Camera.Current != null)
                        {
                            // get the current and previous locations of the two fingers
                            Vector2 a = gesture.Position;
                            Vector2 aOld = gesture.Position - gesture.Delta;
                            Vector2 b = gesture.Position2;
                            Vector2 bOld = gesture.Position2 - gesture.Delta2;

                            // figure out the distance between the current and previous locations
                            float d = Vector2.Distance(a, b);
                            float dOld = Vector2.Distance(aOld, bOld);

                            // calculate the difference between the two and use that to alter the scale
                            float scaleChange = (d - dOld) * .01f;
                            Camera.Current.AddScale (scaleChange);
						//Console.WriteLine(Camera.Current.Scale);
                        }
                        break;
                }
            }
		}

		private void FinishCurrentGame ()
		{
			ExitScreen ();
		}

		private void PauseCurrentGame ()
		{
			// TODO: Display pause screen
		}

		public override void Draw (GameTime gameTime)
		{
			
			GraphicsDevice.Clear (Color.CornflowerBlue);
		
			spriteBatch.Begin (0, null, null, null, null, null, Camera.Current.TransformationMatrix);
			/*
			spriteBatch.Draw (_slingShotBack,
					 slingShot.Position * Constants.Scale, null, Color.White, slingShot.Rotation, new Vector2(.19f,1.2f) * Constants.Scale, 1f,
											   SpriteEffects.None, 0f);
			*/
			foreach (var body in _bodies)
			{
				var prefabUserData = body.UserData as PrefabUserData;
				if(prefabUserData == null)
					continue;
				prefabUserData.Draw(gameTime, spriteBatch, body);
			}
			Vector2 floorPosition = Constants.FloorPosition;
			for(int i = 0;i < 10; i++)
			{
				var _floor = TextureLoader.GetTexture(PrefabType.Floor);
				spriteBatch.Draw (
					_floor,
					floorPosition,
					null,
					Color.White
					);
				floorPosition += new Vector2(_floor.Bounds.Width,0);
			}
			//Draw slingshot
			/*
			spriteBatch.Draw (_slingShotFront,
					 slingShot.Position * Constants.Scale, null, Color.White, slingShot.Rotation, new Vector2(.47f,1.3f) * Constants.Scale, 1f,
											   SpriteEffects.None, 0f);
			*/
			
			if (_projectile.IsBeingDragged) {
				PrefabUserData userData = ((PrefabUserData)_projectile.Body.UserData);
				var texture = TextureLoader.GetTexture(userData.PrefabType);
				spriteBatch.Draw (
					texture,
					_projectile.DragPosition * Constants.Scale,
					null,
					new Color (128, 128, 128, 128),
					_projectile.Body.Rotation,
					userData.Origin * Constants.Scale,
					1f,
					SpriteEffects.None,
					0f);
			}
			
			spriteBatch.End ();
			  // calculate the projection and view adjustments for the debug view
            Matrix projection = Matrix.CreateOrthographicOffCenter(0f, GraphicsDevice.Viewport.Width / Scale,
                                                             GraphicsDevice.Viewport.Height / Scale, 0f, 0f,
                                                             1f);
            
			var view = Camera.Current.DebugView;
			//Console.WriteLine(Camera.Current.CameraCenter/ Scale);
            DebugView.RenderDebugData(ref projection, ref view);
			
			base.Draw (gameTime);
			return;
		}

		private void DrawBackground ()
		{
			/*
            // Clear the background
            ScreenManager.Game.GraphicsDevice.Clear(Color.White);

            // Draw the Sky
            ScreenManager.SpriteBatch.Draw(skyTexture, Vector2.Zero, Color.White);

            // Draw Cloud #1
            ScreenManager.SpriteBatch.Draw(cloud1Texture,
                cloud1Position, Color.White);

            // Draw the Mountain
            ScreenManager.SpriteBatch.Draw(mountainTexture,
                Vector2.Zero, Color.White);

            // Draw Cloud #2
            ScreenManager.SpriteBatch.Draw(cloud2Texture,
                cloud2Position, Color.White);

            // Draw the Castle, trees, and foreground 
            ScreenManager.SpriteBatch.Draw(foregroundTexture,
                Vector2.Zero, Color.White);
                */
		}

		void DrawHud ()
		{
			
		}

		void DrawPlayer (GameTime gameTime)
		{
			
		}

		void DrawComputer (GameTime gameTime)
		{
			
		}

		// A simple helper to draw shadowed text.
		void DrawString (SpriteFont font, string text, Vector2 position, Color color)
		{
			ScreenManager.SpriteBatch.DrawString (font, text,
                new Vector2 (position.X + 1, position.Y + 1), Color.Black);
			ScreenManager.SpriteBatch.DrawString (font, text, position, color);
		}

		// A simple helper to draw shadowed text.
		void DrawString (SpriteFont font, string text, Vector2 position, Color color, float fontScale)
		{
			ScreenManager.SpriteBatch.DrawString (font, text,
                new Vector2 (position.X + 1, position.Y + 1),
                Color.Black, 0, new Vector2 (0, font.LineSpacing / 2),
                fontScale, SpriteEffects.None, 0);
			ScreenManager.SpriteBatch.DrawString (font, text, position,
                color, 0, new Vector2 (0, font.LineSpacing / 2),
                fontScale, SpriteEffects.None, 0);
		}

	}
}
