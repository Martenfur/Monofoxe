# Introduction to ECS

Having just an `Entity` class is enough to make you game. In fact, this is where some engines like Game Maker end their effort. But practice shows that the code becomes a horrible mess. 

Let's say, you have an actor. It can run, jump and do all sorts of other stuff. That actor can be either controlled by AI or the player. So how do we do that? Inherit `NPC` and `Player` classes from some base `Actor` class with all the running-jumping logic? But wait, we already have an inheritance from `SolidObject`, since our actor should be able to collide with solid walls! So, now we have `Entity > SolidObject > Actor > Player/NPC` inheritance tree, which is horrible and very fragile. Oh, and also player should be magic user, so we'll need to stick `MagicUser` class inheritance somewhere in there. Wait a minute, we can't inherit two classes at the same time! Interfaces? Copypasting the same logic several times?! **What have we done?!**

![oh no](FoxeScared.png)



This is why you should use ECS.



## So what's ECS, exactly?

Entity Component System (your lord and savior, in short) is a pattern which consists out of three structures:

- Component -- Pure data. 
- Entity -- A container for components.
- System -- Pure logic.

So now entity is not just a solid structure -- it consists out of several components, which describe some part of the entity. Those components are processed by systems, which contain actual game logic.

This approach gives you:

- Less reliance on inheritance. Inheritance is fragile and tends to make a mess out of the code. With ECS you can have several completely unrelated components working together.
- Flexibility. You can have as many components as you want in a single entity, you can add and remove them at any time!
- Finer control. You can disable certain components if you'd like.

Now you can resolve the problem above by making `Actor`, `SolidObject` and `MagicUser` components and just adding them to an entity.



## But you can also have logic in Entity class, doesn't seem like ECS to me   

Exactly. Pure ECS does not allow any logic in Entity or Component. But we aren't purists, aren't we? : - )

Monofoxe features **hybrid ECS** -- entities can have their own logic too. It's also entirely possible to have logic in your entity **and** have additional components. Also nothing stops you from putting logic into components, although, I would not recommend that. 



## Then what's even the point of having logic in entities, if ECS is so cool?

ECS is great, but it's not a magical cure for everything. Sometimes it's actually better to use traditional entities.

- Use ECS, when dealing with core stuff which will be reused across several different classes. Position, physics, movement, etc.
- Use inheritance when dealing with specific objects whose code won't be heavily reused anywhere else.

Also keep in mind that using ECS is a bit slower than using entities.



## [<< Entities](Entity.md)	|	[Components >>](Components.md)

[<<< Contents](../Contents.md)

