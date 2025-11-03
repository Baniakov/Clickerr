using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CompotClicker
{
    public class GameService
    {
        public event System.Action GameStateChanged;
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

        // --- XP i poziomy ---
        // XP całkowite (historia) — wpływa na level
        public double TotalExperience { get; private set; }

        // XP dostępne jako waluta (można nimi płacić w sklepie)
        public double AvailableExperience { get; private set; }

        // XP zgromadzone w bieżącym poziomie (postęp do kolejnego levela)
        public double ExperienceCurrent { get; private set; }

        public double XPToNextLevel { get; private set; }

        public int Level { get; private set; }

        // --- Punkty / klikanie (mechanika gry) ---
        public double TotalPoints { get; private set; }            // (opcjonalne — zachowane dla kompatybilności)
        public double PointsPerClick { get; private set; }
        public int ClickCount { get; private set; }

        public HashSet<string> PurchasedItems { get; private set; }

        private GameService()
        {
            PurchasedItems = new HashSet<string>();
            LoadGame();
        }
        private void OnGameStateChanged() => GameStateChanged?.Invoke();

        public void AddClick()
        {
            // Dodaj punkty (jeśli wciąż chcesz Points jako oddzielną walutę)
            TotalPoints += PointsPerClick;
            ClickCount++;

            // przy każdym kliknięciu gracz też zdobywa XP (to przykład — jeśli chcesz inny algorytm, powiedz)
            GainExperience(PointsPerClick);

            SaveGame();
            OnGameStateChanged();
        }

        /// <summary>
        /// Próbuj zakupu używając **dostępnego XP (waluty)**.
        /// Zwraca true jeśli zakup udany — dodaje bonus do PointsPerClick.
        /// </summary>
        public bool TryPurchaseXP(double cost, double bonusPerClick)
        {
            if (AvailableExperience >= cost)
            {
                AvailableExperience -= cost;
                PointsPerClick += bonusPerClick;
                SaveGame();
                OnGameStateChanged();
                return true;
            }
            return false;
        }

        /// <summary>
        /// (Stara metoda TryPurchase która używała TotalPoints została zachowana w razie potrzeby)
        /// </summary>
        public bool TryPurchase(double cost, double bonusPerClick)
        {
            if (TotalPoints >= cost)
            {
                TotalPoints -= cost;
                PointsPerClick += bonusPerClick;
                SaveGame();
                OnGameStateChanged();
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

        /// <summary>
        /// Dodaje XP: powiększa totalną historię, dostępną walutę oraz postęp w bieżącym levelu.
        /// Obsługuje zdobywanie kolejnych poziomów (Level up).
        /// </summary>
        private void GainExperience(double amount)
        {
            if (amount <= 0)
                return;

            TotalExperience += amount;
            AvailableExperience += amount;
            ExperienceCurrent += amount;

            bool leveled = false;
            while (ExperienceCurrent >= XPToNextLevel)
            {
                ExperienceCurrent -= XPToNextLevel;
                Level++;
                XPToNextLevel = CalculateXPRequirement(Level);
                leveled = true;
            }

            // zapis i powiadomienie
            SaveGame();
            OnGameStateChanged();

            // ewentualne dodatkowe działania przy level up - UI obsłuży alert (w ViewModelu)
        }

        private double CalculateXPRequirement(int level)
        {
            // Formuła skalowalna: bazowo 100, rośnie o 20% co poziom.
            // Możesz to zmienić (np. liniowo, wykładniczo itp.)
            return 100 * System.Math.Pow(1.2, level - 1);
        }

        private void SaveGame()
        {
            Preferences.Set("TotalPoints", TotalPoints);
            Preferences.Set("PointsPerClick", PointsPerClick);
            Preferences.Set("ClickCount", ClickCount);
            Preferences.Set("PurchasedItems", string.Join(",", PurchasedItems.ToArray()));

            // Nowe klucze XP
            Preferences.Set("TotalExperience", TotalExperience);
            Preferences.Set("AvailableExperience", AvailableExperience);
            Preferences.Set("ExperienceCurrent", ExperienceCurrent);
            Preferences.Set("Level", Level);
            Preferences.Set("XPToNextLevel", XPToNextLevel);
        }

        private void LoadGame()
        {
            // Punkty i klikanie (stare)
            TotalPoints = Preferences.Get("TotalPoints", 0.0);
            PointsPerClick = Preferences.Get("PointsPerClick", 1.0);
            ClickCount = Preferences.Get("ClickCount", 0);

            // XP / Level
            TotalExperience = Preferences.Get("TotalExperience", 0.0);
            AvailableExperience = Preferences.Get("AvailableExperience", 0.0);
            ExperienceCurrent = Preferences.Get("ExperienceCurrent", 0.0);

            Level = Preferences.Get("Level", 1);
            XPToNextLevel = Preferences.Get("XPToNextLevel", 0.0);

            // jeśli brak zapisanej wartości XPToNextLevel -> policz ją
            if (XPToNextLevel <= 0.0)
                XPToNextLevel = CalculateXPRequirement(Level);

            var items = Preferences.Get("PurchasedItems", string.Empty);
            if (!string.IsNullOrEmpty(items))
            {
                var split = items.Split(',');
                PurchasedItems = new HashSet<string>(split);
            }
        }
    }
}
