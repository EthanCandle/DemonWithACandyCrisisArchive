using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class ConfirmationPopUP : MonoBehaviour
{
    public Button yesButton, noButton;
    public TextMeshProUGUI messageText;
    public Action onYesAction;
    public GameObject blockerOfThisMenu; // prevents pressing again on this one
    public ReselectDefaultButton reselectButtonScript;
    public CanvasGroup thingToMakeUnInteractiveHolder;

    public Sound summonSound, deSummonSound;
    // Start is called before the first frame update
    void Start()
    {
        blockerOfThisMenu.SetActive(false);
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(summonSound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetReselectButton(ReselectDefaultButton reselectButtonScriptNew)
    {
        reselectButtonScript = reselectButtonScriptNew;
    }

    public void Show(Action onYes, CanvasGroup thingToMakeUnInteractive, ReselectDefaultButton reselectButtonScriptNew)
    {
        thingToMakeUnInteractiveHolder = thingToMakeUnInteractive;
        thingToMakeUnInteractiveHolder.interactable = false;
        onYesAction = onYes;
        SetReselectButton(reselectButtonScriptNew);
        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(() =>
        {
            onYesAction?.Invoke();
            CloseOnNo();
           

        });

        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(() =>
        {

            CloseOnNo();


        });

    }

    public void CloseOnNo()
    {
        thingToMakeUnInteractiveHolder.interactable = true;
        print("close on no pause");
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(deSummonSound);
        Destroy(gameObject);
    }

    public void CloseOnYes()
    {
        blockerOfThisMenu.SetActive(true);
    }
}
