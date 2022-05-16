using UnityEngine;
using UnityEngine.UI;

public class LevelMan : MonoBehaviour
{
    #region VAR_DECLORATION

    [SerializeField] private GameObject[] paths;

    [SerializeField] private Button[] levels;

    [SerializeField] private GameObject[] ticks;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        UpdateLevels();
    }

    private void UpdateLevels()
    {
        int level = Player.current.playerData.level;


        // Shows the levels that you have completed and show the paths when needed
        for (int i = 0; i <= level; i++)
        {
            if (i > 0)
            {
                paths[i - 1].SetActive(true);
                ticks[i - 1].SetActive(true);
            }

			levels[i].interactable = true;
			//if (i == 0) levels[i].interactable = true;
		}
    }
}
