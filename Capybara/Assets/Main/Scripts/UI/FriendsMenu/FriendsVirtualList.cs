using System.Collections.Generic;
using Main.Scripts.UI.Base;
using UnityEngine;

namespace Main.Scripts.UI.FriendsMenu
{
    public class FriendsVirtualList : VirtualList<FriendListInfo>
    {
        public override void InitItems(List<FriendListInfo> itemDatas)
        {
            Start();
            _itemsData.Clear();
            _itemsData.AddRange(itemDatas);
            _scroller.InitData(_itemsData.Count);
        }

        protected override void OnFillItem(int index, GameObject item)
        {
            FriendInfo friendInfo = item.GetComponent<FriendInfo>();
            if (friendInfo == null)
                return;
            friendInfo.UpdateInfo(_itemsData[index]);
        }
    }
}