#if UNITY_EDITOR
using UnityEngine;

namespace Utilities
{
    public class Notes : MonoBehaviour
    {
        [TextArea(15, 1000)] public string notes;
    }
}
#endif
