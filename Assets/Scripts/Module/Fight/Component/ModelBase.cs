using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelBase : MonoBehaviour
{
    public int Id;//ģ��Id
    public Dictionary<string, string> data;//���ݱ�

    public int Attack;//������
    public int Type;//����
    public int MaxHp;//���Ѫ��
    public int CurHp;//��ǰѪ��

    public int RowIndex;
    public int ColIndex;
    public SpriteRenderer bodySp;//����ͼƬ��Ⱦ���
    public Animator animator;//�������
    public bool isMoving;
    public bool isAttacking;

    private void Awake()
    {
        bodySp = transform.Find("body").GetComponent<SpriteRenderer>();
        animator = transform.Find("body").GetComponent<Animator>();
    }
    private void Start()
    {
        OnStart();
    }
    protected virtual void OnStart()
    {

    }

    private void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {

    }

    private void FixedUpdate()
    {
        OnFixedUpdate();
    }

    protected virtual void OnFixedUpdate()
    {

    }

    public void Init()
    {
        GameApp.MapManager.GetCellPos(this, transform.position);
    }

    public void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    //�ƶ���ָ���±�ĸ���
    public virtual bool Move(int rowIndex, int colIndex, float dt)
    {
        Vector3 pos = GameApp.MapManager.GetBlockPos(rowIndex, colIndex);

        pos.z = transform.position.z;

        if (transform.position.x > pos.x && transform.localScale.x > 0)
        {
            Flip();
        }
        if (transform.position.x < pos.x && transform.localScale.x < 0)
        {
            Flip();
        }

        if (Vector3.Distance(transform.position, pos) <= 0.02f)
        {
            transform.position = pos;
            return true;
        }

        transform.position = Vector3.MoveTowards(transform.position, pos, dt);

        return false;
    }
     
    public void PlayAni(string aniName)
    {
        animator.Play(aniName);
    }

    public void ChangePos(int row, int col)
    {
        transform.position = GameApp.MapManager.GetBlockPos(row, col);
    }

    public void Face2Cell(int row, int col)
    {
        Vector3 pos = GameApp.MapManager.GetBlockPos(row, col);

        pos.z = transform.position.z;

        if (transform.position.x > pos.x && transform.localScale.x > 0)
        {
            Flip();
        }
        if (transform.position.x < pos.x && transform.localScale.x < 0)
        {
            Flip();
        }
    }
}
