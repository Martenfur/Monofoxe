using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.Resources;
using Monofoxe.Engine.SceneSystem;

namespace Monofoxe.Playground.ECSDemo
{
	public class BotTemplate : IEntityTemplate
	{
		public string Tag => "Bot";

		Sprite _botSprite;

		public Entity Make(Layer layer)
		{
			// Bot is a pure ECS entity. This means, that it uses
			// non-derived Entity class with no logic in it.
			// But that poses a problem - where to assemble the entity?
			// For this purpose, entity templates exist.
			// You can assemble your entity here and then create it using
			// Entity.CreateFromTemplate(layer, "Bot");
			// 
			// Though, entity templates are not required. You can assemble new entities
			// anywhere you like. You can even add new components and remove existing
			// at any time.

			_botSprite = ResourceHub.GetResource<Sprite>("DefaultSprites", "Bot");

			var entity = new Entity(layer, Tag);

			entity.AddComponent(new PositionComponent(Vector2.Zero));
			entity.AddComponent(new ActorComponent(_botSprite));
			
			var bot = new BotComponent();

			// It is recommended to reuse random objects.
			bot.TurningSpeed = ECSDemoFactory.Random.Next(120, 240);
			
			entity.AddComponent(bot);
			
			return entity;
		}
	}
}

