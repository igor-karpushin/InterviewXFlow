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
        private event Action innerChanged;
        public event Action Changed {
            add {
                innerChanged += value;
                value();
            }
            remove {
                innerChanged -= value;
            }
        }

        public ExtendPlayer(int startingHealth) : base(startingHealth)
        {
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            innerChanged?.Invoke();
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
        private int? _previousHealth;
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
            ExtendPlayer.Changed += OnPlayerChanged;
            HitPlayer();
        }

        private void HitPlayer() => Player.TakeDamage(20);

        private void OnPlayerChanged()
        {
            _healthView.Text = Player.Health.ToString();
            if (_previousHealth != null && Player.Health - _previousHealth < -10) {
                _healthView.Text = "Color.Red";
            } else {
                _healthView.Text = "Color.White";
            }
            _previousHealth = Player.Health;
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