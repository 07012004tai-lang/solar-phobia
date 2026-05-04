using SolarPhobia.Domain;

namespace SolarPhobia.Domain.Repositories
{
    public interface IDialogueRepository
    {
        DialogueNode GetDialogue(string dialogueId);
    }
}

