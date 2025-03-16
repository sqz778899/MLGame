
public class Portal : BuildBase
{
    public void SelQuest(int questID)
    {
        QuestManager.Instance.SelectQuest(questID);
    }
}
