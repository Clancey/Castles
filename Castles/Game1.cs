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
using GameStateManagement;

namespace Castles
{
	public class Game1 : Game
	{
		
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
		
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
		public Game1 ()
		{
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
			
			graphics.PreferredBackBufferWidth = BufferWidth;
            graphics.PreferredBackBufferHeight = BufferHeight;

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            //Create a new instance of the Screen Manager
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            //Switch to full screen for best game experience
            graphics.IsFullScreen = true;

            // TODO: Start with menu screen
            screenManager.AddScreen(new GameplayScreen(), null);

            // AudioManager.Initialize(this);
		}
	}
}

