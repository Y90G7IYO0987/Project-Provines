using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectProvines.Unity.Doors
{
    /// <summary>
    /// Class for handling movements to other scenes.
    /// </summary>
    public class DoorsController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            SceneManager.LoadScene(gameObject.tag);
        }
    }
}