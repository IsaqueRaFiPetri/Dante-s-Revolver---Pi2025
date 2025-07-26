using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

public interface Iinterectable
{
    public void OnInteract();
}
public class DialogueManager : MonoBehaviour
{
    [Serializable] public struct DialogueStruct
    {
        public string characterName;
        public string messageContent;
    }
    [SerializeField] DialogueStruct[] dialogueStruct;
    [SerializeField] TMP_Text nameTMP_Text;
    [SerializeField] TMP_Text messageTMP_Text;
    [SerializeField] float messageDelay;
    [SerializeField] UnityEvent OnEndDialogue;
    int dialogueStructIndex = 0;

    private void Start()
    {
        Dialogue();
    }
    public void Dialogue()
    {
        if (dialogueStructIndex >= dialogueStruct.Length - 1)
        {
            print("ended :p");
            OnEndDialogue.Invoke();
            StopCoroutine(ShowText());

            dialogueStructIndex = -1;
        }

        messageTMP_Text.text = "";
        dialogueStructIndex++;
        nameTMP_Text.text = dialogueStruct[dialogueStructIndex].characterName;
        StartCoroutine(ShowText());
    }
    IEnumerator ShowText()
    {
        gameObject.GetComponentInChildren<Button>().interactable = false;
        foreach (char letter in dialogueStruct[dialogueStructIndex].messageContent.ToCharArray())
        {
            messageTMP_Text.text += letter;

            yield return new WaitForSeconds(messageDelay);
        }
        gameObject.GetComponentInChildren<Button>().interactable = true;
    }
}
