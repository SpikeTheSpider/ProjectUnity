using UnityEngine;

public class ExampleInteractable : MonoBehaviour, IInteractable
{
    public void ShowInfo()
    {
        Debug.Log("Info about object");
    }

    public void Interact()
    {
        Debug.Log("Interacted with object");
    }

    public void Hack()
    {
        Debug.Log("Hacked object");
    }

    public string GetDefaultCode()
    {
        throw new System.NotImplementedException();
    }

    public string GetHelpInfo()
    {
        throw new System.NotImplementedException();
    }

    public string GetCurrentCode()
    {
        throw new System.NotImplementedException();
    }

    public void SetCurrentCode(string code)
    {
        throw new System.NotImplementedException();
    }

    public object GetContext()
    {
        throw new System.NotImplementedException();
    }
}