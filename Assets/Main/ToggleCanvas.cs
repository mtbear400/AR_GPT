using System.Collections.Generic;
using UnityEngine;

public class ToggleCanvas : MonoBehaviour
{
    // Reference to the list of Canvas GameObjects
    public List<GameObject> canvases;

    // Method to toggle the active state of all the Canvases
    public void ToggleActiveState()
    {
        // Iterate through each Canvas in the list
        foreach (GameObject canvas in canvases)
        {
            // Toggle the activeSelf value of the Canvas
            canvas.SetActive(!canvas.activeSelf);
        }
    }
}
