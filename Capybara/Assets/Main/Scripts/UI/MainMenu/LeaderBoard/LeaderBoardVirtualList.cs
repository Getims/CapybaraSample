using System.Collections.Generic;
using Main.Scripts.UI.Base;
using UnityEngine;

namespace Main.Scripts.UI.MainMenu.LeaderBoard
{
    public class LeaderBoardVirtualList : VirtualList<LeaderBoardItemData>
    {
        public override void InitItems(List<LeaderBoardItemData> itemDatas)
        {
            Start();
            _itemsData.Clear();
            _itemsData.AddRange(itemDatas);
            _scroller.InitData(_itemsData.Count);
        }

        public void SetBottomOffset(bool needOffset)
        {
            Vector2 offsetMin = _container.offsetMin;
            offsetMin.y = needOffset ? _itemHeight + 15 : 0;
            _container.offsetMin = offsetMin;
        }

        protected override void OnFillItem(int index, GameObject item)
        {
            LeaderBoardItem leaderBoardItem = item.GetComponent<LeaderBoardItem>();
            leaderBoardItem.UpdateInfo(_itemsData[index]);
        }
    }
}