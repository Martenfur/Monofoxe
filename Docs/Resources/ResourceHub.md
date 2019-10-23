# Resource Hub

So we have our resources loaded, it's all nice, but where do we store them? The first solution that comes to mind - just hardcore everything in a couple of static classes, it will be fi-ine.

And it will, while we're working within single assembly. Problems arise when we introduce libraries into the mix. How would they access the resources? 

This problem is solved by the `ResourceHub`. It provides a centralized place for all libraries to access the resources. 

By itself `ResourceHub` doesn't do much -- essentially, it's just a container of `ResourceBox` classes, which hold the resources.

To access certain resource from the `ResourceHub`, use:

```C#
ResourceHub.GetResource<ResourceType>("ResourceBoxName", "ResourceName");
```

`ResourceType` - The type of the resource you want to access.

`"ResourceBoxName"` - The name of the resource box containig the resource. There can be several resource boxes which contain the same resource type, that's why we need the box name.

`"ResourceName"` - The name of the concrete resource you want to access.



**NOTE:** 

```C#
public class Foxe
{
    // You CANNOT do this. This code will execute before any ResourceBox is added to the 
    // ResourceHub, so you will always get an exception.
	public Sprite FoxeSprite = ResourceHub.GetResource<Sprite>("DefaultSprites", "Foxe");

    public Foxe()
    {
        // You SHOULD do it like this instead. By the time constructor will be called, 
        // ResourceHub will add all the ResourceBoxes.
        FoxeSprite = ResourceHub.GetResource<Sprite>("DefaultSprites", "Foxe");
    }
}
```



If you want to get whole ResourceBox, use

```C#
ResourceHub.GetResourceBox("ResourceBoxName");
```

Note that it will return `IResourceBox`, which you may want to cast to `ResourceBox<T>`.



You can look up additional methods in sources.



## [<< Adding resources](AddingResources.md) | [Resource Box >>](ResourceBox.md) 

[<<< Contents](../Contents.md)

