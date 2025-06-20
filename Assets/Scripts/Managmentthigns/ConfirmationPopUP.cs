using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class ConfirmationPopUP : MonoBehaviour
{
    public Button yesButton;
    public TextMeshProUGUI messageText;
    public Action onYesAction;
    public GameObject blockerOfThisMenu; // prevents pressing again on this one
    // Start is called before the first frame update
    void Start()
    {
        blockerOfThisMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Show(Action onYes)
    {

        onYesAction = onYes;

        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(() =>
        {
            onYesAction?.Invoke();
            CloseOnNo();
        });

    }

    public void CloseOnNo()
    {
        Destroy(gameObject);
    }

    public void CloseOnYes()
    {
        blockerOfThisMenu.SetActive(true);
    }
}
