
import Foundation
import JWTDecode

@MainActor
final class GardenOverviewViewModel: ObservableObject {
    @Published var gardenOverview: GardenOverviewResponse?
    @Published var isLoading: Bool = true
    @Published var hasData: Bool = false
    @Published var selectedGarden: String? = nil
    @Published var isGardenSelected: Bool = false
    
    static let shared = GardenOverviewViewModel()
    
    private var apiService = ApiService()
    
    func getGardenOverview() {
        Task {
            isLoading = true
            let json = try await apiService.Get(path: "/user/garden")
            let decoder = JSONDecoder()
            decoder.keyDecodingStrategy = .convertFromSnakeCase
            do {
                let gardenOverviewResponse = try JSONDecoder().decode(GardenOverviewResponse.self, from: json)
                gardenOverview = gardenOverviewResponse
                isLoading = false
                hasData = true
            } catch {
                print(error)
                isLoading = false
                hasData = false
            }
        }
    }
    
    func getUserName() -> String {
        let token = LoginViewModel.shared.GetToken()
        if (token != ""){
            let jwt = try? decode(jwt: token)
            if let name = jwt?["name"].string {
                return name
            }
        }
        return ""
    }
    
    func setGarden(id: String) {
        isGardenSelected = true
        selectedGarden = id
    }
}
