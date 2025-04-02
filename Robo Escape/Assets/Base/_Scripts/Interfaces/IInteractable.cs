public interface IInteractable
{
    void OnInteractionTrigger();
    void OnInteractionStay(float duration);
    void OnInteractionExit();
    public InteractionType InteractionType { get; }
}
