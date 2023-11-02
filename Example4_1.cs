using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace InterviewXFlow
{
    public class CheatManager
    {
        // proper singleton
        private static CheatManager _instance;
        public static CheatManager Instance => _instance ?? (_instance = new CheatManager());

        private readonly List<ICheatProvider> _providers = new List<ICheatProvider>();
        private GameObject _panelPrefab;
        private CheatElementBehaviour _cheatElement;
        private GameObject _panel;

        public void Initialize(GameObject panelPrefab, CheatElementBehaviour cheatElement)
        {
            _panelPrefab = panelPrefab;
            _cheatElement = cheatElement;
            RedrawPanel();
        }

        public void RegisterProvider(ICheatProvider provider)
        {
            _providers.Add(provider);
            RedrawPanel();
        }

        public void ShowCheatPanel()
        {
            if (_panelPrefab == null)
            {
                UnityEngine.Debug.Error("Not Initialized");
                return;
            }

            if (_panel != null)
                return;

            _panel = UnityEngine.Object.Instantiate(_panelPrefab);
            foreach (var provider in _providers)
            {
                foreach (var cheatAction in provider.GetCheatActions())
                {
                    var element = UnityEngine.Object.Instantiate(_cheatElement, _panel.transform);
                    element.Setup(cheatAction);
                }
            }
        }

        public void HideCheatPanel()
        {
            if (_panel == null)
                return;

            UnityEngine.Object.Destroy(_panel);
            _panel = null;
        }
        
        private void RedrawPanel()
        {
            // panel is opened -> redraw
            if (_panel != null)
            {
                HideCheatPanel();
                ShowCheatPanel();
            }
        }
    }

    public interface ICheatDescription
    {
        string Name { get; }
        Action CheatAction { get; }
    }

    public class CheatActionDescription : ICheatDescription
    {
        public string Name { get; }
        public Action CheatAction { get; }

        public CheatActionDescription(string name, Action cheatAction)
        {
            Name = name;
            CheatAction = cheatAction;
        }
    }

    public class CheatElementBehaviour : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private Button _button;

        public void Setup(ICheatDescription description)
        {
            _text.text = description.Name;
            _button.onClick.AddListener(description.CheatAction);
        }
    }

    public interface ICheatProvider
    {
        IEnumerable<CheatActionDescription> GetCheatActions();
    }

    public class SomeManagerWithCheats : ICheatProvider
    {
        private int _health;

        public void Initialize()
        {
            CheatManager.Instance.RegisterProvider(this);
        }

        public IEnumerable<CheatActionDescription> GetCheatActions()
        {
            yield return new CheatActionDescription("Cheat health", () => _health++);
            yield return new CheatActionDescription("Reset health", () => _health = 0);
        }
    }
}