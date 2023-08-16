using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private void Start()
    {
        gameObject.GetComponent<Renderer>().sortingLayerName = "Score";
    }
}
