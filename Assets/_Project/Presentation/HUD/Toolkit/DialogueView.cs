using System;
using System.Collections.Generic;
using SolarPhobia.Domain;
using UnityEngine.UIElements;

namespace SolarPhobia.Presentation.HUD
{
    public sealed class DialogueView
    {
        private VisualElement _root;
        private Label _speakerLabel;
        private Label _dialogueLabel;
        private VisualElement _choicesContainer;

        public event Action<Choice> ChoiceSelected;

        public void Bind(VisualElement root)
        {
            _root = root;
            _speakerLabel = _root.Q<Label>("speaker-label");
            _dialogueLabel = _root.Q<Label>("dialogue-text");
            _choicesContainer = _root.Q<VisualElement>("choices-container");
        }

        public void Show(DialogueNode node)
        {
            if (node == null || _speakerLabel == null || _dialogueLabel == null || _choicesContainer == null)
            {
                return;
            }

            _speakerLabel.text = node.Character;
            _dialogueLabel.text = node.Text;
            RenderChoices(node.Choices);
        }

        public void Clear()
        {
            if (_speakerLabel != null)
            {
                _speakerLabel.text = string.Empty;
            }

            if (_dialogueLabel != null)
            {
                _dialogueLabel.text = string.Empty;
            }

            _choicesContainer?.Clear();
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
                var button = new Button(() => ChoiceSelected?.Invoke(choice))
                {
                    text = choice.Text
                };

                _choicesContainer.Add(button);
            }
        }
    }
}


