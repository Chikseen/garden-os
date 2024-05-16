import Foundation
import SwiftUI

class ApiService {
    private let url = "https://gardenapi.drunc.net"
    
    func Get(path: String) async throws -> Data {
        let url = URL(string: "\(url)\(path)")!

        let token = await LoginViewModel.shared.GetToken()
        
        var request = URLRequest(url: url)
        request.addValue("Bearer \(token)", forHTTPHeaderField: "Authorization")
        request.addValue("application/json", forHTTPHeaderField: "Content-Type")
        request.addValue("application/json", forHTTPHeaderField: "Accept")
        request.addValue("scope", forHTTPHeaderField: "openid")

        request.httpMethod = "GET"
        
        let session = URLSession.shared
        let (data, _) = try await session.data(for: request)

        return data
    }
}

