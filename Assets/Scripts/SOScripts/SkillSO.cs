using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillSO",menuName = "SO/Skills")]
public class SkillSO : ScriptableObject
{
    [SerializeField] string _name;
    [SerializeField] Sprite icon;
    [SerializeField] Sprite skillImage;
    [SerializeField] SkillBase skill;
    [SerializeField] float coolTime;

    public string SkillName => _name;
    public Sprite Icon => icon;
    public Sprite SkillImage => skillImage;
    public SkillBase Skill => skill;
    public float CoolTime => coolTime;
}