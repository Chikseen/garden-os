using MainService.DB;

public class Garden
{
    public String Id = String.Empty;

    public Garden()
    {

    }

    public void SetGardenIdByUser(String id)
    {
        List<Dictionary<String, String>> result = MainDB.query(Querys.SelectGardenIDFromUsereId(id));
        Dictionary<String, String>? entry = result.FirstOrDefault();

        if (entry is not null)
            this.Id = entry["garden_id"].Replace("-", "");
        else
            throw new ArgumentException("Garden Id not found");
    }

    public void SetGardenIdByRPI(String id, Boolean replaceDashes = true)
    {
        List<Dictionary<String, String>> result = MainDB.query(Querys.SelectGardenIDFromRpiId(id));
        Dictionary<String, String>? entry = result.FirstOrDefault();

        if (entry is not null)
        {
            if (replaceDashes)
                this.Id = entry["garden_id"].Replace("-", "");
            else this.Id = entry["garden_id"];
        }
        else
            throw new ArgumentException("Garden Id not found");
    }
}