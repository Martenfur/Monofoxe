# Creating new project

We got everything set up and ready, now let's make some games! Wait, Windows projects? Crossplatform projects? And some of them are blank? What is all that!?

![templates](Templates.png)

Yes, it seems quite confusing. If you want a short answer - just pick **Monofoxe Cross Platform Desktop Project**, and you'll be good to go. 



## I don't want a short answer, give me everything!

Basically, the main idea behind all this mess is - *each project type can only export to a single platform*. If you want to export to multiple platforms at the same time, you'll need a *shared project*. Shared project is just a container for the code which is shared between several projects in the solution. Its only purpose is to be connected to another platform-specific project. 

So, knowing that, we can divide projects into three categories:

- ![](CrossplatformProject.png) Crossplatofrm project - standalone project which contains everything necessary to run a game. It already has Windows, OpenGL and Shared projects setup.

- ![](SharedProject.png) Shared project - project which contains all the code and resources that should be shared between platforms. It is very much recommended to use it for big/crossplatform projects. 

- ![](WinOpenGLProjects.png) OpenGL\Windows projects - projects which are made specifically to go with Shared/Crossplatform projects. They doesn't contain any code or resources. The difference between them is that OpenGL can run on Windows and Linux, whereas Windows project runs on DirectX\Windows.  

- ![](LibraryProject.png) Library project - a project which compiles into a standalone library. Use it to separate your game into modules. Unlike shared project, libraries recompile only if their code has been changed, so you can use them to shorten the compile time. This is targeted more towards larger games with lots of code. Also note that library can't contain its own Content, but you can still use content from outside platforms projects or shared libraries though `ResourceHub` class.

  

## You keep talking about crossplatform support, but you've got a crossplatform OpenGL project right there, what's all that shared nonsense is about?

Don't look at me, it's all Monogame's fault.

![](FoxeHiding.png)

OpenGL project isn't that crossplatform if you actually look at it. It produces a single exe which can run on both Windows and Linux using some Mono magic. Monogame supports more platforms -- mobile, consoles, DirectX, all that good stuff. And it requires a setup with shared project.

Currently Monofoxe supports only DesktopGL and DirectX desktop platforms...

**But!**

You can throw default Monogame projects into the mix and it should work! Most likely. Plus, there is an Android project template coming eventually, so stay tuned.



## [<< Setting things up](SettingThingsUp.md)	|	[Monofoxe Basics >>](MonofoxeBasics.md)

[<<< Contents](Contents.md)