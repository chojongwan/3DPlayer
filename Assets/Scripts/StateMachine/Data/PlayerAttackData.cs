using UnityEngine;
using System;
using System.Collections.Generic;

namespace MyGame.Data
{
    [Serializable]
    public class PlayerAttackData
    {
        [field: SerializeField] public List<AttackInfoData> AttackInfoDatas { get; private set; }
        public int GetAttackInfoCount() { return AttackInfoDatas.Count; }
        public AttackInfoData GetAttackInfo(int index) { return AttackInfoDatas[index]; }
    }

    [Serializable]
    public class AttackInfoData
    {
        [field: SerializeField] public string AttackName { get; private set; }
        [field: SerializeField] public float ComboStateIndex { get; private set; }
        [field: SerializeField][field: Range(0f, 1f)] public float ComboTransitionTime { get; private set; }
        [field: SerializeField][field: Range(0f, 3f)] public float ForceTransitionTime { get; private set; }
        [field: SerializeField][field: Range(-10f, 10f)] public float Force { get; private set; }
        [field: SerializeField] public int Damager { get; private set; }
        [field: SerializeField][field: Range(0f, 1f)] public float Dealing_Start_TransitionTime { get; private set; }
        [field: SerializeField][field: Range(0f, 1f)] public float Dealing_End_TransitionTime { get; private set; }
    }
}
