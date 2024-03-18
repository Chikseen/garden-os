using API.DB;
using API.Hub;
using API.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Shared;
using Shared.DeviceModels;
using Shared.Enums;
using Shared.Models;
using System.Reflection;

namespace API.Services
{
    public class DeviceService(IHubContext<MainHub, IMainHub> _hubContext) : IDeviceService
    {
        public void StoreData(DeviceInput model)
        {
            string query = IncommingDataQuery.InsertNewDataQuery(model);
            MainDB.Query(query);

            GardenId gardenId = new(model.GardenId);
            _hubContext.Clients.Group(gardenId.Id).SendCurrentDeviceData(model);
        }

        public void CreateNewDevice(DeviceCreateModel model)
        {
            string query = QueryService.CreateNewDevice(model);
            MainDB.Query(query);
        }

        public List<DeviceMeta> GetAllDevices(string gardenId)
        {
            string query = QueryService.GetAllDevices(gardenId);
            MainDB.Query(query);
            List<Dictionary<string, string>> result = MainDB.Query(query);

            List<DeviceMeta> response = new();
            foreach (var device in result)
            {
                response.Add(new(device));
            }
            return response;
        }

        public Device GetSensorValues(string gardenId, string deviceId, TimeFrameId timeFrameId)
        {
            string query = ViewQueryService.GetSensorValuessQuery(gardenId, deviceId);

            List<Dictionary<string, string>> result = MainDB.Query(query);
            if (result.Count > 0)
            {
                Device devices = new(result);
                return devices;
            }
            else
                return new();
        }

        public DetailedChartData GetDetailedTimeFrame(string gardenId, string deviceId, TimeFrame timeFrame)
        {
            string query = ViewQueryService.DetailedViewQuery(gardenId, deviceId, timeFrame);

            List<Dictionary<string, string>> result = MainDB.Query(query);

            var orderedListByDate = result.GroupBy(r => r[DeviceStatic.UploadDate].ToString()).ToList();

            List<Device> devicesList = new();
            foreach (var date in orderedListByDate)
            {
                devicesList.Add(new(date.ToList()));
            }
            devicesList.Sort((x, y) => DateTime.Compare(x.Date, y.Date));

            DetailedChartData chartData = new(devicesList);
            return chartData;
        }

        public Device UploadNewValue(NewManualValueModel model)
        {
            string query = IncommingDataQuery.UploadNewValueQuery(model);
            MainDB.Query(query);
            return GetSensorValues(model.GardenId, model.DeviceId, 0);
        }

        public void DeleteManualEntry(string gardenId, string entryId)
        {
            string query = QueryService.DeleteManualEntryQuery(gardenId, entryId);
            MainDB.Query(query);
        }
    }
}