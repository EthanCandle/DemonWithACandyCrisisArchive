using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class PlayerTypingTest : MonoBehaviour
{
    public string typedText = ""; // Stores the current typed text
    public TextMeshProUGUI displayText; // UI element to display typed text

    void Update()
    {
        // Check all letters (A-Z)
        for (int i = (int)Key.A; i <= (int)Key.Z; i++)
        {
            Key key = (Key)i;
            if (Keyboard.current[key].wasPressedThisFrame)
            {
                HandleLetterPress(key);
            }
        }

        // Check special keys
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            typedText += " ";
        }
        if (Keyboard.current.backspaceKey.wasPressedThisFrame && typedText.Length > 0)
        {
            typedText = typedText.Substring(0, typedText.Length - 1);
        }
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            SubmitText();
        }

        // Update the display
        if (displayText != null)
        {
            displayText.text = typedText;
        }
    }

    private void HandleLetterPress(Key key)
    {
        bool isShift = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;
        char letter = (char)(key - Key.A + 'a');
        typedText += isShift ? char.ToUpper(letter) : letter;
    }

    private void SubmitText()
    {
        Debug.Log("Submitted Text: " + typedText);
        typedText = ""; // Clear the input
    }
}
