# Time Keeper

Games are updated in discrete intervals. Knowing that, we can count time pretty easily. Assuming our game runs at 60 fps, we can do something like this...

```c#
int timer = 120; // Frames.

...

timer -= 1;
if (timer == 0)
	DoTheThing();
```

...and get a decent timer. 

But there is a big problem: it fully depends on the game's fps. What if we want to have dynamic fps or time scaling? Then we will have to use **deltatime**. Deltatime is essentially just a variable which tells how much time has passed since the previous frame. 

So now our timer will look like this:

```c#
int timer = 2; // Seconds.

...

timer -= dt;
if (timer <= 0)
	DoTheThing();
```



Now let's wrap dt into a nice class, and we got a `TimeKeeper`! 

`TimeKeeper` has two purposes:

- Tell you how much time it is.
- *Deceive* you.

Time-telling part is pretty straightforward. Let's rewrite the code above once again to use `TimeKeeper`.

```c#
int timer = 2; // Seconds.
TimeKeeper _keeper = new TimeKeeper();

...

timer -= _keeper.Time();
if (timer <= 0)
	DoTheThing();
```

You can also do speeds with it.

```c#
var movementSpeed = 100; // 100 pixels per second.

x += TimeKeeper.Global.Time(movementSpeed); // Speed will be automatically multiplied by the delta time.

// NOTE: Here we've used a static TimeKeeper.Global, which is a regular TimeKeeper instance, just created by the engine for conveniency.
```



And now, the main reason why you should use `TimeKeeper`. **The deceiving.**

`TimeKeeper` has a property named `TimeMultiplier`, which can trick your stuff into running faster or slower.

```c#
int timer = 2; // Seconds.
TimeKeeper _keeper = new TimeKeeper();
_keeper.TimeMultiplier = 0.5; // Now the timer will run for 4 seconds before firing.
```

You can have several time keepers to slow down some parts of the game, but keep others at normal speed. Can you feel the **power** that gives you?! :0

So yeah, I do recommend using `TimeKeeper` even if you have fixed fps.



## [<< Creating custom resource types](../Resources/CreatingCustomResourceTypes.md)  | []()

[<<< Contents](../Contents.md)

