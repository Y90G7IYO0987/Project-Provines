using ProjectProvines.Core.UI;
using ProjectProvines.Unity.Scriptables;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectProvines.Unity.UI
{
    /// <summary>
    /// The class that handles clicks on inventory cells.
    /// It is hung on an object in one inventory slot.
    /// </summary>
    public class ItemActions : MonoBehaviour, IPointerClickHandler
    {
        // Size change parameters for cell animation.
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

        /// <summary>
        /// Returns the key of the cell object (Guid).
        /// </summary>
        /// <returns>Cell Guid.</returns>
        public string GetItemGuid() => _itemGuid;

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
        /// Handling a click on inventory cell.
        /// </summary>
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
        /// The cell animation processes.
        /// Smoothly increases and returns to the base cell size.
        /// </summary>
        /// <param name="size">The size of the cell enlargement.</param>
        /// <param name="animateTime">Animation time.</param>
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