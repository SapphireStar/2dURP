using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockSpriteData",menuName = "Data/GameObject/BlockSpriteData", order = 1)]
public class BlockSpriteData : ScriptableObject
{
    [SerializeField]
    private BlockSpriteWrapper[] m_blockSpriteWrappers;

    private Dictionary<GridState, Sprite> m_spriteDict;

    public void GetSpriteViaGridState(GridState state, out Sprite sprite)
    {
        if(m_spriteDict == null)
        {
            m_spriteDict = new Dictionary<GridState, Sprite>();
        }
        if(!m_spriteDict.TryGetValue(state,out sprite))
        {
            for (int i = 0; i < m_blockSpriteWrappers.Length; i++)
            {
                if(state == m_blockSpriteWrappers[i].State)
                {
                    m_spriteDict.Add(state, m_blockSpriteWrappers[i].GridSprite);
                    sprite = m_spriteDict[state];
                }
            }
        }
    }
    

}

[Serializable]
public class BlockSpriteWrapper
{
    public GridState State;
    public Sprite GridSprite;
}
