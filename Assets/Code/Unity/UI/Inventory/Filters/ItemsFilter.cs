using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectProvines.Unity.UI
{
    /// <summary>
    /// A class for filtering inventory cells.
    /// </summary>
    public class ItemsFilter : MonoBehaviour
    {
        /// <summary>
        /// Filters all cells depending on the selected filter.
        /// </summary>
        /// <param name="items">N is the number of inventory slots.</param>
        /// <param name="selectedFilter">The tag that will be used for verification.</param>
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