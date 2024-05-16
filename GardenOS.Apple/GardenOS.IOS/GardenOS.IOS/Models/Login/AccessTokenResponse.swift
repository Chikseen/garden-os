
import Foundation

struct AccessTokenResponse :Codable {
   public let accessToken: String
    
    public enum CodingKeys: String, CodingKey {
        case accessToken = "access_token"
    }

    public init(accessToken: String) {
        self.accessToken = accessToken
    }
}
