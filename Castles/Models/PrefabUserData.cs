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

namespace Castles
{
	public enum BodyStatus
	{
		Active,
		ToBeDestroyed,
		Destroyed
	}

	public class PrefabUserData
	{
		public int IsDestructable {get;set;}
		public int HitPoints { get; set; }

		public BodyStatus Status { get; set; }

		public bool IsProjectile { get; set; }
		
		public PrefabType PrefabType {get;set;}

		public Vector2 Origin { get; set; }

		public ExplosionAnimation ExplosionAnimation { get; set; }

		public PrefabUserData ()
		{
			ExplosionAnimation = new ExplosionAnimation ("smoke");
		}

		public void Destroy (Body body)
		{
			Status = BodyStatus.Destroyed;
			ExplosionAnimation.Activate (body);
			//body.DestroyFixture();
			body.Dispose();
		}
		
		public void Draw (GameTime gameTime, SpriteBatch spriteBatch, Body body)
		{
			switch (Status) {
			case BodyStatus.Active:
				spriteBatch.Draw (TextureLoader.GetTexture(PrefabType),
					 body.Position * Constants.Scale, null, Color.White, body.Rotation, Origin * Constants.Scale, 1f,
											   SpriteEffects.None, 0f);
				break;
			case BodyStatus.Destroyed:
				ExplosionAnimation.Draw (spriteBatch, TextureLoader.GetTexture(PrefabType.Smoke), gameTime);
				break;
			default:
				return;
			}
			

		}
		
	}
}

