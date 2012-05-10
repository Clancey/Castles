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
using Microsoft.Xna.Framework.Input.Touch;
using FarseerPhysics.Dynamics;

namespace Castles
{
	public class Projectile
	{
		private const float TouchRadius = 0.5f;
		private Vector2 _touchOffsetFromCenter;
		private int _touchId;
		private bool touched;
		private const float ImpulseModifier = 2.25f;
		private const float MaxDragRadius = 1.5f;

		private static readonly Vector2 CenterOffset = new Vector2(0, 0.025f);

		public Body Body { get; private set; }

		/// <summary>
		/// Shortcut to Body.Position
		/// </summary>
		public Vector2 Position {
			get { return Body.Position; }
		}

		/// <summary>
		/// User is dragging the projectile in order to launch it
		/// Where is the current drag position, taking offset correction into account.
		/// </summary>
		public Vector2 DragPosition { get; private set; }

		/// <summary>
		/// Is user dragging the projectile in order to launch it?
		/// </summary>
		public bool IsBeingDragged { get; private set; }

		public Projectile (Body body)
		{
			Body = body;
			((PrefabUserData) Body.UserData).IsProjectile = true;
		}

		/// <summary>
		/// Handle user input
		/// </summary>
		/// <param name="gameTime">current gameTime</param>
		/// <param name="touches">TouchCollection</param>
		/// <returns>True if projectile handled input, otherwise false</returns>
		public bool HandleInput (TouchCollection touches)
		{
			touched = false;
			foreach (TouchLocation touchLocation in touches)
			{
				Vector2 touchPositionSim = Camera.Current.ScreenToSimulation(touchLocation.Position);
				//Console.WriteLine(touchPositionSim + " - " + Body.Position);
				if (touchLocation.State == TouchLocationState.Pressed)
				{
					
					//Console.WriteLine((Position - touchPositionSim).Length());
					if ((Position - touchPositionSim).Length() < TouchRadius && !IsBeingDragged)
					{
						Console.WriteLine("touched");
						//start dragging the projectile, ready to shoot
						IsBeingDragged = true;
						//user never clicks dead-center on the projectile, this is the offset of the click compared to the center
						_touchOffsetFromCenter = Position - touchPositionSim;
						DragPosition = Position;
						_touchId = touchLocation.Id;
						touched = true;
						return true;
					}
					break;
				}
				else if (touchLocation.State == TouchLocationState.Moved && IsBeingDragged && touchLocation.Id == _touchId)
				{
						Console.WriteLine("moved");
					Vector2 pullPosition = touchPositionSim + _touchOffsetFromCenter;
					Vector2 dragVector = Position - pullPosition;
				
					if (dragVector.Length() < MaxDragRadius)
					{
						DragPosition = pullPosition;
					}
					else
					{
						// if draggingbeyond max radius, limit to max radius.
						dragVector.Normalize();
						DragPosition = Position - Vector2.Multiply(dragVector, MaxDragRadius);
					}
					touched = true;
					return true;
				}
				else if (touchLocation.State == TouchLocationState.Released && IsBeingDragged && touchLocation.Id == _touchId)
				{	
					Launch();
					touched = false;
					return true;
				}
				
			}
			
			return true;
		}
		
		private void Launch()
		{
			Console.WriteLine("launced");
			Camera.Current.StartTracking(this.Body);
			_touchId = 0;
			IsBeingDragged = false;
			Vector2 dragVector = (Body.Position - DragPosition) * ImpulseModifier;
			// apply an impulse to the Body, but a little bit off-center to give it a nice arcing motion
			Vector2 centerWithOffset = Body.WorldCenter + CenterOffset;
			Body.ApplyLinearImpulse(dragVector, centerWithOffset);
		}
	}
}

