using UnityEngine;

public class TagTarget : MonoBehaviour
{
    public PlayerIdentity Owner { get; private set; }

    public void Initialize(PlayerIdentity owner)
    {
        Owner = owner;
    }
}