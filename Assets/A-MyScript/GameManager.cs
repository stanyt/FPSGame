
using UnityEngine;

public class GameManager : Singlton<GameManager>
{
    public bool GamePaused,Reloading;
    public GameData_SO GameData;
    public Character theCharacter;
    private Ray ray;
    public RaycastHit raycastHit;
    public ItemOnWorld currentItemOnWorldSelected;
    private Camera _mainCam;

    private void Start()
    {
        _mainCam = Camera.main;
    }

    private void Update()
    {
        if (UIManager.Instance.aimUI is null) return;
        ray = _mainCam.ScreenPointToRay(UIManager.Instance.aimUI.transform.position);
        Physics.Raycast(ray, out raycastHit);
        if (raycastHit.collider&& raycastHit.collider.TryGetComponent(out currentItemOnWorldSelected))
        {
            currentItemOnWorldSelected.IsSelected = true;
        }
    }
}
