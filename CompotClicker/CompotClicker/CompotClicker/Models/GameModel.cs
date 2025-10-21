using System;
using System.Collections.Generic;
using System.Text;

namespace CompotClicker
{
    public class GameModel
    {
        public double TotalPoints { get; set; }
        public double PointsPerClick { get; set; } = 1;
        public double PointsPerSecond { get; set; } = 0;
        public int ClickCount { get; set; }

        public void AddClick()
        {
            TotalPoints += PointsPerClick;
            ClickCount++;
        }

        public void AddPassivePoints(double deltaTime)
        {
            TotalPoints += PointsPerSecond * deltaTime;
        }

        public bool TryPurchase(double cost)
        {
            if (TotalPoints >= cost)
            {
                TotalPoints -= cost;
                return true;
            }
            return false;
        }
    }

}