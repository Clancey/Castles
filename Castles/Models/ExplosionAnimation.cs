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
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;

namespace Castles
{
	/// <summary>
	/// Component to display an explosion
	/// </summary>
	public class ExplosionAnimation
	{
		private bool _isActive;
		private Vector2 _position;
		private float _rotation;
		private float _transitionValue; // goes from 0 (start explosion) to 1 (end)

		private TimeSpan _timeToLive = TimeSpan.FromSeconds (0.4); //how long the explosion lives

		private readonly string _spritename;

		public ExplosionAnimation (string spritename)
		{
			_spritename = spritename;
		}

		/// <summary>
		/// Start an explosion
		/// </summary>
		/// <param name="body">exploded body</param>
		public void Activate (Body body)
		{
			_position = body.Position * Constants.Scale;
			_rotation = body.Rotation;

			_transitionValue = 0f;
			_isActive = true;
		}

		public void Draw (SpriteBatch sb, Texture2D texture, GameTime elapsedGameTime)
		{
			if (_isActive) {
				UpdateTransition (elapsedGameTime);

				int colorChannel = 255 - (int)(_transitionValue * 255);
				int alphaCannel = 160 - (int)(_transitionValue * 160);
				Color color = new Color (colorChannel, colorChannel, colorChannel, alphaCannel);
				float scale = 0.75f + _transitionValue;

				Rectangle sourceRect = texture.Bounds;
				Vector2 origin = new Vector2 (sourceRect.Width / 2.0f, sourceRect.Height / 2.0f);

				sb.Draw (
				texture,
				_position,
				null,
				color,
				_rotation,
				origin,
				scale,
				SpriteEffects.None,
				0);
			}
		}

		private void UpdateTransition (GameTime elapsedGameTime)
		{
			// How much should we move by?
			float transitionDelta = (float)(elapsedGameTime.ElapsedGameTime.TotalMilliseconds /
										_timeToLive.TotalMilliseconds);

			// Update the transition position.
			_transitionValue += transitionDelta;

			// Did we reach the end of the transition?
			if (_transitionValue >= 1) {
				_transitionValue = 1f;
				_isActive = false;
			}
		}
	}
}

