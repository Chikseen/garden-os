using API.DB;
using Shared.Models;

public class GardenId
{
    public string Id = string.Empty;

    public GardenId()
    {

    }

    public GardenId(string id)
    {
        Id = id;
    }

    public void SetGardenIdByUser(string id)
    {
        List<Dictionary<string, string>> result = MainDB.Query(Querys.SelectGardenIDFromUsereId(id));
        Dictionary<string, string>? entry = result.FirstOrDefault();

        if (entry is not null)
            Id = entry["garden_id"].Replace("-", "");
        else
            throw new ArgumentException("Garden Id not found");
    }

    public void SetGardenIdByRPI(string id, bool replaceDashes = true)
    {
        List<Dictionary<string, string>> result = MainDB.Query(Querys.SelectGardenIDFromRpiId(id));
        Dictionary<string, string>? entry = result.FirstOrDefault();

        if (entry is not null)
        {
            if (replaceDashes)
                Id = entry["garden_id"].Replace("-", "");
            else Id = entry["garden_id"];
        }
        else
            throw new ArgumentException("Garden Id not found");
    }
}