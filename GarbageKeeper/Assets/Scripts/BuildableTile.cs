using UnityEngine;
using UnityEngine.EventSystems;

public class BuildableTile : MonoBehaviour
{
    private Tourelle _turretOnTile;
    private Material material;
    private Color initialColor;
    private bool _pointerOnTile;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        initialColor = material.color;
    }

    public void OnMouseEnter()
    {
        if (_turretOnTile)
        {
            material.color = Color.red;
        }
        else
        {
            material.color = Color.green;
        }
        _pointerOnTile = true;
    }

    public void OnMouseExit()
    {
        material.color = initialColor;
        _pointerOnTile = false;
    }

    private void OnDrawGizmos()
    {
        if (_pointerOnTile)
        {
            if(GameManager.Instance.CanBuyTurret() && (_turretOnTile == null ))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(this.transform.position, this.GetComponent<MeshRenderer>().bounds.size);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(this.transform.position, this.GetComponent<MeshRenderer>().bounds.size);
            }

            if (DebugManager.Instance.gameObject.activeSelf && _turretOnTile != null && DebugManager.Instance.showRangeAroundTurrets)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(this.transform.position, Settings.Instance.turretsNormalRange);
            }
        }
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.CanBuyTurret() && (_turretOnTile == null))
        {
            _turretOnTile = ((GameObject)Instantiate(Resources.Load("Prefabs/Turret"))).GetComponent<Tourelle>();
            _turretOnTile.transform.position = this.transform.position;
            GameManager.Instance.BuyTurret();
        }
        else if (Input.GetMouseButtonDown(1) && (_turretOnTile != null))
        {
            Destroy(_turretOnTile.gameObject);
            _turretOnTile = null;
            GameManager.Instance.RecycleTurret();
        }
    }
}
