import SwiftUI

struct GardenItemsComponent: View {
    @StateObject var gardenData: GardenOverviewViewModel
    
    var body: some View {
        LazyVGrid(columns: [GridItem(.adaptive(minimum: 100)), GridItem(.flexible())]) {
            GardenOverviewGridFixedComponent()
            if (gardenData.isLoading) {
                ProgressView()
            }
            else
            {
                if (gardenData.hasData){
                    ForEach((gardenData.gardenOverview!.gardenData), id: \.self.gardenID) { garden in
                        GardenItemComponent(garden: garden)
                    }
                }
            }
        }
        .padding(20)
        .accentColor(/*@START_MENU_TOKEN@*/.blue/*@END_MENU_TOKEN@*/)
        
    }
}

#Preview {
  /*  let gardenOne = Garden(gardenID: "123", gardenName: "hello", weatherLocationID: "123", hubs: [])
    let gardenOverviewResponse = GardenOverviewResponse(gardenData: [gardenOne])
    @State var deviceOverviewViewModel = DeviceOverviewViewModel(gardenOverview: gardenOverviewResponse)*/
    return GardenItemsComponent(gardenData: GardenOverviewViewModel())
}
