
using UnityEngine.Events;
using UnityEngine;

public class Speech
{
    [SerializeField][Range(-1f, 9999f)] private int speechID = -1;
    private string speecher;
    private string speechText;
    private UnityAction speechAction = null;

    public int SpeechID => speechID;
    public string Speecher => speecher;
    public string SpeechText => speechText;
    public UnityAction SpeechAction => speechAction;


    public Speech(int speechID, string speecher, string speechText)
    {
        this.speechID = speechID;
        this.speecher = speecher;
        this.speechText = speechText;
        this.speechAction = null;
    }

    public Speech(int speechID, string speecher, string speechText, UnityAction speechAction)
    {
        this.speechID = speechID;
        this.speecher = speecher;
        this.speechText = speechText;
        this.speechAction = speechAction;
    }

    public void AddSpeechAction(UnityAction additionAction)
    {
        if (speechAction == null)
        {
            speechAction = additionAction;
        }
        else
        {
            speechAction += additionAction;
        }
    }

    public void AddSpeechEvent(UnityEvent additionEvent)
    {
        UnityAction actionToAdd = new UnityAction(additionEvent.Invoke);
        AddSpeechAction(actionToAdd);
    }
}
