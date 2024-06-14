
using System;

public class PromptTool : Singleton<PromptTool>
{
    public delegate void OnPromptChanged();
    public OnPromptChanged OnPromptChangedHandler;

    private string promptMessage = "";
    public string PromptMessage
    {
        get
        {
            return promptMessage;
        }
        set
        {
            promptMessage = value;
            OnPromptChangedHandler?.Invoke();
        }
    }
}
