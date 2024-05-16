
import SwiftUI

struct GardenItemComponent: View {
    @State var garden: Garden
    @ObservedObject var gardenOverviewViewModel: GardenOverviewViewModel = GardenOverviewViewModel.shared
    
    var body: some View {
        ZStack() {
            Rectangle()
                .frame(minWidth: 100, minHeight: 100)
                .foregroundColor(.box)
                .cornerRadius(10)
                .shadow(radius:2)
            VStack(){
                Text(garden.gardenName)
                    .frame(minWidth: 100)
                    .font(.title3)
                Text(garden.gardenID)
                    .frame(minWidth: 100)
                    .font(.caption)
            }
            .onTapGesture {
                gardenOverviewViewModel.setGarden(id: garden.gardenID)
            }
        }
        .aspectRatio(CGSize(width: 1, height: 1), contentMode: .fit)
    }
}
