using System.Collections.Generic;
public class MapInfo{
	public string MapName;//地图的图片
	public int RowsCount;
	public int ColumnsCount;
	public string BossRange;//格式 1|8 1~8随机
	public Dictionary<int,int> BossList;

	public string BlockRange;//格式 1|8 1~8随机

	public string MonsterRange;//格式 1|8 1~8随机
	public Dictionary<int,int> MonsterList;

	public string EventRange;//格式 1|8 1~8随机
	public Dictionary<int,int> EventConfirmed;//id,数量
	public Dictionary<int,int> EventPossible;//id,权重
	/*	任务、特殊任务
	 *	商店、特殊商店
	 *	随机物品（加血、加食物、内力上限、血上限、临时buff）、特殊物品
	 *	打造、特殊打造
	 *	删卡（顿悟）
	 * */

	public MapInfo(string mapId){
        MapName = mapId;
        RowsCount = 7;
        ColumnsCount = 7;
        BossRange="1|1";
        BlockRange="12|15";
        MonsterRange="5|10";
        EventRange="2|5";
	}
}

public class Map{
    
}



