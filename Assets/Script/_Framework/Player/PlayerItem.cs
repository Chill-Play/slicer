


[System.Serializable]
public struct PlayerItem
{
    public string id;
    public string category;


    public override string ToString()
    {
        return "id : " + id + " , category" + category;
    }
}