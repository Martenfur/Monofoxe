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

Game iterates through `Update()` and `Draw()` methods over and over again until the game end. `Update()` has all of our game's logic, and `Draw()` has all the rendering logic.

You can look deeper into `Game1` in Monogame tutorials, but for the time being you don't need to worry about it.

The only thing you should notice from it is that it creates am instance of `GameController` class, which should initialize the rest of your game.

By default it already has some simple logic. You can modify this class however you like.

The `Content` folder contains all game's resources in their original format. it also has a `.mgcb` ([Content Pipeline](http://www.monogame.net/documentation/?page=Pipeline)) and `.npl` ([NoPipeline](https://github.com/gnFur/NoPipeline)) configs which tell how resources should be processed. Don't worry about those for now.

The last notable folder is `Resources`. It provides a way to load and reference your resources. More detailed explanation will be provided in Resources section. 



## [<< Creating new project](CreatingNewProject.md) | [Input >>](Input.md)

[<<< Contents](Contents.md)

