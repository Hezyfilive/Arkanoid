using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private void Start()
    {
        //Change Foreground to the layer you want it to display on 
        //You could prob. make a public variable for this
        gameObject.GetComponent<Renderer>().sortingLayerName = "Score";
    }
}