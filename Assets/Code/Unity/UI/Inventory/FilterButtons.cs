using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectProvines.Unity.UI
{
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
        /// Обрабатывает нажатия по кнопке фильтра, и добавляет фильтр по тегу.
        /// </summary>
        /// <param name="clickedButton"></param>
        private void OnButtonClick(Button clickedButton)
        {
            if (buttonToBool.TryGetValue(clickedButton, out bool isAnimate))
            {
                if (!isAnimate)
                {
                    buttonToBool[clickedButton] = true;
                    StartCoroutine(AnimateClick(clickedButton, animateSize, animateTime));
                }

                Debug.Log(clickedButton.tag);
                _itemsFilter.FilterCells(inventoryController.GetInventoryItems(), clickedButton.tag);
            }
        }

        /// <summary>
        /// Анимация кнопки фильтра. Плавно увеличивает и уменьшает размер кнопки.
        /// </summary>
        /// <param name="item">Объект ячейки.</param>
        /// <param name="size">Размер увеличения ячейки.</param>
        /// <param name="animateTime">Время анимации.</param>
        /// <returns></returns>
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