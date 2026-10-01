using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectProvines.Unity.UI
{
    public class ItemsFilter : MonoBehaviour
    {
        /// <summary>
        /// Фильтрует все ячейки в зависимости от выбранного фильтра.
        /// </summary>
        /// <param name="items">N количество ячеек инвентаря.</param>
        /// <param name="selectedFilter">Тег по которому пойдет проверка.</param>
        public void FilterCells(List<Image> items, string selectedFilter)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var cell = items[i];
                if (!cell.CompareTag(selectedFilter))
                {
                    cell.enabled = false;
                    continue;
                }

                cell.enabled = true;
            }
        }
    }
}