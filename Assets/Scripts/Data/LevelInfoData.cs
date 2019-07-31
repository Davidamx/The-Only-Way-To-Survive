using System.Collections.Generic;

//ID : 关卡ID(int)
//LevelName : 关卡名称(string)
//SceneName : 场景名称(string)
//Description : 关卡描述(string)
//TimeLimit : 通关限定时间（秒）(float)
//Newspaper : 主界面的报纸名称(string)
//UnLockLevel : 解锁关卡(string)
//ResultWord : 通关时的结束语(string)

public class LevelInfoData : Singleton<LevelInfoData>
{
    public Dictionary<int, Dictionary<string, string>> data = new Dictionary<int, Dictionary<string, string>>()
    {
        {10001, new Dictionary<string, string>(){ {"LevelName", "Combo"}, {"SceneName", "10001"}, {"Description", "The enemy from the east declared war to the republic few days ago. Although the government promised that Combo city would not be occupied, the city meets the invaders soon after the rout of the troops at the front line."}, {"TimeLimit", "300"}, {"Newspaper", "Newspaper01"}, {"UnLockLevel", "Newaustin"}, {"ResultWord", "Mike gets on the train to Newaustin succsefuly. It will be much more easier to get back to Barea to find his family---- if Newaustin is still resisting."}, } },
        {10002, new Dictionary<string, string>(){ {"LevelName", "Newaustin"}, {"SceneName", "10002"}, {"Description", "Is was lucky for Mike to escape from Combo. However, enemies are now attacking Newaustin, the railway terminal of the east republic. Mike needs to get to another train station before republic lost it."}, {"TimeLimit", "300"}, {"Newspaper", "Newspaper02"}, {"UnLockLevel", "Barea"}, {"ResultWord", "That was close! The sound of the train gave Mike much more power and confidence. However, Mike and other passengers on the train are not the only one who speeding along to Barea,"}, } },
        {10003, new Dictionary<string, string>(){ {"LevelName", "Barea"}, {"SceneName", "10003"}, {"Description", "Mike finaly returned the capital of the city, the most important battle in this war are hapenning. Get to the other side of the city, to the direction where reinforcements will come, leave the city before it completely turned in to battle field of iron and bombs!"}, {"TimeLimit", "300"}, {"Newspaper", "Newspaper03"}, {"UnLockLevel", "Combo"}, {"ResultWord", "After Mike leave the battle field, he went to the evacuation zone to find his wife and daughter."}, } },
    };

}
