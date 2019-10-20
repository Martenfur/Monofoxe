# Creating new project

We got everything set up and ready, now let's make some games! Wait, Windows projects? Crossplatform projects? And some of them are blank? What is all that!?

![templates](Templates.png)

Yes, it seems quite confusing. If you want a short answer - just pick **Monofoxe Cross Platform Desktop Project**, and you'll be good to go. 



## I don't want a short answer, give me everything!

Basically, the main idea behind all this mess is - *each project type can only export to a single platform*. If you want to export to multiple platforms at the same time, you'll need a *shared project*. Shared project is just a container for the code which is shared between several projects in the solution. Its only purpose is to be connected to another platform-specific project. 

So, knowing that, we can divide projects into three categories:

- ![](CrossplatformProject.png) Basic Desktop GL project - standalone project which contains everything necessary to run a game. I recommend using them for small and quick projects which doesn't need to be ported to other platforms.

- ![](SharedProject.png) Shared project - project which contains all the code and resources that should be shared between platforms. It is very much recommended to use it for big/crossplatform projects. 

- ![](WinCrossplatformProjectsEmpty.png) Blank Crossplatform\Windows projects - projects which are made specifically to go with Shared project. They doesn't contain any code or resources. The difference between them is that Crossplatform runs on OpenGL and can run on Windows and Linux, whereas Windows project runs on DirectX and only on Windows.  

- ![](LibraryProject.png) Library project - a project which compiles into a standalone library. Use it to separate your game into modules. Unlike shared project, libraries recompile only if their code has been changed, so you can use them to shorten the compile time. This is targeted more towards larger games with lots of code. Also note that library can't contain its own Content, but you can still use content from outside platforms projects or shared libraries though `ResourceHub` class.

  

## Ok, but how do I set up all this?

- Create a Shared project. This will automatically create a solution, which is just a container for projects.
- Create a new project in the same solution for whatever platform you'd like. I recommend naming it `<SharedProjectName>.<Platform>`, so if you're creating new Windows project for a shared project named `TestProject`, name it `TestProject.Windows`. You can name them however you like, tho, but I found that this works the best.
- Now select your platform-specific project, right-click on its `References`, click `Add Reference`, find your shared project and add it.
- Open `Program.cs` and make sure Shared project's namespace is included, so `Game1` class is visible. 

And now you should be ready to go!



## You keep talking about crossplatform support, but you've got a crossplatform project right there, what's all that shared nonsense is about?

Don't look at me, it's all Monogame's fault.

![](FoxeHiding.png)

Crossplatform project isn't that crossplatform if you actually look at it. It produces a single exe which can run on both Windows and Linux using some Mono magic. Monogame supports more platforms -- mobile, consoles, DirectX, all that good stuff. And it requires a setup with shared project.

Currently Monofoxe supports only DesktopGL and DirectX desktop platforms...

**But!**

You can throw default Monogame projects into the mix and it should work! Most likely. Plus, there is an Android project template coming eventually, so stay tuned.



## [<< Setting things up](SettingThingsUp.md)	|	[Monofoxe Basics >>](MonofoxeBasics.md)

[<<< Contents](Contents.md)