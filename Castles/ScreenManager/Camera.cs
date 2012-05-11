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
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace Castles
{
	public class Camera
	{
		private float _offsetX,_offsetY;
		private Body _trackingBody;

		public Matrix TransformationMatrix { get; private set; }

		public float CenterPointTarget { get; set; }
		public float ScreenScale {get;set;}
		float _scale =1f;
		public float Scale {
			get{return _scale * ScreenScale;}
		}
		private Vector2 centerPoint;
		public Vector2 CenterPoint {
			get{if(_trackingBody != null)
				return _trackingBody.Position;
				return centerPoint;}
			set{
				centerPoint = value;
				StopTracking();
			}
		}
		public void AddScale(float scale)
		{
			_scale = MathHelper.Clamp(_scale + scale,.25f,1.5f);
		}

		public static readonly Camera Current = new Camera ();

		private Camera ()
		{
			ScreenScale = 1f;
		}
		
		public void Update (GameTime gameTime)
		{
			if (_trackingBody != null) {
				// if tracking body is not located in the center of the view (half screen width + current offset)
				//if (CenterPoint.X * Scale != (Constants.HalfScreenWidth /_scale) + _offsetX) {
					// move camera's offset so tracking body is dead center again
					_offsetX = Clamp (CenterPoint.X * Constants.Scale - Constants.HalfScreenWidth);
				//}
				_offsetY =  (Constants.FloorPosition.Y + Constants.FloorSize.Y) - (Constants.ScreenHeight / _scale) ;
			}
			
			TransformationMatrix = Matrix.CreateTranslation (-_offsetX, -_offsetY, 0f) * Matrix.CreateScale(Scale);
			//Matrix.CreateScale(.5f);
		}
		public Matrix DebugView
		{
			get{return Matrix.CreateTranslation (-_offsetX/Constants.Scale, -_offsetY/Constants.Scale, 0f)* Matrix.CreateScale(Scale);}
		}
		public Vector2 CameraCenter
		{
			get {return new Vector2(0,-0);}
		}

		private float Clamp (float value)
		{
			// Camera's offset is not allowed to move beyond the centerpoint of our target
			return MathHelper.Clamp (value, 0, (Constants.WorldWidth * Constants.Scale) - Constants.ScreenWidth);
		}

		public void StartTracking (Body body)
		{
			_trackingBody = body;
		}

		public void StopTracking ()
		{
			_trackingBody = null;
		}

		/// <summary>
		/// Converts a location on the screen to a location in our world, taking care of the current camera settings and physics scale.
		/// </summary>
		/// <param name="touchPosition"></param>
		/// <returns></returns>
		public Vector2 ScreenToSimulation (Vector2 touchPosition)
		{
			return Vector2.Transform (touchPosition, Matrix.Invert (TransformationMatrix)) / Constants.Scale;
		}
	}
}

