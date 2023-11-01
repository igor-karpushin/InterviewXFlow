using System;
using Newtonsoft.Json;

namespace InterviewXFlow
{
    // Create an interface for data loading.
    public interface IDataLoader<out T> { T Load(string filePath);}

    // Implement a FileDataLoader class that implements the IDataLoader interface.
    public class FileDataLoader<T> : IDataLoader<T>
    {
        public T Load(string filePath)
        {
            try
            {
                var json = System.IO.File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                // Handle file loading or deserialization errors here.
                Console.WriteLine($"Error loading data from {filePath}: {ex.Message}");
                throw;
            }
        }
    }
    
    // Create a GameManager class to encapsulate game logic.
    public class GameManager
    {
        private readonly IDataLoader<Player> playerLoader;
        private readonly IDataLoader<Settings> settingsLoader;

        public GameManager(IDataLoader<Player> playerLoader, IDataLoader<Settings> settingsLoader)
        {
            this.playerLoader = playerLoader;
            this.settingsLoader = settingsLoader;
        }

        public void StartGame(string playerFilePath, string settingsFilePath)
        {
            try
            {
                var player = playerLoader.Load(playerFilePath);
                var settings = settingsLoader.Load(settingsFilePath);

                player.Hit(settings.Damage);
                
                Console.WriteLine($"Damage: {settings.Damage} Health: {player.Health}");

                // Game logic and other operations.
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the game.
                Console.WriteLine($"Game error: {ex.Message}");
            }
        }
    }
    
    public class Player
    {
        [JsonProperty("Health")] private int health;

        [JsonIgnore]
        public int Health
        {
            get => health;
            private set
            {
                if (value >= 0)
                    health = value;
                else
                    Console.WriteLine("Health cannot be negative.");
            }
        }

        public void Hit(int damage) => Health -= damage;
    }
    
    public class Settings
    {
        [JsonProperty("Damage")] private int damage;
        [JsonIgnore] public int Damage => damage;
    }

    class Program
    {
        public static void Main(string[] args)
        {
            // Configure file paths and other settings.
            const string playerFilePath = "NewPlayer.json";
            const string settingsFilePath = "Settings.json";

            var playerLoader = new FileDataLoader<Player>();
            var settingsLoader = new FileDataLoader<Settings>();
            var gameManager = new GameManager(playerLoader, settingsLoader);

            // Start the game with the specified file paths.
            gameManager.StartGame(playerFilePath, settingsFilePath);
        }
    }
}