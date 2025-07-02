using TMPro;
using UnityEngine;

public class NumberSelector : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text numberText;

    int currentNumber = 0;
    public int CurrentNumber => currentNumber; // Property to access the current number

    private void Start()
    {
        if (numberText != null)
        {
            numberText.text = currentNumber.ToString(); // Initialize the displayed number
        }
    }

    public void IncrementNumber()
    {
        currentNumber = (currentNumber + 1) % 10; // Cycle through 0-9
        Debug.Log("Current Number: " + currentNumber);

        if(animator != null) animator.SetTrigger("Increment"); // Trigger the increment animation
        if (numberText != null) numberText.text = currentNumber.ToString(); // Update the displayed number
    }

    public void DecrementNumber()
    {
        currentNumber = (currentNumber - 1 + 10) % 10; // Cycle through 0-9
        Debug.Log("Current Number: " + currentNumber);

        if (animator != null) animator.SetTrigger("Decrement"); // Trigger the decrement animation
        if (numberText != null) numberText.text = currentNumber.ToString(); // Update the displayed number
    }
}
