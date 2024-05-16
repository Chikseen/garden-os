import SwiftUI
import JWTDecode

struct GardenOverviewView: View {
    @StateObject private var gardenOverviewViewModel = GardenOverviewViewModel()
    
    var body: some View {
        VStack() {
            Text("Hello \(gardenOverviewViewModel.getUserName())")
                .font(.title)
            
            GardenItemsComponent(gardenData: gardenOverviewViewModel)
            .onAppear { gardenOverviewViewModel.getGardenOverview() }
        }
        Spacer()
    }
}

#Preview {
    GardenOverviewView()
}
