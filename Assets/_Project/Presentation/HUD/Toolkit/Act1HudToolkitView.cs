using System.Collections.Generic;
using System;
using ObservableCollections;
using SolarPhobia.Application.Messages;
using SolarPhobia.Domain;
using UnityEngine;
using UnityEngine.UIElements;

namespace SolarPhobia.Presentation.HUD.Toolkit
{
    [RequireComponent(typeof(UIDocument))]
    public class Act1HudToolkitView : MonoBehaviour
    {
        private VisualElement _root;
        private Label _phaseLabel;
        private Label _kindnessLabel;
        private Label _spiritLabel;
        private Label _dialogueLabel;
        private VisualElement _choicesContainer;
        private VisualElement _ordersContainer;

        public GameSessionState State { get; private set; }
        public event Action<Choice> ChoiceSelected;

        private void Awake()
        {
            var document = GetComponent<UIDocument>();
            _root = document.rootVisualElement;
            BuildUi();
        }

        public void Bind(GameSessionState state)
        {
            State = state;
            RefreshState();
        }

        public void ShowDialogue(DialogueNode node)
        {
            if (node == null)
            {
                return;
            }

            _dialogueLabel.text = $"{node.Character}: {node.Text}";
            RenderChoices(node.Choices);
        }

        public void RenderOrders(ObservableDictionary<OrderType, int> itemsNeeded)
        {
            _ordersContainer.Clear();

            if (itemsNeeded == null)
            {
                return;
            }

            foreach (var pair in itemsNeeded)
            {
                _ordersContainer.Add(new Label($"{pair.Key}: {pair.Value}"));
            }
        }

        public void RefreshState()
        {
            if (State == null)
            {
                return;
            }

            _phaseLabel.text = $"Phase: {State.CurrentPhase}";
            _kindnessLabel.text = $"Kindness: {State.KindnessScore.Value}";
            _spiritLabel.text = $"Spirit: {State.SpiritEssence}";
        }

        private void BuildUi()
        {
            _root.Clear();
            _root.style.flexDirection = FlexDirection.Column;
            _root.style.paddingLeft = 12;
            _root.style.paddingRight = 12;
            _root.style.paddingTop = 12;
            _root.style.paddingBottom = 12;

            _phaseLabel = new Label("Phase: BOOT");
            _kindnessLabel = new Label("Kindness: 0");
            _spiritLabel = new Label("Spirit: 0");
            _dialogueLabel = new Label("Dialogue will appear here.");
            _choicesContainer = new VisualElement();
            _ordersContainer = new VisualElement();

            _choicesContainer.style.flexDirection = FlexDirection.Column;
            _ordersContainer.style.flexDirection = FlexDirection.Column;

            _root.Add(_phaseLabel);
            _root.Add(_kindnessLabel);
            _root.Add(_spiritLabel);
            _root.Add(new Label("--- Dialogue ---"));
            _root.Add(_dialogueLabel);
            _root.Add(new Label("--- Choices ---"));
            _root.Add(_choicesContainer);
            _root.Add(new Label("--- Orders ---"));
            _root.Add(_ordersContainer);
        }

        private void RenderChoices(IReadOnlyList<Choice> choices)
        {
            _choicesContainer.Clear();

            if (choices == null)
            {
                return;
            }

            foreach (var choice in choices)
            {
                var button = new Button(() => { })
                {
                    text = choice.Text
                };

                button.clicked += () => ChoiceSelected?.Invoke(choice);

                _choicesContainer.Add(button);
            }
        }
    }
}

