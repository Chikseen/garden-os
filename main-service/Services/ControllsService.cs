using MainService.DB;
using Shared.Models;

namespace MainService.Services
{
	public class ControllsService
	{
		public ControllerResponseModel GetControllsOverview(string gardenId)
		{
			string query = QueryService.GetControllerOverviewQuery(gardenId);
			List<Dictionary<string, string>> result = MainDB.Query(query);
			ControllerResponseModel response = new(result);
			return response;
		}
	}
}