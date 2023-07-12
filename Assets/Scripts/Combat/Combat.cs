using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Defines a Combat encounter. Only one combat encounter is allowed in a scene
public class Combat : MonoBehaviour
{
    //Serialize Field for now, will change
    //public List<Enemy> combatEnemies;

    [SerializeField] Canvas canvas;

    [SerializeField] 
    private List<Enemy> enemies;
    private List<Ally> allies;

    [SerializeField] private Slider healthBarPrefab;

    public static Combat Instance {get; private set;}
    private void Awake() 
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
        canvas.worldCamera = Camera.main;

        CombatManager.Instance.OnEndCombat += EndCombat;
        BattleGridManager.Instance.OnBattleGridCreated += InitalizeCombat;
    }

    public void InitalizeCombat()
    {
        allies = CombatManager.Instance.characters.OfType<Ally>().ToList();
        enemies = CombatManager.Instance.characters.OfType<Enemy>().ToList();

        Player.Instance.healthBar = Instantiate(healthBarPrefab.gameObject, CombatManager.Instance.gridWorldCanvas.transform).GetComponent<Slider>();
        Player.Instance.healthBar.transform.position = Player.Instance.transform.position + (Vector3.down * .5f);

        foreach(Ally ally in allies)
        {
            ally.healthBar = Instantiate(healthBarPrefab.gameObject, CombatManager.Instance.gridWorldCanvas.transform).GetComponent<Slider>();
            ally.healthBar.transform.position = ally.transform.position + (Vector3.down * .5f);
        }
        foreach(Enemy enemy in enemies)
        {
            enemy.healthBar = Instantiate(healthBarPrefab.gameObject, CombatManager.Instance.gridWorldCanvas.transform).GetComponent<Slider>();
            enemy.healthBar.transform.position = enemy.transform.position + (Vector3.down * .5f);
        }
    }

    public void CheckForCombatEnd()
    {
        Manager.Instance.GameState = (int) GameStateEnum.explore;
    }

    public void EndCombat()
    {

    }
}
