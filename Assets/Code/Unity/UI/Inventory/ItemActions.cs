using ProjectProvines.Core.UI;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectProvines.Unity.UI
{
    public class ItemActions : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private float _animateSize = 0.52f;
        [SerializeField] private float _animateTime = 0.24f;

        private CellData _itemData;
        private Image _itemImage;

        private SelectedItemData _selectedItemData;

        private TextMeshProUGUI _titleText;
        private TextMeshProUGUI _nameText;

        private Vector2 _defaultSize;

        private string _itemGuid;
        private bool _isAnimate;

        private void Awake()
        {
            _itemImage = GetComponent<Image>();
            _defaultSize = transform.localScale;
        }

        public void Initialize(ActionsData data)
        {
            _itemData = data.ItemData;
            _itemGuid = data.ItemGuid;

            _selectedItemData = data.SelectedItemData;

            _titleText = data.TitleText;
            _nameText = data.NameText;
        }

        /// <summary>
        /// Обработка клика на ячейку инвентаря.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isAnimate)
                StartCoroutine(AnimateClick(_animateSize, _animateTime));

            if (_selectedItemData.ItemGuid == _itemGuid)
            {
                Debug.LogWarning($"Item with {_itemGuid} was already equipped!");
                return;
            }

            _selectedItemData.SetNewItem(_itemImage, _itemData, _itemGuid);

            _titleText.text = _itemData.Description;
            _nameText.text = _itemData.Name;

            Debug.Log($"Clicked to {gameObject.name}");
        }

        /// <summary>
        /// Анимация ячейки. Плавно увеличивает и уменьшает размер ячейки.
        /// </summary>
        /// <param name="item">Объект ячейки.</param>
        /// <param name="size">Размер увеличения ячейки.</param>
        /// <param name="animateTime">Время анимации.</param>
        /// <returns></returns>
        private IEnumerator AnimateClick(float size, float animateTime)
        {
            _isAnimate = true;

            Vector3 startScale = _defaultSize;
            Vector3 targetScale = startScale + new Vector3(size, size, 0);

            float halfTime = animateTime / 2f;
            float elapsedTime = 0f;

            while (elapsedTime < halfTime)
            {
                elapsedTime += Time.deltaTime;
                transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / halfTime);
                yield return null;
            }

            elapsedTime = 0f;

            while (elapsedTime < halfTime)
            {
                elapsedTime += Time.deltaTime;
                transform.localScale = Vector3.Lerp(targetScale, startScale, elapsedTime / halfTime);
                yield return null;
            }

            transform.localScale = startScale;
            _isAnimate = false;
        }
    }
}