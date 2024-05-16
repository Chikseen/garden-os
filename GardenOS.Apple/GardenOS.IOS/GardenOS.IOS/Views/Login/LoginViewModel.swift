import Foundation
import SwiftUI

@MainActor
final class LoginViewModel: ObservableObject {
    @Published var isLoading = false
    @Published var isAuthenticated: Bool = false
    @Published var accessToken: String? = nil
    @Published var loginFormModel = LoginFormModel()
    
    static let shared = LoginViewModel()
    
    init() {
        let data = TokenService.read() ?? Data("".utf8)
        let accessToken = String(data: data, encoding: .utf8)!
        if (accessToken != "") {
            SetToken(token: AccessTokenResponse(accessToken: accessToken))
        }
    }
    
    func SetToken(token:AccessTokenResponse) {
        accessToken = token.accessToken
        isAuthenticated = true
    }
    
    func GetToken() -> String {
         if (accessToken == nil || accessToken == "") {
            isAuthenticated = false
            return ""
        }
        return accessToken ?? ""
    }
    
    func Logout() -> Void {
        TokenService.delete()
        isAuthenticated = false
        accessToken = nil
    }
    
    func GetAccessToken() async throws {
        let requestHeaders: [String:String] =
        ["Content-Type": "application/x-www-form-urlencoded"]
        
        var requestBody = URLComponents()
        requestBody.queryItems =
        [URLQueryItem(name: "grant_type", value: "password"),
         URLQueryItem(name: "client_id", value: "client"),
         URLQueryItem(name: "username", value: loginFormModel.email),
         URLQueryItem(name: "password", value: loginFormModel.password)]
        
        let url = URL(string: "https://auth.garden-os.com/realms/GardenOS-PROD/protocol/openid-connect/token")!
        var request = URLRequest(url: url)
        request.httpMethod = "POST"
        request.allHTTPHeaderFields = requestHeaders
        request.httpBody = requestBody.query?.data(using: .utf8)
        
        URLSession.shared.dataTask(with: request) { (data, response, error) in
            if let data = data {
                if let string = String(data: data, encoding: .utf8) {
                    let decoder = JSONDecoder()
                       let jsonData = Data(string.utf8)
                       do {
                           let token = try decoder.decode(AccessTokenResponse.self, from: jsonData)
                           Task 
                           {
                               await self.SetToken(token: token)
                           }
                           let data = Data(token.accessToken.utf8)
                           TokenService.save(data)
                       } catch {
                           print(error.localizedDescription)
                       }
                   }
               }
        }.resume()
    }
}
