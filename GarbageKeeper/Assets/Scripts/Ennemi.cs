using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Ennemi : MonoBehaviour
{
    public float movingSpeed = 2f;

    private Transform _lastCheckpointReached = null;
    private Transform _nextCheckpoint = null;

    private void Awake()
    {
        StartWalking();
    }

    private void Update()
    {
        if(_nextCheckpoint != null)
        {
            if (Vector3.Distance(this.transform.position, _nextCheckpoint.position) < 0.1F)
            {
                ChangeDirection();
                return;
            }

            var travelDirection = Vector3.Normalize(_nextCheckpoint.position - _lastCheckpointReached.position);
            this.transform.Translate(
                movingSpeed * travelDirection.x,
                movingSpeed * travelDirection.y,
                movingSpeed * travelDirection.z,
                Space.World);

            var directionOfCheckpoint = Vector3.Normalize(_nextCheckpoint.position - this.transform.position);

            if (Vector3.Dot(directionOfCheckpoint, travelDirection) < 0)
            {
                //If the e nnemyhas passed the checkpoint, we take him back on it
                this.transform.position = _nextCheckpoint.position;
            }   
        }

    }

    private void StartWalking()
    {
        GetComponent<Animator>().SetBool("Walking", true);
        _nextCheckpoint = GameManager.Instance.mainScene.checkpoints[0];
        ChangeDirection();
    }

    private void ChangeDirection()
    {
        _lastCheckpointReached = _nextCheckpoint;
        int indexOfCheckpoint = GameManager.Instance.mainScene.checkpoints.IndexOf(_lastCheckpointReached);
        if(indexOfCheckpoint == GameManager.Instance.mainScene.checkpoints.Count - 1)
        {
            ReachPathEnd();
        }
        else
        {
            this.transform.position = _nextCheckpoint.position;
            _lastCheckpointReached = _nextCheckpoint;
            _nextCheckpoint = GameManager.Instance.mainScene.checkpoints[indexOfCheckpoint + 1];
            this.transform.LookAt(_nextCheckpoint);
        }
    }

    private void ReachPathEnd()
    {
        this.GetComponent<Animator>().SetTrigger("Dying");
        _nextCheckpoint = null;
    }

    //Branch to event in dying animation
    public void OnDyingAnimationOver()
    {
        GameManager.Instance.Lo
    }
}
