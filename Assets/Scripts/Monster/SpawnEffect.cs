using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.6875f);
    }

}
