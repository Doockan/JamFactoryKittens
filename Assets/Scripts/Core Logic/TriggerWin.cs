using UnityEngine;
using System;
using System.Collections;
using System.Threading;

public class TriggerWin : MonoBehaviour
{
    public event Action WunAction;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<TypeGameObject>().typeObject == TypeObject.Player)
        {
            StartCoroutine(ActiveWinEvent());
        }
    }
    private IEnumerator ActiveWinEvent()
    {
        yield return new WaitForSeconds(1f);
        WunAction?.Invoke();

    }
}
