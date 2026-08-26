using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryFilter : MonoBehaviour
{
    [SerializeField] private List<Button> filterButtons;

    private void Awake()
    {
        foreach(Button filterBtn in filterButtons)
        {
            if (filterBtn == null) continue;

            filterBtn.onClick.AddListener(() => OnFiltered(filterBtn));
        }
    }

    private void OnFiltered(Button clickedButton)
    {
        Debug.Log($"You clicked on {clickedButton.name}.");
    }
}
