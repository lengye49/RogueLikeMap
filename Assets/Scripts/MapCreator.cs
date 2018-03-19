using System;

public class MapCreator
{
    private MapInfo mapInfo;
    int bossNum;
    int monsterNum;
    int eventNum;

    public MapCreator(MapInfo info){
        mapInfo = info;
    }

    public Map CreateMap(){
        bossNum = CalculateMethod.GetRandomValue (mapInfo.BossCount);
        monsterNum = CalculateMethod.GetRandomValue (mapInfo.MonsterCount);
        eventNum = CalculateMethod.GetRandomValue (mapInfo.EventCount); 
    }

}


