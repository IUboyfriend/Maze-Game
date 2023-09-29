using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class LevelIntroduction : MonoBehaviour
{
    private string txt;
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log(EventSystem.current.gameObject);
            //Debug.Log(GetOverUI(gameObject));
            if (GetOverUI(gameObject)!= null)
            {

                if (GetOverUI(gameObject).name == "levelButton")
                {
                    transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = @"<color=#2fb9ff>Goal: Find the exit.</color>
Enemy: 
<color=#d49577>Boar</color>: When you get too close, it will deal continuous damage to you; when you are farther away it will rush at you and deal higher damage.




<color=#ff8f69>Hint</color>: Long-range attack.
";
                }
                else if (GetOverUI(gameObject).name == "levelButton (1)")
                {
                    transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = @"<color=#2fb9ff>Goal: Find the exit.</color>
Enemy: 
<color=#d49577>Boar</color>;

<color=#c5e663>Frog</color>: fires poisonous tracer bullets.

<color=#ca867f>Toad</color>: Red, which fires a venom that remains on the ground for some time.

<color=#ff8f69>Hint</color>: They are weak, get them done fast!

";
                }
                else if (GetOverUI(gameObject).name == "levelButton (2)")
                {
                    transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = @"<color=#2fb9ff>Goal: Find the exit.</color>
Enemy:
<color=#d49577>Boar</color>; <color=#c5e663>Frog</color>; <color=#ca867f>Toad</color>;

Monkey: learns to pick up branches as weapons! Every so often, a monkey with a branch will go into a berserk state, moving faster and attacking more powerfully. 

<color=#ff8f69>Hint</color>: You can use a full power attack (branch or stone) to knock the monkey's branch off. The monkey will no longer go into a berserk state.

";
                }
            }
            //txt=transform.GetChild(8).GetComponent<TextMeshProUGUI>().text;
        }
    }
    public static GameObject GetOverUI(GameObject canvas)
    {
        if (canvas.GetComponent<GraphicRaycaster>() == null) return null;
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(pointerEventData, results);
        if (results.Count != 0)
        {
            return results[0].gameObject;
        }
        return null;
    }
}
