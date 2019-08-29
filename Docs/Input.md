# Input

To be able to play a game, we need some way to input information into it - with a controller, keyboard or mouse. Monofoxe provides basic way to read input with `Input` class, which resides in `Monofoxe.Engine` namespace. 

All the button readings are done with `CheckButton*()` methods. There is three of them:

- `CheckButtonPress()` - Checks if the button has just been pressed in the current frame.
- `CheckButton()` - Checks if the button is being held in the current step.
- `CheckButtonRelease()` - Checks if the button has just been released in the current step. 

These methods work with keyboard, gamepad and mouse buttons. They accept a button from `Buttons` enum and in case of gamepad - an index, since there can be several of them.

Here's an example of usage:

```c#
if (Input.CheckButtonPress(Buttons.A))
{
  Console.WriteLine("I've been pressed!");
}
if (Input.CheckButton(Buttons.A))
{
  Console.WriteLine("I am being held!");
}
if (Input.CheckButtonRelease(Buttons.A))
{
  Console.WriteLine("I've been released!");
}
```

There are also additional methods for gamepad, mouse position and clearing the input, you can look up their methods descriptions in the sources. 



## [<< Monofoxe Basics](MonofoxeBasics.md) | [Entities >>](Entities/Entities.md)

[<<< Contents](Contents.md)

