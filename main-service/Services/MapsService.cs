
using MainService.DB;
using ExtensionMethods;

namespace Services.Maps
{
    public class MapsService
    {
        public MapJSON? SaveMap(String id, MapJSON json)
        {
            String query = @$"
                UPDATE maps
                SET json = '{json.Json}'
                WHERE id = '{id}'".Clean();
            MainDB.query(query);

            return GetMap(id);
        }

        public MapJSON? GetMap(String id)
        {
            String query = @$"
                SELECT * 
                FROM maps
                WHERE id = '{id}'".Clean();
            List<Dictionary<String, String>> result = MainDB.query(query);

            if (result.Count != 1)
                return null;

            return new MapJSON(result);
        }
    }
}