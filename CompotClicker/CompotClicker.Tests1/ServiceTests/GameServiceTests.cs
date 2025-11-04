using Xunit;
using CompotClicker.Core.Services;


namespace CompotClicker.Core.Services
{
    public class GameServiceTests
    {
        [Fact]
        public void AddPoints_ShouldNotIncreaseTotalPoints()
        {
            var gameService = new GameService();
            var liczbaPunktowNaKlikniecie = gameService.PointsPerClick;

            gameService.AddClick();
            ;

            Assert.NotEqual(gameService.TotalPoints, gameService.TotalPoints + liczbaPunktowNaKlikniecie);
        }
    }
}
