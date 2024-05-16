
import SwiftUI

struct GardenOverviewGridFixedComponent: View {
    @ObservedObject var loginViewModel: LoginViewModel = LoginViewModel.shared
    
    @State var requestId = ""
    var placeHolder: String = "GardenId"
    
    var body: some View {
        ZStack() {
            Rectangle()
                .frame(minWidth: 100, minHeight: 100)
                .foregroundColor(.box)
                .cornerRadius(10)
                .shadow(radius:2)
            VStack() {
                TextField(placeHolder, text: $requestId)
                    .textFieldStyle(.roundedBorder)
                Button("Request Access") {
                    
                }
                Spacer()
                Button("Logout") {
                    loginViewModel.Logout()
                }
            }.padding(10)
        }
        .aspectRatio(CGSize(width: 1, height: 1), contentMode: .fit)
    }
}

#Preview {
    GardenOverviewGridFixedComponent()
}
