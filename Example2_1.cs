using System;

namespace InterviewXFlow
{
    interface IPlayer
    {
        int Health { get; }
        void TakeDamage(int damage);
    }

    public class Player : IPlayer
    {
        private int _health;

        public int Health
        {
            get => _health;
            private set
            {
                if (value >= 0)
                    _health = value;
                else
                    Console.WriteLine("Health cannot be negative.");
            }
        }

        public Player(int startingHealth) => Health = startingHealth;
        public virtual void TakeDamage(int damage) => Health -= damage;
    }

    class ExtendPlayer : Player
    {
        public delegate void HealthChangedDelegate(int oldHealth, int newHealth);

        public event HealthChangedDelegate HealthChanged;

        public ExtendPlayer(int startingHealth) : base(startingHealth)
        {
        }

        public override void TakeDamage(int damage)
        {
            var oldHealth = Health;
            base.TakeDamage(damage);
            HealthChanged?.Invoke(oldHealth, Health);
        }
    }

    public class TextView
    {
        public string Text
        {
            set => Console.WriteLine(value);
        }
    }

    class Bootstrap
    {
        protected const int NewPlayerHealth = 100;
        public IPlayer Player { get; protected set; }
        protected Bootstrap() => Player = new Player(NewPlayerHealth);

        public virtual void Start()
        {
        }
    }

    class ExtendBootstrap : Bootstrap
    {
        private readonly TextView _healthView;
        private ExtendPlayer ExtendPlayer => (ExtendPlayer)Player;

        public ExtendBootstrap()
        {
            Player = new ExtendPlayer(NewPlayerHealth);
            _healthView = new TextView();
        }

        public override void Start()
        {
            base.Start();
            _healthView.Text = Player.Health.ToString();
            ExtendPlayer.HealthChanged += OnPlayerHealthChanged;
            HitPlayer();
        }

        private void HitPlayer() => Player.TakeDamage(20);

        private void OnPlayerHealthChanged(int oldHealth, int newHealth)
        {
            _healthView.Text = newHealth.ToString();
            if (newHealth - oldHealth < -10)
            {
                _healthView.Text = "Color.Red";
            }
            else
            {
                _healthView.Text = "Color.White";
            }
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            var bootstrap = new ExtendBootstrap();
            bootstrap.Start();
        }
    }
}