using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectProvines.Unity.UI
{
    /// <summary>
    /// Input handler for inventory cell filter buttons.
    /// </summary>
    public class FilterButtons : MonoBehaviour
    {
        [SerializeField] private float animateSize = 0.52f;
        [SerializeField] private float animateTime = 0.2f;

        [SerializeField] private InventoryController inventoryController;

        [SerializeField] private List<Button> buttons;

        private Dictionary<Button, bool> buttonToBool = new Dictionary<Button, bool>();

        private ItemsFilter _itemsFilter;

        private void Awake()
        {
            _itemsFilter = GetComponent<ItemsFilter>();

            for (int i = 0; i < buttons.Count; i++)
            {
                var button = buttons[i];
                if (!buttonToBool.ContainsKey(button))
                    buttonToBool[button] = false;
                button.onClick.AddListener(() => OnButtonClick(button));
            }
        }

        /// <summary>
        /// Processes clicks on the filter button.
        /// Sets a filter by tag.
        /// </summary>
        private void OnButtonClick(Button clickedButton)
        {
            if (buttonToBool.TryGetValue(clickedButton, out bool isAnimate))
            {
                if (!isAnimate)
                {
                    buttonToBool[clickedButton] = true;
                    StartCoroutine(AnimateClick(clickedButton, animateSize, animateTime));
                }

                _itemsFilter.FilterCells(inventoryController.GetInventoryItems(), clickedButton.tag);
            }
        }

        /// <summary>
        /// Filter button animation. Smooth increase and decrease in the button size.
        /// </summary>
        /// <param name="size">The size of the cell enlargement.</param>
        /// <param name="animateTime">Animation time.</param>
        private IEnumerator AnimateClick(Button button, float size, float animateTime)
        {
            Vector3 startScale = button.transform.localScale;
            Vector3 targetScale = startScale + new Vector3(size, size, 0);

            float halfTime = animateTime / 2f;
            float elapsedTime = 0f;

            while (elapsedTime < halfTime)
            {
                elapsedTime += Time.deltaTime;
                button.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / halfTime);
                yield return null;
            }

            elapsedTime = 0f;

            while (elapsedTime < halfTime)
            {
                elapsedTime += Time.deltaTime;
                button.transform.localScale = Vector3.Lerp(targetScale, startScale, elapsedTime / halfTime);
                yield return null;
            }

            buttonToBool[button] = false;
            button.transform.localScale = startScale;
        }
    }
}