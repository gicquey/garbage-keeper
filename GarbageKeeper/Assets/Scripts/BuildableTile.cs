using UnityEngine;
using UnityEngine.EventSystems;

public class BuildableTile : MonoBehaviour
{
    private Tourelle _turretOnTile;
    private bool _pointerOnTile;

    public void OnMouseEnter()
    {
        _pointerOnTile = true;
    }

    public void OnMouseExit()
    {
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
