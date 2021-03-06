# Monofoxe basics

So now you should have a new project freshly created and ready to go. Let's look closer into what's inside.

First of all, we got `Game1` class. It is Monogame's main class which initializes the game and creates game loop.

What is a game loop?

![](FoxeLoop.png)

In the most basic terms, game loop looks like this:

```C#
Initialize();
var quit = false;
while(!quit)
{
	Update();
	Draw();
	WaitForNextFrame();
}
```

Game iterates through `Update()` and `Draw()` methods over and over again until the game ends. `Update()` has all of our game's logic, and `Draw()` has all the rendering logic.

You can look deeper into `Game1` in Monogame tutorials, but for the time being you don't need to worry about it.

The only thing you should notice from it is that it creates an instance of `GameController` class, which should initialize the rest of your game. `GameController` is just a regular entity, it's not necessary for the game to run.

By default it already has some simple logic. You can modify this class however you like.

The `Content` folder contains all the game's resources in their original format. it also has a `.mgcb` ([Content Pipeline](http://www.monogame.net/documentation/?page=Pipeline)) and `.npl` ([Nopipeline](https://github.com/Martenfur/Nopipeline)) configs which tell how resources should be processed. Don't worry about those for now.



## [<< Setting things up](SettingThingsUp.md) | [Input >>](Input.md)

[<<< Contents](Contents.md)

