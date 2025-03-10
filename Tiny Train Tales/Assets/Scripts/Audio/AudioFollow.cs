using UnityEngine;

public class AudioFollow : MonoBehaviour
{
    [SerializeField] bool isMusic;

    public bool GetIsMusic
    {
        get { return isMusic; }
        set { isMusic = value; }
    }
}
