using MainService.DB;
using Shared.Models;

public class Garden
{
    public string Id = string.Empty;

    public Garden()
    {

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