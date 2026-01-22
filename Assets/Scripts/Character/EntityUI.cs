using UnityEngine;

public abstract class EntityUI : MonoBehaviour
{
    public void SpawnUI()
    {
        // TODO: We can add common UI spawning logic here in the future
        SpawnUISpecific();
    }
    //TODO Improve this, currently a hack for future inheritance implementation
    protected abstract void SpawnUISpecific();
}
