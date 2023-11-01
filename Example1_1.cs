using System;

namespace InterviewXFlow
{
    public class Player
    {
        private int health;

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

        public Player(int startingHealth)
        {
            Health = startingHealth;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
        }
    }

    class Program
    {
        private const int NewPlayerHealth = 100;
        private const int Damage = 10;
        private Player player;

        public static void Main(string[] args)
        {
            Program program = new Program();
            program.player = new Player(NewPlayerHealth);
            program.player.TakeDamage(Damage);
            Console.WriteLine($"Player's health: {program.player.Health}");
        }
    }
}