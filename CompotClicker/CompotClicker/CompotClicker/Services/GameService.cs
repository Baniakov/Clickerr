using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;

namespace CompotClicker
{
    public class GameService
    {
        private static GameService _instance;
        public static GameService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameService();
                return _instance;
            }
        }

        public double TotalPoints { get; private set; }
        public double PointsPerClick { get; private set; }
        public int ClickCount { get; private set; }

        public HashSet<string> PurchasedItems { get; private set; }

        private GameService()
        {
            PurchasedItems = new HashSet<string>();
            LoadGame();
        }
        public void ResetGame()
        {
            TotalPoints = 0;
            PointsPerClick = 1; // Początkowa wartość punktów na kliknięcie
            ClickCount = 0;
            PurchasedItems.Clear(); // Usuwa wszystkie zakupione przedmioty
            SaveGame(); // Zapisz dane
        }

        public void AddClick()
        {
            TotalPoints += PointsPerClick;
            ClickCount++;
            SaveGame();
        }

        public bool TryPurchase(double cost, double bonusPerClick)
        {
            if (TotalPoints >= cost)
            {
                TotalPoints -= cost;
                PointsPerClick += bonusPerClick;
                SaveGame();
                return true;
            }
            return false;
        }

        public void MarkItemAsPurchased(string id)
        {
            if (!PurchasedItems.Contains(id))
                PurchasedItems.Add(id);

            SaveGame();
        }

        private void SaveGame()
        {
            Preferences.Set("TotalPoints", TotalPoints);
            Preferences.Set("PointsPerClick", PointsPerClick);
            Preferences.Set("ClickCount", ClickCount);
            Preferences.Set("PurchasedItems", string.Join(",", PurchasedItems.ToArray()));
        }


        private void LoadGame()
        {
            TotalPoints = Preferences.Get("TotalPoints", 0.0);
            PointsPerClick = Preferences.Get("PointsPerClick", 1.0);
            ClickCount = Preferences.Get("ClickCount", 0);

            var items = Preferences.Get("PurchasedItems", string.Empty);
            if (!string.IsNullOrEmpty(items))
            {
                var split = items.Split(',');
                PurchasedItems = new HashSet<string>(split);
            }
        }
    }
}