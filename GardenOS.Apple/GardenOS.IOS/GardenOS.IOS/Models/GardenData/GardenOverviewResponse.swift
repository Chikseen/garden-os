// This file was generated from JSON Schema using quicktype, do not modify it directly.
// To parse the JSON, add this file to your project and do:
//
//   let gardenOverviewResponse = try? JSONDecoder().decode(GardenOverviewResponse.self, from: jsonData)

import Foundation

// MARK: - GardenOverviewResponse
public struct GardenOverviewResponse: Codable {
    public let gardenData: [Garden]

    public enum CodingKeys: String, CodingKey {
        case gardenData = "garden_data"
    }

    public init(gardenData: [Garden]) {
        self.gardenData = gardenData
    }
}

// MARK: - GardenDatum
public struct Garden: Codable {
    public let gardenID, gardenName, weatherLocationID: String
    public let hubs: [JSONAny]

    public enum CodingKeys: String, CodingKey {
        case gardenID = "garden_id"
        case gardenName = "garden_name"
        case weatherLocationID = "weather_location_id"
        case hubs
    }

    public init(gardenID: String, gardenName: String, weatherLocationID: String, hubs: [JSONAny]) {
        self.gardenID = gardenID
        self.gardenName = gardenName
        self.weatherLocationID = weatherLocationID
        self.hubs = hubs
    }
}
