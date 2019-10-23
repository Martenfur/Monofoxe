![logo](logo/logo_transparent.png)

# What am I looking at?
This is Monofoxe - a game engine based on the [Monogame Framework](http://monogame.net). 
Its main goal is to greatly simplify working with Monogame and to provide basic set of tools 
enabling you to just *create a new project and make a damn game* without removing low-level access to the framework.
Monofoxe took a lot of inspiration from Game Maker, so it should be a bit familiar to some folks.

**Current version: 2.0.0.0-dev** [View changelog](/CHANGELOG.md)
[Download in-dev build (recommended)](https://github.com/gnFur/Monofoxe/releases/tag/v2.0.0.0-dev%2B001)
[Download last stable release](https://github.com/gnFur/Monofoxe/releases/latest)


# What can it do?

Everything Monogame does, plus:

* Graphics pipeline and automated batch\vertex buffer management.
* Easy animation from sprite sheets.
* Texture packing.
* Sprite groups and dynamic graphics loading.
* Input management.
* Useful math for collisions and other game stuff.
* Timers, alarms, cameras, state machines, tilemaps, foxes!
* FMOD audio support (As a standalone [library](https://github.com/gnFur/ChaiFoxes.FMODAudio/)).
* Hybrid ECS.
* Scene system (with layers!).
* Tiled maps support.
* Enhanced resource management via [NoPipeline](https://github.com/gnFur/NoPipeline).


Coming in the future:

* Animated tiles and infinite tilemaps from Tiled.
* Particle system.
* Documentation.

# Can I use it in my p...

Yes, you can. Monofoxe is licensed under MPL 2.0, so you can use it and its code in any shenanigans you want. Free games, commercial games, your own the-coolest-in-the-world engines - no payment or royalties required. Just please leave a credit. ; - )
(Though, if you will be using FMOD, it has its own [license](https://fmod.com/licensing#faq), which is much less permissive than mine.)

# Should I use it?

Well, up to you. Currently I am developing Monofoxe alone, and can't really provide huge support, or anything. This is mostly an engine for myself and my games - I am not naive enough to think, that everyone will suddenly rush and drop Unity/Game Maker/whatever in favor of Monofoxe. But you can try. ( - :

# How do I use it?

Download the Monofoxe installer from the [latest release](https://github.com/gnFur/Monofoxe/releases/latest) or [in-dev build (recommended)](https://github.com/gnFur/Monofoxe/releases/tag/v2.0.0.0-dev%2B001).
Installer bundles Visual Studio 2015, 2017 and 2019 templates, [NoPipeline](https://github.com/gnFur/NoPipeline) and Monogame 3.7.1 installation. If you don't want 3.7.1, Monofoxe is confirmed to work on 3.6 and 3.7. It most likely will work on Monogame dev build, but it constantly changes, so you never know.

Just install Monofoxe, create Monofoxe project and you're good to go. 

You can also check out the [basic feature demos](Monofoxe.Playground/), [Demo game](https://bitbucket.org/gnFur/monofoxe.demo/) or the [Docs](Docs/Contents.md) (Currently not finished) to learn how to use Monofoxe.

# I've suddenly started loving foxes and want to contribute!

That's the spirit. Check out if I need any help on my [Quire board](https://quire.io/w/Monofoxe/?board=Monofoxe). Stuff under `Open for taking` category is, well, open for taking. You can also contact me via email (`chaifoxes@gmail.com`), on [Twitter](https://twitter.com/ChaiFoxes) or on Discord (`gn Fur#2490`).

**Don't forget to check out Codestyle.cs before contributing!!!**

## Foxes who helped

- [MirrorOfSun](https://github.com/MirrorOfSUn)
- [Shazan](https://bitbucket.org/%7B07c29368-d971-4ab1-8ec5-1a89d56bfa43%7D/)

*don't forget to pet your foxes*