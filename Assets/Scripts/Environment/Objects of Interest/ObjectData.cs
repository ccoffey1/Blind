using UnityEngine;

[CreateAssetMenu(menuName = "New Object Of Interest")]
public class ObjectData : ScriptableObject
{
    public Color TrailColor;
    public AudioClip[] SoundsOnPlayerCollision;
    public AudioClip[] SoundsOnSoundBulletCollision;
}
