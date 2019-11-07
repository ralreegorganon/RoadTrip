using DryIoc;
using RoadTrip;
using RoadTrip.Game;
using RoadTrip.Game.Components;
using Xunit;

namespace RoadTripTest
{
    public abstract class BaselineTest
    {
        protected BaselineTest()
        {
            God = new God();
            God.Container.Resolve<ScriptLoader>();
            var game = God.Container.Resolve<Game>();
            game.Setup();
        }

        protected God God { get; set; }
    }

    public class DumbTests : BaselineTest
    {
        [Fact]
        public void DoIt()
        {
            var game = God.Container.Resolve<Game>();
            var position = game.Player.Get<Position>();
            Assert.Equal(new Coordinate(0, 0, 0), position.Coordinate);
            var mcr = game.Player.Set<WantsToMove>();
            mcr.Movement = new Coordinate(1, 0, 0);
            game.Tick();
            Assert.Equal(new Coordinate(1, 0, 0), position.Coordinate);
        }
    }
}
