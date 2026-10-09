using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectProvines.Unity.Doors
{
    public class DoorsController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            SceneManager.LoadScene(gameObject.tag);
        }
    }
}