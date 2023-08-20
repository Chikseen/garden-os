namespace shared_data.Models
{
    public static class Querys
    {
        public static string SelectGardenIDFromUsereId(string id)
        {
            return @$"SELECT GARDEN_ID FROM USERS WHERE ID = '{id}'";
        }

        public static string SelectGardenIDFromRpiId(string id)
        {
            return $"SELECT GARDEN_ID FROM RPIS WHERE ID = '{id}'";
        }
    }
}