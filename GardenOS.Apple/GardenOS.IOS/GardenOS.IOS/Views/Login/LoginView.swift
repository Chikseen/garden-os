import SwiftUI

struct LoginView: View {
    @ObservedObject var loginViewModel: LoginViewModel = LoginViewModel.shared
    
    var body: some View {
        VStack() {
            Spacer()
            VStack() {
                Image(uiImage: UIImage(named: "AppIconTransparent") ?? UIImage())
                    .resizable()
                    .frame(maxWidth: 100, maxHeight: 100)
                Text("GardenOS")
                    .font(.title)
                EmailInput(placeHolder: "Email", email: $loginViewModel.loginFormModel.email)
                PasswordInput(placeHolder: "Password", password: $loginViewModel.loginFormModel.password)
            }
            .padding(35)
            if (loginViewModel.isLoading) {
                ProgressView()
            }
            else {
                Button("Login") {
                    Task {
                        try await loginViewModel.GetAccessToken()
                    }
                }
            }
            Spacer()
        }.background(Color.box)
    }
}

#Preview {
    LoginView()
}
