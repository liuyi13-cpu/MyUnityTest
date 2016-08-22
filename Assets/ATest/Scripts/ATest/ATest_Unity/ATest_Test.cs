using UnityEngine;
using UnityEngine.UI;

public class ATest_Test : MonoBehaviour
{
    public Material mat;
    void Start()
    {
        if (mat)
        {
            var tex = mat.GetTexture("_AlphaTex");
            mat.SetTexture("_AlphaTex", null);
        }

        var cachedTransform = transform;
        cachedTransform.position = new Vector3(100, 0,0);
    }
}
