public static class Querys
{
    public static String SelectGardenIDFromUsereId(String id)
    {
        return @$"SELECT GARDEN_ID FROM USERS WHERE ID = '{id}'";
    }

    public static String SelectGardenIDFromRpiId(String id)
    {
        return $"SELECT GARDEN_ID FROM RPIS WHERE ID = '{id}'";
    }
}