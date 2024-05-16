import SwiftUI

struct MainView: View {
    @ObservedObject var loginViewModel: LoginViewModel = LoginViewModel.shared
    @ObservedObject var gardenOverviewViewModel: GardenOverviewViewModel = GardenOverviewViewModel.shared
    
    var body: some View {
        if (loginViewModel.isAuthenticated) {
            if (gardenOverviewViewModel.isGardenSelected) {
                DashboardView()
            }
            else {
                GardenOverviewView()
            }
        }
        else {
            LoginView()
        }
    }
}

#Preview {
    MainView()
}


