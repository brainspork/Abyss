using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    [Header("Magic Details")]
    public int maxMana;
    public Projectile projectile;
    [SerializeField] private GameObject manaUiBox;
    [SerializeField] private Sprite filledManaBubble;
    [SerializeField] private Sprite emptyManaBubble;

    private int currentMana;

    private GameManager gm;

    public override void Start()
    {
        base.Start();

        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        currentMana = maxMana;
        
        RenderMana();
    }

    public override void Update()
    {
        if (gm.gameState == GameState.Playing)
        {
            base.Update();

            direction = Input.GetAxis("Horizontal");

            if (Input.GetButtonDown("Fire1"))
            {
                HandleAttack();
            }

            if (Input.GetButtonDown("Fire2"))
            {
                FireProjectile();
            }

            if (Input.GetButtonDown("Jump"))
            {
                HandleJump();
            }
        }
    }

    private void FireProjectile()
    {
        if (projectile && currentMana > 0)
        {
            currentMana--;

            var proj = Instantiate(projectile, attackPoint.position, attackPoint.rotation);
            proj.direction = transform.localScale.x;
            proj.enemyLayer = enemyLayer;

            UpdateManaUI();
        }
    }

    private void RenderMana()
    {
        var halfImageWidth = 10;
        var imageRightPadding = 5;

        for(var i = 0; i < maxMana; i++)
        {
            var anchorX = halfImageWidth + (i * halfImageWidth * 2) + (i * imageRightPadding);
            CreateManaImage(new Vector2(anchorX, -halfImageWidth));
        }

        UpdateManaUI();
    }

    private void UpdateManaUI()
    {
        for(var i = 0; i < maxMana; i++)
        {
            var sprite = i <= currentMana - 1 ? filledManaBubble : emptyManaBubble;
            manaUiBox.transform.GetChild(i).GetComponent<Image>().sprite = sprite;
        }
    }

    private Image CreateManaImage(Vector2 anchor)
    {
        var bubble = new GameObject("ManaBubble", typeof(Image));

        bubble.transform.SetParent(manaUiBox.transform);
        bubble.transform.localPosition = Vector3.zero;
        bubble.transform.localScale = new Vector3(0.2f, 0.2f);

        var rect = bubble.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.anchoredPosition = anchor;

        var bubbleImage = bubble.GetComponent<Image>();
        bubbleImage.sprite = emptyManaBubble;

        return bubbleImage;
    }

    public void Heal(int amount)
    {
        var newHealth = currentHealth + amount;
        currentHealth = newHealth > maxHealth ? maxHealth : newHealth;
    }

    public void RechargeMana(int amount)
    {
        var newMana = currentMana + amount;
        currentMana = newMana > maxMana ? maxMana : newMana;

        UpdateManaUI();
    }
}
