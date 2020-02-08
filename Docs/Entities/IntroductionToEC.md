# Introduction to EC

Having just an `Entity` class is enough to make your game. In fact, this is where some engines like Game Maker end their effort. But practice shows that the code becomes a horrible mess this way. 

Let's say, you have an actor. It can run, jump and do all sorts of other stuff. That actor can be either controlled by AI or the player. So how do we do that? Inherit `NPC` and `Player` classes from some base `Actor` class with all the running-jumping logic? But wait, we already have an inheritance from `SolidObject`, since our actor should be able to collide with solid walls! So, now we have `Entity > SolidObject > Actor > Player/NPC` inheritance tree, which is horrible and very fragile. Oh, and also player should be magic user, so we'll need to stick `MagicUser` class inheritance somewhere in there. Wait a minute, we can't inherit two classes at the same time! Interfaces? Copypasting the same logic several times?! **What have we done?!**

![oh no](FoxeScared.png)



This is why you should use EC.



## So what's EC, exactly?

Entity Component (your lord and savior, in short) is a pattern which consists out of two structures:

- Entity - A container for components. 
- Component - Sort of a mini-entity with its own Update/Draw methods. Components depend on entities and cannot exist in a game loop on their own.

So now entity is not just a solid structure - it consists out of several components, which describe different parts of the entity. 

This approach gives you:

- Less reliance on inheritance. Inheritance is fragile and tends to make a mess out of the code. With EC you can have several completely unrelated components working together.
- Flexibility. You can have as many components as you want in a single entity, you can add and remove them at any time! It's even possible to transfer components from one entity to another.
- Finer control. You can disable certain components if you'd like.

Now we can resolve the problem above by making `Actor`, `SolidObject` and `MagicUser` components and just adding them to an entity.



## But you can also have logic in Entity class, doesn't seem like EC to me   

Exactly. Pure EC does not allow any logic in Entity. But we aren't purists, aren't we? : - )

Monofoxe features **hybrid EC** - entities can have their own logic too. It's also entirely possible to have logic in your entity **and** have additional components.



## Then what's even the point of having logic in entities, if EC is so cool?

EC is great, but it's not a magical cure for everything. Sometimes it's actually better to use traditional entities.

- Use EC, when dealing with core stuff which will be reused across several different classes. Position, physics, movement, etc.
- Use inheritance when dealing with specific objects whose code won't be heavily reused anywhere else.



## [<< Entities](Entities.md)	|	[Components >>](Components.md)

[<<< Contents](../Contents.md)

