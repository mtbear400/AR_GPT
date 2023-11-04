using UnityEngine;
using UnityEngine.UI;

public class PromptInjector : MonoBehaviour
{
    [SerializeField] private Button injectButton;   // The button you'll press to insert the recipe
    [SerializeField] private InputField targetInputField; // The input field you want to inject the recipe into
    [TextArea]
    [SerializeField] private string recipeText;     // Your pre-made recipe in text format

    private void Start()
    {
        injectButton.onClick.AddListener(InjectRecipe);
    }

    private void InjectRecipe()
    {
        targetInputField.text = recipeText;
    }
}
