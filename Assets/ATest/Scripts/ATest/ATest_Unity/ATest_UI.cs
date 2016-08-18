using UnityEngine;

public class ATest_UI : MonoBehaviour
{
    [HideInInspector][SerializeField] Rect _rect;
    [HideInInspector][SerializeField] Texture _texture;
    [HideInInspector][SerializeField] int _index;

    public Rect m_rect
    {
        get
        {
            return _rect;
        }
        set
        {
            _rect = value;
        }
    }
    public Texture m_texture
    {
        get
        {
            return _texture;
        }
        set
        {
            _texture = value;
        }
    }

    public int m_index
    {
        get
        {
            return _index;
        }
        set
        {
            _index = value;
        }
    }

}
