using MainService.DB;

public class Garden
{
    public String Id = String.Empty;

    public Garden()
    {

    }

    public void SetGardenIdByUser(String id)
    {
        String query = $"SELECT GARDEN_ID FROM USERS WHERE ID = '{id}'";

        List<Dictionary<String, String>> result = MainDB.query(query);
        Dictionary<String, String>? entry = result.FirstOrDefault();

        if (entry is not null)
            this.Id = entry["garden_id"].Replace("-", "");
        else
            throw new ArgumentException("Garden Id not found");
    }

    public void SetGardenIdByRPI(String id)
    {
        String query = $"SELECT GARDEN_ID FROM RPIS WHERE ID = '{id}'";

        List<Dictionary<String, String>> result = MainDB.query(query);
        Dictionary<String, String>? entry = result.FirstOrDefault();

        if (entry is not null)
            this.Id = entry["garden_id"].Replace("-", "");
        else
            throw new ArgumentException("Garden Id not found");
    }
}