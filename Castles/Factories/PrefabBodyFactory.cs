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
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework.Graphics;
using GameStateManagement;

namespace Castles
{
	public enum PrefabType
	{
		GoldBox,
		GrayBox,
		Ball,
		TriangleLeft,
		TriangleRight,
		Pig,
	}
	/// <summary>
/// The PrefabBodyFactory will create new Body instances with specific, predefined settings
/// </summary>
	public class PrefabBodyFactory
	{
		private static readonly Dictionary<PrefabType, Func<World, Vector2, Body>> Library = new Dictionary<PrefabType, Func<World, Vector2, Body>> ();

		/// <summary>
		/// Static constructor will initialize our library of prefab-shapes
		/// </summary>
		static PrefabBodyFactory ()
		{
			
			Library.Add(PrefabType.GoldBox, (world, position) => CreateRectangle(world, position, "box1", 0.4f, 0.4f, 14));
			Library.Add(PrefabType.GrayBox, (world, position) => CreateRectangle(world, position, "box2", 0.4f, 0.4f, 7));
			Library.Add(PrefabType.Pig, (world, position) => CreateRectangle(world, position, "pig", 0.5f, 0.59f, 7));
			Library.Add(PrefabType.Ball, (world, position) => CreateCircle(world, position, "ball", 0.3f));
			Library.Add(PrefabType.TriangleLeft, (world, position) => CreateTriangleL(world, position, "triangleL", 0.4f, 0.4f, 70));
			Library.Add(PrefabType.TriangleRight, (world, position) => CreateTriangleR(world, position, "triangleR", 0.4f, 0.4f, 70));
		}
		/// <summary>
		/// Create Body of certain prefab type
		/// </summary>
		/// <param name="type">prefab type</param>
		/// <param name="world">Farseer world</param>
		/// <param name="position">position of the body in the physics world (using simulation units)</param>
		/// <returns>a new instance of a Body</returns>
		public static Body CreateBody (PrefabType type, World world, Vector2 position)
		{
			//will lookup the correct delegate in our prefab library and execute the delegate in order to create a new instance
			return Library [type] (world, position);
		}

		/// <summary>
		/// Create a body with a rectangular shape
		/// </summary>
		/// <param name="world">World</param>
		/// <param name="position">Position in sim-units</param>
		/// <param name="spriteName">Sprite name to render with</param>
		/// <param name="width">width of the body in sim-units</param>
		/// <param name="height">height of the body in sim-units</param>
		/// <returns>new instance of a Body</returns>
		private static Body CreateRectangle (World world, Vector2 position, string spriteName, float width, float height, int hitPoints)
		{
			Body body = BodyFactory.CreateRectangle (world, width, height, 1f, position);
			body.UserData = new PrefabUserData
			            {
			                Origin = new Vector2(width/2f, height/2f),
			                SpriteName = spriteName,
							HitPoints = hitPoints,
			            };
			body.BodyType = BodyType.Dynamic;
			return body;
		}
		
		/// <summary>
		/// Create a body with a circular shape
		/// </summary>
		/// <param name="world">World</param>
		/// <param name="position">Position in sim-units</param>
		/// <param name="spriteName">Sprite name to render with</param>
		/// <param name="radius">radius of the body in sim-units</param>
		/// <returns>new instance of a Body</returns>
		private static Body CreateCircle(World world, Vector2 position, string spriteName, float radius)
		{
			Body body = BodyFactory.CreateCircle(world, radius, 1f, position);
			body.UserData = new PrefabUserData
			{
				Origin = new Vector2(radius),
				SpriteName = spriteName
			};
			body.AngularDamping = 2f; //without this, our ball would keep on rolling regardless of friction
			body.BodyType = BodyType.Dynamic;
			return body;
		}
		private static Body CreateTriangleL(World world, Vector2 position, string spriteName,float width,float height,int hitPoints)
		{
			
			var w = width /2;
			var h = height / 2;
			Vertices vertices =  new Vertices(3);
		//	vertices.Add(new Vector2(-w,-h));
			vertices.Add(new Vector2(w,-h));
			vertices.Add(new Vector2(w,h));
			vertices.Add(new Vector2(-w,h));
			
            PolygonShape chassis = new PolygonShape(vertices, 1);
			
			
            Body body = new Body(world);
            body.BodyType = BodyType.Dynamic;
            body.CreateFixture(chassis);
			body.Position = position;
			
			body.UserData = new PrefabUserData
			            {
			                Origin = new Vector2(width/2f, height/2f),
			                SpriteName = spriteName,
							HitPoints = hitPoints,
			            };
			return body;
			            
		}
		private static Body CreateTriangleR(World world, Vector2 position, string spriteName,float width,float height,int hitPoints)
		{
			/*
			 
			 13,25
			 25,13
			 -4,13
			 -9,12
			 -12,13
			 -13,-6
			 
			 */
			var w = width /2;
			var h = height / 2;
			Vertices vertices =  new Vertices(6);
			vertices.Add(new Vector2(-w,-h));
			vertices.Add(new Vector2(w,h));
			vertices.Add(new Vector2(-w,h));
			//vertices.Add(new Vector2(-.10f,.00f));
			
            PolygonShape chassis = new PolygonShape(vertices, 1);
			
			
            Body body = new Body(world);
            body.BodyType = BodyType.Dynamic;
            body.CreateFixture(chassis);
			body.Position = position;
			
			body.UserData = new PrefabUserData
			            {
			                Origin = new Vector2(width/2f, height/2f),
			                SpriteName = spriteName,
							HitPoints = hitPoints,
			            };
			return body;
			            
			            
		}
	
	}
}

