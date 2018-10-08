using UnityEngine;

public class SkinController : MonoBehaviour
{
    private int skinId;
    public int maxSkinId = 1;
    public GameObject skinSelectUI;

    private void Start()
    {
        Time.timeScale = 0;
    }

    public void Apply()
    {
        Time.timeScale = 1;
        skinSelectUI.SetActive(false);
    }

    private void DisableSkin(int skinId)
    {
        transform.FindChild(skinId.ToString()).gameObject.SetActive(false);
    }

    private void EnableSkin(int skinId)
    {
        transform.FindChild(skinId.ToString()).gameObject.SetActive(true);
    }

    public void Increase()
    {
        if (skinId >= maxSkinId) return;
        DisableSkin(skinId);
        skinId++;
        EnableSkin(skinId);
    }

    public void Descrease()
    {
        if (skinId <= 0) return;
        DisableSkin(skinId);
        skinId--;
        EnableSkin(skinId);
    }
}