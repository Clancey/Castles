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
using Microsoft.Xna.Framework.Graphics;
using GameStateManagement;

namespace Castles
{
	public static class TextureLoader
	{
		static Dictionary<PrefabType,Texture2D> textures;
		static TextureLoader ()
		{
			textures = new Dictionary<PrefabType, Texture2D>();
		}
		
		public static void Unload()
		{
			foreach(var texture in textures)
			{
				texture.Value.Dispose();
			}
			textures = new Dictionary<PrefabType, Texture2D>();
		}
		
		public static void Load(GameScreen gameScreen, PrefabType prefabType)
		{
			if(textures.ContainsKey(prefabType))
				return;
			textures.Add(prefabType, gameScreen.Load<Texture2D>(GetTextureName(prefabType)));
			
		}
		
		public static Texture2D GetTexture(PrefabType prefabType)
		{
			return textures[prefabType];
		}
		
		
		public static string GetTextureName (PrefabType prefabType)
		{
			switch (prefabType) {
			case PrefabType.Ball:
					return "ball";
			case PrefabType.GoldBox: 
				return "box1";
			case PrefabType.GrayBox: 
				return "box2";
			case PrefabType.Pig:
				return "pig";
			case PrefabType.Smoke:
				return "smoke";
			case PrefabType.TriangleLeft: 
				return "triangleL";
			case PrefabType.TriangleRight:
				return "triangleR";
			case PrefabType.Floor:
				return "floor";
				//Wood
			case PrefabType.Wood100x20:
				return "Textures/Wood/100x20";
			case PrefabType.Wood175x20:
				return "Textures/Wood/175x20";
			case PrefabType.Wood200x20:
				return "Textures/Wood/200x20";
			case PrefabType.Wood25x20:
				return "Textures/Wood/25x20";
			case PrefabType.Wood50x20:
				return "Textures/Wood/50x20";
			//ICE
			case PrefabType.Ice100x20:
				return "Textures/Ice/100x20";
			case PrefabType.Ice175x20:
				return "Textures/Ice/175x20";
			case PrefabType.Ice200x20:
				return "Textures/Ice/200x20";
			case PrefabType.Ice25x20:
				return "Textures/Ice/25x20";
			case PrefabType.Ice50x20:
				return "Textures/Ice/50x20";
				
			}
			return "";
		}

	}
}

