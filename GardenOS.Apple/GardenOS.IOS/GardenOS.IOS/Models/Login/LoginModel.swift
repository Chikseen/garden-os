//
//  LoginState.swift
//  GardenOS.IOS
//
//  Created by Tim Keutel on 08.04.24.
//

import Foundation
import SwiftUI

@Observable public class LoginModel: ObservableObject {
    static let shared = LoginModel()
    
    var IsAuthenticated: Bool = false
    var AccessToken: String? = nil
    
    init() {
        let data = TokenService.read() ?? Data("hi".utf8)
        let accessToken = String(data: data, encoding: .utf8)!
        if (accessToken == "") {
                SetToken(token: AccessTokenResponse(access_token: accessToken))
            }
    }
    
    func SetToken(token:AccessTokenResponse) {
        AccessToken = token.access_token
        IsAuthenticated = true
    }
    
    func GetToken() -> String {
         if (AccessToken == nil || AccessToken == "") {
            IsAuthenticated = false
            return ""
        }
        return AccessToken ?? ""
    }
    
    func Logout() -> Void {
        TokenService.delete()
        IsAuthenticated = false
        AccessToken = nil
    }
}
